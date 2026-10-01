using EurasiaBus.Data;
using EurasiaBus.Models;
using EurasiaBus.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EurasiaBus.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController(
ApplicationDbContext db,
RouteTextService routeTextService) : Controller
{
// =========================================================
// DASHBOARD
// =========================================================


public async Task<IActionResult> Dashboard()
    {
        ViewBag.TotalBookings =
            await db.Bookings.CountAsync();

        ViewBag.ActiveBookings =
            await db.Bookings.CountAsync(x => x.IsActive);

        ViewBag.CancelledBookings =
            await db.Bookings.CountAsync(x => !x.IsActive);

        ViewBag.TotalBuses =
            await db.Buses.CountAsync();

        ViewBag.TotalBusTrips =
            await db.BusTrips.CountAsync();

        ViewBag.ActiveBusTrips =
            await db.BusTrips.CountAsync(x => x.IsActive);

        ViewBag.TotalBusStops =
            await db.BusStops.CountAsync();

        ViewBag.TotalUsers =
            await db.Users.CountAsync();

        ViewBag.TotalPayments =
            await db.Payments.CountAsync();

        ViewBag.PaidPayments =
            await db.Payments.CountAsync(x => x.Status == "Paid");

        ViewBag.TotalRevenue =
            await db.Payments
                .Where(x => x.Status == "Paid")
                .Select(x => (decimal?)x.Amount)
                .SumAsync() ?? 0m;

        ViewBag.AveragePayment =
            await db.Payments
                .Where(x => x.Status == "Paid")
                .Select(x => (decimal?)x.Amount)
                .AverageAsync() ?? 0m;

        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        ViewBag.TodayBookings =
            await db.Bookings.CountAsync(x =>
                x.BookingDate >= today &&
                x.BookingDate < tomorrow);

        ViewBag.LatestBookings =
            await db.Bookings
                .Include(x => x.User)
                .Include(x => x.Passenger)
                .Include(x => x.BusTrip)
                    .ThenInclude(x => x.Bus)
                .Include(x => x.BusTrip)
                    .ThenInclude(x => x.DepartureStop)
                .Include(x => x.BusTrip)
                    .ThenInclude(x => x.ArrivalStop)
                .Include(x => x.BusTripSeat)
                .OrderByDescending(x => x.BookingDate)
                .Take(10)
                .ToListAsync();

        return View();
    }


    // =========================================================
    // ROUTES TXT - GET
    // =========================================================

    [HttpGet]
    public IActionResult RoutesText()
    {
        try
        {
            var routes = routeTextService.GetRoutes();

            ViewBag.FileExists =
                routeTextService.FileExists();

            return View(routes);
        }
        catch (Exception ex)
        {
            ViewBag.FileExists = false;

            ViewBag.RouteError =
                "Не удалось загрузить маршруты: " + ex.Message;

            return View(new List<EurasiaBus.Models.Route>());
        }
    }


    // =========================================================
    // ROUTES TXT - UPLOAD
    // =========================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UploadRoutesText(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            TempData["Error"] =
                "Выберите TXT-файл Routes.txt.";

            return RedirectToAction(nameof(RoutesText));
        }

        var extension =
            Path.GetExtension(file.FileName);

        if (!extension.Equals(
                ".txt",
                StringComparison.OrdinalIgnoreCase))
        {
            TempData["Error"] =
                "Разрешена загрузка только TXT-файлов.";

            return RedirectToAction(nameof(RoutesText));
        }

        try
        {
            routeTextService.SaveUploadedFile(file);

            TempData["Success"] =
                "Файл Routes.txt успешно загружен.";

            return RedirectToAction(nameof(RoutesText));
        }
        catch (Exception ex)
        {
            TempData["Error"] =
                $"Ошибка загрузки файла: {ex.Message}";

            return RedirectToAction(nameof(RoutesText));
        }
    }


    // =========================================================
    // ROUTES TXT - DOWNLOAD
    // =========================================================

    [HttpGet]
    public IActionResult DownloadRoutesText()
    {
        if (!routeTextService.FileExists())
        {
            TempData["Error"] =
                "Файл Routes.txt ещё не загружен.";

            return RedirectToAction(nameof(RoutesText));
        }

        try
        {
            byte[] bytes =
                routeTextService.GetFileBytes();

            if (bytes.Length == 0)
            {
                TempData["Error"] =
                    "Файл Routes.txt пуст или недоступен.";

                return RedirectToAction(nameof(RoutesText));
            }

            return File(
                bytes,
                "text/plain; charset=utf-8",
                "Routes.txt");
        }
        catch (Exception ex)
        {
            TempData["Error"] =
                $"Ошибка скачивания файла: {ex.Message}";

            return RedirectToAction(nameof(RoutesText));
        }
    }


    // =========================================================
    // BUSES
    // =========================================================

    [HttpGet]
    public async Task<IActionResult> Buses()
    {
        var buses =
            await db.Buses
                .Include(x => x.Trips)
                .OrderBy(x => x.Name)
                .ToListAsync();

        return View(buses);
    }


    // =========================================================
    // ADD BUS - GET
    // =========================================================

    [HttpGet]
    public IActionResult AddBus()
    {
        return View();
    }


    // =========================================================
    // ADD BUS - POST
    // =========================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddBus(Bus bus)
    {
        if (!ModelState.IsValid)
        {
            return View(bus);
        }

        bus.IsActive = true;

        db.Buses.Add(bus);

        await db.SaveChangesAsync();

        TempData["Success"] =
            "Автобус успешно добавлен.";

        return RedirectToAction(nameof(Buses));
    }


    // =========================================================
    // DELETE BUS
    // =========================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBus(int id)
    {
        var bus =
            await db.Buses
                .Include(x => x.Trips)
                .FirstOrDefaultAsync(
                    x => x.BusId == id);

        if (bus == null)
        {
            TempData["Error"] =
                "Автобус не найден.";

            return RedirectToAction(nameof(Buses));
        }

        if (bus.Trips != null &&
            bus.Trips.Any())
        {
            TempData["Error"] =
                "Нельзя удалить автобус, у которого существуют рейсы. Сначала удалите связанные рейсы.";

            return RedirectToAction(nameof(Buses));
        }

        db.Buses.Remove(bus);

        await db.SaveChangesAsync();

        TempData["Success"] =
            "Автобус успешно удалён.";

        return RedirectToAction(nameof(Buses));
    }


    // =========================================================
    // BUS STOPS
    // =========================================================

    [HttpGet]
    public async Task<IActionResult> BusStops()
    {
        var stops =
            await db.BusStops
                .OrderBy(x => x.City)
                .ThenBy(x => x.Name)
                .ToListAsync();

        return View(stops);
    }


    // =========================================================
    // STOPS
    // Алиас для /Admin/Stops
    // =========================================================

    [HttpGet]
    public async Task<IActionResult> Stops()
    {
        var stops =
            await db.BusStops
                .OrderBy(x => x.City)
                .ThenBy(x => x.Name)
                .ToListAsync();

        return View("BusStops", stops);
    }


    // =========================================================
    // ADD BUS STOP - GET
    // =========================================================

    [HttpGet]
    public IActionResult AddBusStop()
    {
        return View();
    }


    // =========================================================
    // ADD BUS STOP - POST
    // =========================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddBusStop(BusStop stop)
    {
        if (!ModelState.IsValid)
        {
            return View(stop);
        }

        stop.IsActive = true;

        db.BusStops.Add(stop);

        await db.SaveChangesAsync();

        TempData["Success"] =
            "Автобусная остановка успешно добавлена.";

        return RedirectToAction(nameof(BusStops));
    }


    // =========================================================
    // DELETE BUS STOP
    // =========================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBusStop(int id)
    {
        var stop =
            await db.BusStops
                .FirstOrDefaultAsync(
                    x => x.BusStopId == id);

        if (stop == null)
        {
            TempData["Error"] =
                "Остановка не найдена.";

            return RedirectToAction(nameof(BusStops));
        }

        var hasTrips =
            await db.BusTrips.AnyAsync(x =>
                x.DepartureStopId == id ||
                x.ArrivalStopId == id);

        if (hasTrips)
        {
            TempData["Error"] =
                "Нельзя удалить остановку, которая используется в рейсах.";

            return RedirectToAction(nameof(BusStops));
        }

        db.BusStops.Remove(stop);

        await db.SaveChangesAsync();

        TempData["Success"] =
            "Остановка успешно удалена.";

        return RedirectToAction(nameof(BusStops));
    }


    // =========================================================
    // BUS TRIPS
    // =========================================================

    [HttpGet]
    public async Task<IActionResult> BusTrips()
    {
        var trips =
            await db.BusTrips
                .Include(x => x.Bus)
                .Include(x => x.DepartureStop)
                .Include(x => x.ArrivalStop)
                .Include(x => x.Seats)
                .OrderBy(x => x.DepartureDateTime)
                .ToListAsync();

        return View(trips);
    }


    // =========================================================
    // TRIPS
    // Алиас для /Admin/Trips
    // =========================================================

    [HttpGet]
    public async Task<IActionResult> Trips()
    {
        var trips =
            await db.BusTrips
                .Include(x => x.Bus)
                .Include(x => x.DepartureStop)
                .Include(x => x.ArrivalStop)
                .Include(x => x.Seats)
                .OrderBy(x => x.DepartureDateTime)
                .ToListAsync();

        return View("BusTrips", trips);
    }


    // =========================================================
    // ADD BUS TRIP - GET
    // =========================================================

    [HttpGet]
    public async Task<IActionResult> AddBusTrip()
    {
        await LoadBusTripLists();

        return View();
    }


    // =========================================================
    // ADD BUS TRIP - POST
    // =========================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddBusTrip(BusTrip trip)
    {
        ModelState.Remove(nameof(BusTrip.Bus));
        ModelState.Remove(nameof(BusTrip.DepartureStop));
        ModelState.Remove(nameof(BusTrip.ArrivalStop));
        ModelState.Remove(nameof(BusTrip.Seats));

        if (trip.DepartureStopId == trip.ArrivalStopId)
        {
            ModelState.AddModelError(
                "",
                "Остановка отправления и остановка прибытия не могут совпадать.");
        }

        if (trip.ArrivalDateTime <= trip.DepartureDateTime)
        {
            ModelState.AddModelError(
                "",
                "Время прибытия должно быть позже времени отправления.");
        }

        if (!ModelState.IsValid)
        {
            await LoadBusTripLists(
                trip.BusId,
                trip.DepartureStopId,
                trip.ArrivalStopId);

            return View(trip);
        }

        var bus =
            await db.Buses
                .FirstOrDefaultAsync(
                    x => x.BusId == trip.BusId);

        if (bus == null)
        {
            ModelState.AddModelError(
                "",
                "Выбранный автобус не найден.");

            await LoadBusTripLists(
                trip.BusId,
                trip.DepartureStopId,
                trip.ArrivalStopId);

            return View(trip);
        }

        var departureStop =
            await db.BusStops
                .FirstOrDefaultAsync(
                    x => x.BusStopId == trip.DepartureStopId);

        var arrivalStop =
            await db.BusStops
                .FirstOrDefaultAsync(
                    x => x.BusStopId == trip.ArrivalStopId);

        if (departureStop == null ||
            arrivalStop == null)
        {
            ModelState.AddModelError(
                "",
                "Не удалось найти выбранные остановки.");

            await LoadBusTripLists(
                trip.BusId,
                trip.DepartureStopId,
                trip.ArrivalStopId);

            return View(trip);
        }

        trip.IsActive = true;

        db.BusTrips.Add(trip);

        await db.SaveChangesAsync();

        for (int i = 1; i <= bus.SeatCount; i++)
        {
            var seat = new BusTripSeat
            {
                BusTripId = trip.BusTripId,
                SeatNumber = i.ToString(),
                IsAvailable = true,
                Price = trip.Price
            };

            db.BusTripSeats.Add(seat);
        }

        await db.SaveChangesAsync();

        TempData["Success"] =
            "Автобусный рейс успешно добавлен.";

        return RedirectToAction(nameof(BusTrips));
    }


    // =========================================================
    // DELETE BUS TRIP
    // =========================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBusTrip(int id)
    {
        var trip =
            await db.BusTrips
                .Include(x => x.Seats)
                .FirstOrDefaultAsync(
                    x => x.BusTripId == id);

        if (trip == null)
        {
            TempData["Error"] =
                "Рейс не найден.";

            return RedirectToAction(nameof(BusTrips));
        }

        var hasBookings =
            await db.Bookings.AnyAsync(
                x => x.BusTripId == id);

        if (hasBookings)
        {
            TempData["Error"] =
                "Нельзя удалить рейс, у которого существуют бронирования.";

            return RedirectToAction(nameof(BusTrips));
        }

        if (trip.Seats != null &&
            trip.Seats.Any())
        {
            db.BusTripSeats.RemoveRange(trip.Seats);
        }

        db.BusTrips.Remove(trip);

        await db.SaveChangesAsync();

        TempData["Success"] =
            "Автобусный рейс успешно удалён.";

        return RedirectToAction(nameof(BusTrips));
    }


    // =========================================================
    // BUS TRIP DROPDOWNS
    // =========================================================

    private async Task LoadBusTripLists(
        int? selectedBusId = null,
        int? selectedDepartureStopId = null,
        int? selectedArrivalStopId = null)
    {
        var buses =
            await db.Buses
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .ToListAsync();

        var stops =
            await db.BusStops
                .Where(x => x.IsActive)
                .OrderBy(x => x.City)
                .ThenBy(x => x.Name)
                .ToListAsync();

        ViewBag.Buses =
            new SelectList(
                buses,
                "BusId",
                "Name",
                selectedBusId);

        ViewBag.DepartureStops =
            new SelectList(
                stops,
                "BusStopId",
                "Name",
                selectedDepartureStopId);

        ViewBag.ArrivalStops =
            new SelectList(
                stops,
                "BusStopId",
                "Name",
                selectedArrivalStopId);
    }


    // =========================================================
    // BOOKINGS
    // =========================================================

    [HttpGet]
    public async Task<IActionResult> Bookings()
    {
        var bookings =
            await db.Bookings
                .Include(x => x.User)
                .Include(x => x.Passenger)
                .Include(x => x.BusTrip)
                    .ThenInclude(x => x.Bus)
                .Include(x => x.BusTrip)
                    .ThenInclude(x => x.DepartureStop)
                .Include(x => x.BusTrip)
                    .ThenInclude(x => x.ArrivalStop)
                .Include(x => x.BusTripSeat)
                .OrderByDescending(x => x.BookingDate)
                .ToListAsync();

        return View(bookings);
    }


    // =========================================================
    // USERS
    // =========================================================

    [HttpGet]
    public async Task<IActionResult> Users()
    {
        var users =
            await db.Users
                .OrderBy(x => x.Email)
                .ToListAsync();

        return View(users);
    }


    // =========================================================
    // PAYMENTS
    // =========================================================

    [HttpGet]
    public async Task<IActionResult> Payments()
    {
        var payments =
            await db.Payments
                .Include(x => x.Booking)
                .OrderByDescending(x => x.PaymentId)
                .ToListAsync();

        return View(payments);
    }


    // =========================================================
    // CANCEL BOOKING
    // =========================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelBooking(int id)
    {
        var booking =
            await db.Bookings
                .Include(x => x.BusTripSeat)
                .FirstOrDefaultAsync(
                    x => x.BookingId == id);

        if (booking == null)
        {
            return NotFound();
        }

        if (!booking.IsActive)
        {
            TempData["Error"] =
                "Это бронирование уже отменено.";

            return RedirectToAction(nameof(Bookings));
        }

        booking.IsActive = false;

        booking.Status = "Cancelled";

        if (booking.BusTripSeat != null)
        {
            booking.BusTripSeat.IsAvailable = true;
        }

        await db.SaveChangesAsync();

        TempData["Success"] =
            "Бронирование успешно отменено.";

        return RedirectToAction(nameof(Bookings));
    }


    // =========================================================
    // ACTIVATE / RESTORE BOOKING
    // =========================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ActivateBooking(int id)
    {
        var booking =
            await db.Bookings
                .Include(x => x.BusTripSeat)
                .FirstOrDefaultAsync(
                    x => x.BookingId == id);

        if (booking == null)
        {
            return NotFound();
        }

        if (booking.IsActive)
        {
            TempData["Error"] =
                "Это бронирование уже активно.";

            return RedirectToAction(nameof(Bookings));
        }

        if (booking.BusTripSeat == null)
        {
            TempData["Error"] =
                "Невозможно восстановить бронирование: место не найдено.";

            return RedirectToAction(nameof(Bookings));
        }

        var seatAlreadyBooked =
            await db.Bookings.AnyAsync(x =>
                x.BookingId != booking.BookingId &&
                x.BusTripSeatId == booking.BusTripSeatId &&
                x.IsActive);

        if (seatAlreadyBooked)
        {
            TempData["Error"] =
                "Невозможно восстановить бронирование: это место уже занято.";

            return RedirectToAction(nameof(Bookings));
        }

        booking.IsActive = true;

        booking.Status = "Confirmed";

        booking.BusTripSeat.IsAvailable = false;

        await db.SaveChangesAsync();

        TempData["Success"] =
            "Бронирование успешно восстановлено.";

        return RedirectToAction(nameof(Bookings));
    }


    // =========================================================
    // DELETE CANCELLED BOOKING
    // =========================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        var booking =
            await db.Bookings
                .Include(x => x.BusTripSeat)
                .FirstOrDefaultAsync(
                    x => x.BookingId == id);

        if (booking == null)
        {
            return NotFound();
        }

        if (booking.IsActive)
        {
            TempData["Error"] =
                "Можно удалить только отменённое бронирование.";

            return RedirectToAction(nameof(Bookings));
        }

        if (booking.BusTripSeat != null)
        {
            booking.BusTripSeat.IsAvailable = true;
        }

        db.Bookings.Remove(booking);

        await db.SaveChangesAsync();

        TempData["Success"] =
            "Отменённое бронирование успешно удалено.";

        return RedirectToAction(nameof(Bookings));
    }


}
