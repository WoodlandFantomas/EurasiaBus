using EurasiaBus.ViewModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EurasiaBus.Services;

public class PdfTicketService
{
    public byte[] Generate(TicketPdfViewModel ticket)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);


            page.DefaultTextStyle(x =>
                x.FontSize(10));

                page.Header()
                    .PaddingBottom(15)
                    .Column(column =>
                    {
                        column.Item()
                            .Text("EURASIA BUS")
                            .FontSize(25)
                            .Bold()
                            .FontColor(Colors.Blue.Medium);

                        column.Item()
                            .Text("BUS TICKET / БИЛЕТ")
                            .FontSize(11)
                            .FontColor(Colors.Grey.Darken1);
                    });


                page.Content()
                    .Column(column =>
                    {
                        column.Spacing(15);


                        // PNR

                        column.Item()
                            .Background(Colors.Blue.Lighten5)
                            .Border(1)
                            .BorderColor(Colors.Blue.Lighten2)
                            .Padding(15)
                            .Row(row =>
                            {
                                row.RelativeItem()
                                    .Column(c =>
                                    {
                                        c.Item()
                                            .Text("PNR")
                                            .FontSize(9)
                                            .FontColor(Colors.Grey.Darken1);

                                        c.Item()
                                            .Text(ticket.PnrCode)
                                            .FontSize(20)
                                            .Bold()
                                            .FontColor(Colors.Blue.Medium);
                                    });

                                row.ConstantItem(120)
                                    .AlignRight()
                                    .Column(c =>
                                    {
                                        c.Item()
                                            .Text("STATUS")
                                            .FontSize(9)
                                            .FontColor(Colors.Grey.Darken1);

                                        c.Item()
                                            .Text(ticket.PaymentStatus.ToUpperInvariant())
                                            .FontSize(12)
                                            .Bold()
                                            .FontColor(Colors.Green.Darken2);
                                    });
                            });


                        // ROUTE

                        column.Item()
                            .PaddingTop(5)
                            .Column(c =>
                            {
                                c.Item()
                                    .Text("МАРШРУТ")
                                    .FontSize(9)
                                    .Bold()
                                    .FontColor(Colors.Grey.Darken1);

                                c.Item()
                                    .PaddingTop(6)
                                    .Row(row =>
                                    {
                                        row.RelativeItem()
                                            .Column(city =>
                                            {
                                                city.Item()
                                                    .Text("ОТКУДА")
                                                    .FontSize(8)
                                                    .FontColor(Colors.Grey.Darken1);

                                                city.Item()
                                                    .Text(ticket.FromCity)
                                                    .FontSize(16)
                                                    .Bold();
                                            });

                                        row.ConstantItem(50)
                                            .AlignCenter()
                                            .AlignMiddle()
                                            .Text("→")
                                            .FontSize(20)
                                            .FontColor(Colors.Blue.Medium);

                                        row.RelativeItem()
                                            .Column(city =>
                                            {
                                                city.Item()
                                                    .Text("КУДА")
                                                    .FontSize(8)
                                                    .FontColor(Colors.Grey.Darken1);

                                                city.Item()
                                                    .Text(ticket.ToCity)
                                                    .FontSize(16)
                                                    .Bold();
                                            });
                                    });
                            });


                        // TRIP DETAILS

                        column.Item()
                            .Border(1)
                            .BorderColor(Colors.Grey.Lighten2)
                            .Padding(15)
                            .Column(c =>
                            {
                                c.Item()
                                    .Text("ПОЕЗДКА")
                                    .FontSize(9)
                                    .Bold()
                                    .FontColor(Colors.Grey.Darken1);

                                c.Item()
                                    .PaddingTop(8)
                                    .Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                        });

                                        table.Cell()
                                            .PaddingBottom(10)
                                            .Text(text =>
                                            {
                                                text.Span("Дата отправления\n")
                                                    .FontSize(8)
                                                    .FontColor(Colors.Grey.Darken1);

                                                text.Span(ticket.DepartureDateTime.ToString("dd.MM.yyyy"))
                                                    .Bold();
                                            });

                                        table.Cell()
                                            .PaddingBottom(10)
                                            .Text(text =>
                                            {
                                                text.Span("Время отправления\n")
                                                    .FontSize(8)
                                                    .FontColor(Colors.Grey.Darken1);

                                                text.Span(ticket.DepartureDateTime.ToString("HH:mm"))
                                                    .Bold();
                                            });

                                        table.Cell()
                                            .Text(text =>
                                            {
                                                text.Span("Автобус\n")
                                                    .FontSize(8)
                                                    .FontColor(Colors.Grey.Darken1);

                                                text.Span(ticket.BusName)
                                                    .Bold();
                                            });

                                        table.Cell()
                                            .Text(text =>
                                            {
                                                text.Span("Место\n")
                                                    .FontSize(8)
                                                    .FontColor(Colors.Grey.Darken1);

                                                text.Span(ticket.SeatNumber)
                                                    .FontSize(14)
                                                    .Bold()
                                                    .FontColor(Colors.Blue.Medium);
                                            });
                                    });
                            });


                        // PASSENGER

                        column.Item()
                            .Border(1)
                            .BorderColor(Colors.Grey.Lighten2)
                            .Padding(15)
                            .Column(c =>
                            {
                                c.Item()
                                    .Text("ПАССАЖИР")
                                    .FontSize(9)
                                    .Bold()
                                    .FontColor(Colors.Grey.Darken1);

                                c.Item()
                                    .PaddingTop(8)
                                    .Text(ticket.PassengerName)
                                    .FontSize(14)
                                    .Bold();

                                if (!string.IsNullOrWhiteSpace(ticket.PassportNo))
                                {
                                    c.Item()
                                        .PaddingTop(4)
                                        .Text($"Паспорт: {ticket.PassportNo}")
                                        .FontSize(9);
                                }

                                if (!string.IsNullOrWhiteSpace(ticket.Email))
                                {
                                    c.Item()
                                        .PaddingTop(3)
                                        .Text($"Email: {ticket.Email}")
                                        .FontSize(9);
                                }

                                if (!string.IsNullOrWhiteSpace(ticket.Phone))
                                {
                                    c.Item()
                                        .PaddingTop(3)
                                        .Text($"Телефон: {ticket.Phone}")
                                        .FontSize(9);
                                }
                            });


                        // PAYMENT

                        column.Item()
                            .Background(Colors.Grey.Lighten4)
                            .Padding(15)
                            .Row(row =>
                            {
                                row.RelativeItem()
                                    .Column(c =>
                                    {
                                        c.Item()
                                            .Text("Оплата")
                                            .FontSize(8)
                                            .FontColor(Colors.Grey.Darken1);

                                        c.Item()
                                            .Text(ticket.PaymentMethod)
                                            .Bold();

                                        if (!string.IsNullOrWhiteSpace(ticket.TransactionId))
                                        {
                                            c.Item()
                                                .PaddingTop(3)
                                                .Text($"Transaction: {ticket.TransactionId}")
                                                .FontSize(8);
                                        }
                                    });

                                row.ConstantItem(150)
                                    .AlignRight()
                                    .Column(c =>
                                    {
                                        c.Item()
                                            .Text("ИТОГО")
                                            .FontSize(8)
                                            .FontColor(Colors.Grey.Darken1);

                                        c.Item()
                                            .Text($"€ {ticket.Price:N2}")
                                            .FontSize(18)
                                            .Bold()
                                            .FontColor(Colors.Blue.Medium);
                                    });
                            });

                    });


                page.Footer()
                    .AlignCenter()
                    .Column(column =>
                    {
                        column.Item()
                            .Text("EURASIA BUS")
                            .Bold()
                            .FontColor(Colors.Blue.Medium);

                        column.Item()
                            .Text("Спасибо за поездку!")
                            .FontSize(9)
                            .FontColor(Colors.Grey.Darken1);
                    });
            });
        });

        return document.GeneratePdf();
    }


}
