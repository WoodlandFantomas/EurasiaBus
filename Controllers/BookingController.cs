using EurasiaBus.Data;
using EurasiaBus.Models;
using EurasiaBus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EurasiaBus.Controllers;
[Authorize]
public class BookingController(ApplicationDbContext db, UserManager<ApplicationUser> users) : Controller
{
    public async Task<IActionResult> SelectSeats(int id, int passengers = 1)
    {
        var trip = await db.BusTrips.Include(x => x.Bus).Include(x => x.DepartureStop).Include(x => x.ArrivalStop).Include(x => x.Seats).FirstOrDefaultAsync(x => x.BusTripId == id && x.IsActive);
        if (trip == null) return NotFound();
        ViewBag.Passengers = passengers; return View(trip);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> PassengerDetails(SeatSelectionViewModel model)
    {
        var seats = await db.BusTripSeats.Where(x => model.SelectedSeatIds.Contains(x.BusTripSeatId) && x.BusTripId == model.TripId && x.IsAvailable).ToListAsync();
        if (seats.Count != model.SelectedSeatIds.Count) return BadRequest("One or more seats are no longer available.");
        return View(new PassengerDetailsViewModel { TripId = model.TripId, SeatIds = model.SelectedSeatIds });
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBooking(PassengerDetailsViewModel model)
    {
        if (!ModelState.IsValid) return View("PassengerDetails", model);
        var user = await users.GetUserAsync(User); if (user == null) return Challenge();
        await using var tx = await db.Database.BeginTransactionAsync();
        var trip = await db.BusTrips.Include(x => x.Seats).FirstOrDefaultAsync(x => x.BusTripId == model.TripId);
        if (trip == null) return NotFound();
        var seats = trip.Seats.Where(x => model.SeatIds.Contains(x.BusTripSeatId) && x.IsAvailable).ToList();
        if (seats.Count != model.SeatIds.Count) { ModelState.AddModelError("", "Selected seats are no longer available."); return View("PassengerDetails", model); }
        var passenger = new Passenger { UserId = user.Id, FirstName = model.FirstName, LastName = model.LastName, Email = model.Email, PhoneNumber = model.Phone, PassportNo = model.PassportNo, BirthDate = model.BirthDate };
        db.Passengers.Add(passenger); await db.SaveChangesAsync();
        var pnr = GeneratePnr();
        foreach (var seat in seats)
        {
            seat.IsAvailable = false;
            db.Bookings.Add(new Booking { UserId = user.Id, PassengerId = passenger.PassengerId, BusTripId = trip.BusTripId, BusTripSeatId = seat.BusTripSeatId, Status = "Pending", PnrCode = pnr, TicketPrice = seat.Price > 0 ? seat.Price : trip.Price });
        }
        await db.SaveChangesAsync(); await tx.CommitAsync();
        var booking = await db.Bookings.Where(x => x.PnrCode == pnr).OrderBy(x => x.BookingId).FirstAsync();
        return RedirectToAction("Index", "Payment", new { id = booking.BookingId });
    }
    public async Task<IActionResult> MyBookings() { var u = await users.GetUserAsync(User); if (u == null) return Challenge(); var list = await db.Bookings.Include(x => x.BusTrip).ThenInclude(x => x.DepartureStop).Include(x => x.BusTrip).ThenInclude(x => x.ArrivalStop).Include(x => x.BusTripSeat).Include(x => x.Passenger).Where(x => x.UserId == u.Id).OrderByDescending(x => x.BookingDate).ToListAsync(); return View(list); }
    private static string GeneratePnr() => Guid.NewGuid().ToString("N")[..6].ToUpperInvariant();
}
