using EurasiaBus.Data;
using EurasiaBus.Models;
using EurasiaBus.Services;
using EurasiaBus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EurasiaBus.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RouteTextService _routeTextService;


    public HomeController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RouteTextService routeTextService)
    {
        _context = context;
        _userManager = userManager;
        _routeTextService = routeTextService;
    }


    // =========================================================
    // ГЛАВНАЯ СТРАНИЦА
    // =========================================================

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        await LoadStops();

        // -----------------------------------------------------
        // Загружаем маршруты из Routes.txt
        // -----------------------------------------------------

        try
        {
            ViewBag.Routes = _routeTextService.GetRoutes();
        }
        catch (Exception ex)
        {
            ViewBag.Routes =
                new List<EurasiaBus.Models.Route>();

            ViewBag.RouteError =
                "Не удалось загрузить маршруты: " + ex.Message;
        }


        var model = new BusSearchViewModel
        {
            DepartureDate = DateTime.Today.AddDays(1),
            Passengers = 1
        };


        return View(model);
    }


    // =========================================================
    // О НАС
    // =========================================================

    [HttpGet]
    public IActionResult About()
    {
        return View();
    }


    // =========================================================
    // ПОИСК - GET
    // =========================================================

    [HttpGet]
    public async Task<IActionResult> Search()
    {
        await LoadStops();

        var model = new BusSearchViewModel
        {
            DepartureDate = DateTime.Today.AddDays(1),
            Passengers = 1
        };

        return View("Index", model);
    }


    // =========================================================
    // ПОИСК РЕЙСОВ - POST
    // =========================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Search(
        BusSearchViewModel model)
    {
        // -----------------------------------------------------
        // Загружаем остановки.
        // Нужно при возврате на форму с ошибкой.
        // -----------------------------------------------------

        await LoadStops();


        // -----------------------------------------------------
        // Проверка модели
        // -----------------------------------------------------

        if (!ModelState.IsValid)
        {
            return View("Index", model);
        }


        // -----------------------------------------------------
        // Отправление и прибытие не должны совпадать
        // -----------------------------------------------------

        if (model.DepartureStopId == model.ArrivalStopId)
        {
            ModelState.AddModelError(
                string.Empty,
                "Место отправления и место прибытия должны отличаться."
            );

            return View("Index", model);
        }


        // -----------------------------------------------------
        // Количество пассажиров
        // -----------------------------------------------------

        if (model.Passengers < 1)
        {
            ModelState.AddModelError(
                nameof(model.Passengers),
                "Количество пассажиров должно быть не менее 1."
            );

            return View("Index", model);
        }


        if (model.Passengers > 10)
        {
            ModelState.AddModelError(
                nameof(model.Passengers),
                "Количество пассажиров не может превышать 10."
            );

            return View("Index", model);
        }


        // -----------------------------------------------------
        // Дата
        // -----------------------------------------------------

        var departureDate =
            model.DepartureDate.Date;

        var nextDay =
            departureDate.AddDays(1);


        // -----------------------------------------------------
        // Ищем рейсы
        // -----------------------------------------------------

        var trips = await _context.BusTrips

            .Include(x => x.Bus)

            .Include(x => x.DepartureStop)

            .Include(x => x.ArrivalStop)

            .Include(x => x.Seats)

            .Where(x =>
                x.IsActive &&

                x.DepartureStopId ==
                    model.DepartureStopId &&

                x.ArrivalStopId ==
                    model.ArrivalStopId &&

                x.DepartureDateTime >=
                    departureDate &&

                x.DepartureDateTime <
                    nextDay
            )

            .OrderBy(x => x.DepartureDateTime)

            .ToListAsync();


        // -----------------------------------------------------
        // Только рейсы с достаточным количеством свободных мест
        // -----------------------------------------------------

        trips = trips
            .Where(x =>
                x.Seats.Count(seat =>
                    seat.IsAvailable)
                >= model.Passengers
            )
            .ToList();


        // -----------------------------------------------------
        // Если ничего не найдено
        // -----------------------------------------------------

        if (trips.Count == 0)
        {
            TempData["Info"] =
                "На выбранную дату подходящих автобусных рейсов не найдено.";

            return View("Index", model);
        }


        // -----------------------------------------------------
        // Передаём дополнительную информацию в SearchResults
        // -----------------------------------------------------

        ViewBag.Passengers =
            model.Passengers;

        ViewBag.DepartureStopId =
            model.DepartureStopId;

        ViewBag.ArrivalStopId =
            model.ArrivalStopId;

        ViewBag.DepartureDate =
            model.DepartureDate;


        // -----------------------------------------------------
        // Результаты поиска
        // -----------------------------------------------------

        return View(
            "SearchResults",
            trips
        );
    }


    // =========================================================
    // МОИ БИЛЕТЫ
    // =========================================================

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> MyBookings()
    {
        // -----------------------------------------------------
        // Получаем ID текущего пользователя Identity
        // -----------------------------------------------------

        var userId =
            _userManager.GetUserId(User);


        if (string.IsNullOrWhiteSpace(userId))
        {
            return Challenge();
        }


        // -----------------------------------------------------
        // Загружаем бронирования текущего пользователя
        // -----------------------------------------------------

        var bookings = await _context.Bookings

            .AsNoTracking()

            .Include(x => x.Passenger)

            .Include(x => x.BusTrip)
                .ThenInclude(x => x.Bus)

            .Include(x => x.BusTrip)
                .ThenInclude(x => x.DepartureStop)

            .Include(x => x.BusTrip)
                .ThenInclude(x => x.ArrivalStop)

            .Include(x => x.BusTripSeat)

            .Include(x => x.Payment)

            .Where(x =>
                x.UserId == userId
            )

            .OrderByDescending(x =>
                x.BookingDate
            )

            .ToListAsync();


        return View(
            "MyBookings",
            bookings
        );
    }


    // =========================================================
    // ЗАГРУЗКА ОСТАНОВОК
    // =========================================================

    private async Task LoadStops()
    {
        ViewBag.Stops = await _context.BusStops

            .Where(x => x.IsActive)

            .OrderBy(x => x.City)

            .ThenBy(x => x.Name)

            .ToListAsync();
    }


    // =========================================================
    // ERROR
    // =========================================================

    [ResponseCache(
        Duration = 0,
        Location = ResponseCacheLocation.None,
        NoStore = true
    )]
    public IActionResult Error()
    {
        return View();
    }
}