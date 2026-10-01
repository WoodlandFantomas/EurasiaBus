using EurasiaBus.Data;
using EurasiaBus.Models;
using EurasiaBus.Services;
using EurasiaBus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EurasiaBus.Controllers;

[Authorize]
public class PaymentController(
ApplicationDbContext db,
PdfTicketService pdfTicketService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(int id)
    {
        var b = await db.Bookings
        .Include(x => x.BusTrip)
        .FirstOrDefaultAsync(x => x.BookingId == id);


    if (b == null)
            return NotFound();

        return View(new PaymentViewModel
        {
            BookingId = id,
            Amount = b.TicketPrice,
            PaymentMethod = "Card"
        });
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Pay(PaymentViewModel model)
    {
        var b = await db.Bookings
            .FirstOrDefaultAsync(x => x.BookingId == model.BookingId);

        if (b == null)
            return NotFound();


        db.Payments.Add(new Payment
        {
            BookingId = b.BookingId,
            Amount = b.TicketPrice,
            PaymentMethod = model.PaymentMethod,
            Status = "Paid",
            TransactionId =
                Guid.NewGuid()
                    .ToString("N")[..12]
                    .ToUpperInvariant()
        });


        b.Status = "Confirmed";
        b.IsActive = true;


        await db.SaveChangesAsync();


        return RedirectToAction(
            "Success",
            new { id = b.BookingId });
    }


    public async Task<IActionResult> Success(int id)
    {
        var b = await db.Bookings
            .Include(x => x.BusTrip)
                .ThenInclude(x => x.DepartureStop)

            .Include(x => x.BusTrip)
                .ThenInclude(x => x.ArrivalStop)

            .Include(x => x.BusTripSeat)

            .Include(x => x.Passenger)

            .Include(x => x.Payment)

            .FirstOrDefaultAsync(x =>
                x.BookingId == id);


        if (b == null)
            return NotFound();


        return View(b);
    }


    [HttpGet]
    public async Task<IActionResult> TicketPdf(int id)
    {
        var b = await db.Bookings
            .Include(x => x.BusTrip)
                .ThenInclude(x => x.DepartureStop)

            .Include(x => x.BusTrip)
                .ThenInclude(x => x.ArrivalStop)

            .Include(x => x.BusTrip)
                .ThenInclude(x => x.Bus)

            .Include(x => x.BusTripSeat)

            .Include(x => x.Passenger)

            .Include(x => x.Payment)

            .FirstOrDefaultAsync(x =>
                x.BookingId == id);


        if (b == null)
            return NotFound();


        var ticket = new TicketPdfViewModel
        {
            BookingId = b.BookingId,

            PnrCode = b.PnrCode ?? "",

            PassengerName =
                $"{b.Passenger?.FirstName} {b.Passenger?.LastName}".Trim(),

            Email = b.Passenger?.Email ?? "",

            Phone = b.Passenger?.PhoneNumber ?? "",

            PassportNo = b.Passenger?.PassportNo ?? "",

            FromCity = b.BusTrip?.DepartureStop?.City ?? "",

            ToCity = b.BusTrip?.ArrivalStop?.City ?? "",

            DepartureDateTime =
                b.BusTrip?.DepartureDateTime ?? DateTime.MinValue,

            BusName =
                b.BusTrip?.Bus?.Name ?? "",

            SeatNumber =
                b.BusTripSeat?.SeatNumber ?? "",

            Price = b.TicketPrice,

            PaymentStatus =
                b.Payment?.Status ?? b.Status ?? "",

            PaymentMethod =
                b.Payment?.PaymentMethod ?? "",

            TransactionId =
                b.Payment?.TransactionId ?? "",

            BookingDate = b.BookingDate
        };


        var pdf = pdfTicketService.Generate(ticket);


        return File(
            pdf,
            "application/pdf",
            $"EurasiaBus-Ticket-{ticket.PnrCode}.pdf");
    }


}
