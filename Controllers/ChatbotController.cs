using EurasiaBus.Data;
using EurasiaBus.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EurasiaBus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatbotController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChatbotController(ApplicationDbContext context)
        {
            _context = context;
        }

        public class ChatRequest
        {
            public string? Message { get; set; }
        }

        public class ChatResponse
        {
            public string Reply { get; set; } = string.Empty;
        }

        [HttpPost("message")]
        [AllowAnonymous]
        public async Task<ActionResult<ChatResponse>> Message(
            [FromBody] ChatRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest(new ChatResponse
                {
                    Reply = "Пожалуйста, напишите ваш вопрос."
                });
            }

            string message = request.Message.Trim();
            string lower = message.ToLowerInvariant();

            // -----------------------------------------
            // ПРИВЕТСТВИЕ
            // -----------------------------------------
            if (ContainsAny(lower,
                "привет",
                "здравствуйте",
                "добрый день",
                "добрый вечер",
                "доброе утро",
                "hello",
                "hi"))
            {
                return Ok(new ChatResponse
                {
                    Reply =
                        "Здравствуйте! 👋 Я EurasiaBus Support.\n\n" +
                        "Я могу помочь вам с поиском автобуса, бронированием, " +
                        "выбором места, оплатой и билетами.\n\n" +
                        "Например, спросите:\n" +
                        "• Как купить билет?\n" +
                        "• Как выбрать место?\n" +
                        "• Где мои билеты?\n" +
                        "• Как узнать PNR?"
                });
            }

            // -----------------------------------------
            // ПОИСК АВТОБУСА
            // -----------------------------------------
            if (ContainsAny(lower,
                "найти автобус",
                "поиск автобуса",
                "расписание",
                "автобус",
                "рейс",
                "маршрут"))
            {
                return Ok(new ChatResponse
                {
                    Reply =
                        "🚌 Чтобы найти автобус, перейдите на главную страницу " +
                        "EurasiaBus.\n\n" +
                        "Укажите город отправления, город прибытия, дату поездки " +
                        "и количество пассажиров. После поиска вы увидите доступные " +
                        "рейсы и свободные места."
                });
            }

            // -----------------------------------------
            // ПОКУПКА БИЛЕТА
            // -----------------------------------------
            if (ContainsAny(lower,
                "купить билет",
                "покупка билета",
                "забронировать",
                "бронирование",
                "как купить"))
            {
                return Ok(new ChatResponse
                {
                    Reply =
                        "🎫 Для покупки билета:\n\n" +
                        "1. Найдите подходящий автобус.\n" +
                        "2. Выберите рейс.\n" +
                        "3. Выберите свободное место.\n" +
                        "4. Введите данные пассажира.\n" +
                        "5. Перейдите к оплате.\n" +
                        "6. После успешного оформления вы получите номер PNR."
                });
            }

            // -----------------------------------------
            // ВЫБОР МЕСТА
            // -----------------------------------------
            if (ContainsAny(lower,
                "место",
                "сиденье",
                "seat",
                "выбрать место",
                "свободное место"))
            {
                return Ok(new ChatResponse
                {
                    Reply =
                        "💺 После выбора автобуса откроется схема доступных мест. " +
                        "Свободные места можно выбрать перед переходом к данным пассажира.\n\n" +
                        "Если выбранное место уже занято, необходимо выбрать другое."
                });
            }

            // -----------------------------------------
            // ОПЛАТА
            // -----------------------------------------
            if (ContainsAny(lower,
                "оплат",
                "карта",
                "payment",
                "paypal",
                "банковская карта"))
            {
                return Ok(new ChatResponse
                {
                    Reply =
                        "💳 После ввода данных пассажира вы перейдёте на страницу оплаты.\n\n" +
                        "Проверьте маршрут, дату, пассажира, место и итоговую стоимость " +
                        "перед подтверждением платежа."
                });
            }

            // -----------------------------------------
            // МОИ БИЛЕТЫ
            // -----------------------------------------
            if (ContainsAny(lower,
                "мои билеты",
                "мой билет",
                "билеты",
                "ticket",
                "my tickets"))
            {
                if (User.Identity?.IsAuthenticated == true)
                {
                    return Ok(new ChatResponse
                    {
                        Reply =
                            "🎟️ Вы авторизованы.\n\n" +
                            "Откройте раздел «Мои билеты» в меню вашего аккаунта. " +
                            "Там отображаются ваши бронирования, маршрут, дата поездки, " +
                            "место, стоимость и PNR."
                    });
                }

                return Ok(new ChatResponse
                {
                    Reply =
                        "🎟️ Для просмотра ваших билетов сначала войдите в аккаунт. " +
                        "После входа откройте раздел «Мои билеты»."
                });
            }

            // -----------------------------------------
            // PNR
            // -----------------------------------------
            if (ContainsAny(lower,
                "pnr",
                "номер бронирования",
                "код бронирования",
                "код билета"))
            {
                return Ok(new ChatResponse
                {
                    Reply =
                        "🔐 PNR — это уникальный код вашего бронирования.\n\n" +
                        "Он создаётся после успешного оформления бронирования " +
                        "и отображается в информации о вашем билете."
                });
            }

            // -----------------------------------------
            // ОТМЕНА
            // -----------------------------------------
            if (ContainsAny(lower,
                "отменить",
                "отмена",
                "cancel",
                "отменить билет",
                "отменить бронирование"))
            {
                return Ok(new ChatResponse
                {
                    Reply =
                        "❌ Для отмены бронирования откройте «Мои билеты» " +
                        "и выберите нужное бронирование.\n\n" +
                        "Если кнопка отмены недоступна, обратитесь в службу поддержки."
                });
            }

            // -----------------------------------------
            // ЦЕНА
            // -----------------------------------------
            if (ContainsAny(lower,
                "цена",
                "стоимость",
                "сколько стоит",
                "price",
                "cost"))
            {
                return Ok(new ChatResponse
                {
                    Reply =
                        "💶 Стоимость зависит от выбранного автобусного рейса и места.\n\n" +
                        "Точная стоимость отображается во время выбора рейса " +
                        "и перед оплатой."
                });
            }

            // -----------------------------------------
            // ВОЗВРАТ
            // -----------------------------------------
            if (ContainsAny(lower,
                "возврат",
                "вернуть деньги",
                "refund",
                "деньги"))
            {
                return Ok(new ChatResponse
                {
                    Reply =
                        "💰 По вопросам возврата средств проверьте статус вашего " +
                        "бронирования в разделе «Мои билеты» или обратитесь " +
                        "в службу поддержки EurasiaBus."
                });
            }

            // -----------------------------------------
            // ПРОФИЛЬ / ВХОД
            // -----------------------------------------
            if (ContainsAny(lower,
                "войти",
                "вход",
                "регистрация",
                "зарегистрироваться",
                "аккаунт",
                "профиль",
                "login",
                "register"))
            {
                return Ok(new ChatResponse
                {
                    Reply =
                        "👤 Вы можете зарегистрировать аккаунт или войти в существующий " +
                        "через меню сайта.\n\n" +
                        "После входа будут доступны ваши билеты и данные аккаунта."
                });
            }

            // -----------------------------------------
            // ПОДДЕРЖКА
            // -----------------------------------------
            if (ContainsAny(lower,
                "поддержка",
                "оператор",
                "менеджер",
                "помощь",
                "support",
                "contact",
                "контакты"))
            {
                return Ok(new ChatResponse
                {
                    Reply =
                        "📞 EurasiaBus Support\n\n" +
                        "Опишите вашу проблему максимально подробно: " +
                        "маршрут, дату поездки, PNR или текст ошибки. " +
                        "Я постараюсь подсказать решение."
                });
            }

            // -----------------------------------------
            // РЕЙСЫ ИЗ БАЗЫ
            // -----------------------------------------
            if (ContainsAny(lower,
                "ближайшие рейсы",
                "доступные рейсы",
                "есть ли рейсы"))
            {
                var trips = await _context.BusTrips
                    .Include(x => x.DepartureStop)
                    .Include(x => x.ArrivalStop)
                    .Where(x => x.IsActive &&
                                x.DepartureDateTime >= DateTime.Now)
                    .OrderBy(x => x.DepartureDateTime)
                    .Take(5)
                    .ToListAsync();

                if (trips.Count == 0)
                {
                    return Ok(new ChatResponse
                    {
                        Reply = "🚌 Сейчас в базе нет ближайших активных рейсов."
                    });
                }

                var reply = "🚌 Ближайшие рейсы:\n\n";

                foreach (var trip in trips)
                {
                    string departure =
                        trip.DepartureStop?.City ??
                        trip.DepartureStop?.Name ??
                        "Отправление";

                    string arrival =
                        trip.ArrivalStop?.City ??
                        trip.ArrivalStop?.Name ??
                        "Прибытие";

                    reply +=
                        $"• {departure} → {arrival}\n" +
                        $"  {trip.DepartureDateTime:dd.MM.yyyy HH:mm}\n" +
                        $"  Цена: €{trip.Price:N2}\n\n";
                }

                return Ok(new ChatResponse
                {
                    Reply = reply.Trim()
                });
            }

            // -----------------------------------------
            // СПАСИБО
            // -----------------------------------------
            if (ContainsAny(lower,
                "спасибо",
                "благодарю",
                "thanks",
                "thank you"))
            {
                return Ok(new ChatResponse
                {
                    Reply =
                        "😊 Пожалуйста! Рад помочь. Хорошей поездки вместе с EurasiaBus! 🚌"
                });
            }

            // -----------------------------------------
            // НЕИЗВЕСТНЫЙ ВОПРОС
            // -----------------------------------------
            return Ok(new ChatResponse
            {
                Reply =
                    "🤖 Пока я не совсем понял ваш вопрос.\n\n" +
                    "Попробуйте спросить:\n" +
                    "• Как купить билет?\n" +
                    "• Как выбрать место?\n" +
                    "• Где мои билеты?\n" +
                    "• Как узнать PNR?\n" +
                    "• Как отменить бронирование?\n" +
                    "• Какие есть ближайшие рейсы?"
            });
        }

        private static bool ContainsAny(
            string text,
            params string[] values)
        {
            return values.Any(value =>
                text.Contains(value, StringComparison.OrdinalIgnoreCase));
        }
    }
}
