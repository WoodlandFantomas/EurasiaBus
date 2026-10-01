using ClosedXML.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using RouteModel = EurasiaBus.Models.Route;

namespace EurasiaBus.Services
{
    public class RouteExcelService
    {
        private readonly IWebHostEnvironment _environment;

        private const string ExcelFolder = "App_Data/Routes";
        private const string ExcelFileName = "Routes.xlsx";

        public RouteExcelService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        // Полный путь к Excel-файлу
        public string GetExcelFilePath()
        {
            var folder = Path.Combine(
                _environment.ContentRootPath,
                ExcelFolder);

            Directory.CreateDirectory(folder);

            return Path.Combine(folder, ExcelFileName);
        }

        // Проверка существования файла
        public bool ExcelFileExists()
        {
            return File.Exists(GetExcelFilePath());
        }

        // Получение маршрутов из Excel
        public List<RouteModel> GetRoutes()
        {
            var routes = new List<RouteModel>();

            var filePath = GetExcelFilePath();

            if (!File.Exists(filePath))
                return routes;

            try
            {
                // Открываем только для чтения.
                // ReadWrite разрешает другим процессам держать файл открытым.
                using var stream = new FileStream(
                    filePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite);

                using var workbook = new XLWorkbook(stream);

                var worksheet = workbook.Worksheets.FirstOrDefault();

                if (worksheet == null)
                    return routes;

                var lastRow = worksheet.LastRowUsed();

                if (lastRow == null)
                    return routes;

                int lastRowNumber = lastRow.RowNumber();

                for (int row = 1; row <= lastRowNumber; row++)
                {
                    // A = Откуда
                    string from = worksheet
                        .Cell(row, 1)
                        .GetString()
                        .Trim();

                    // B = Маршрут
                    string routePath = worksheet
                        .Cell(row, 2)
                        .GetString()
                        .Trim();

                    // C = Цена
                    string priceText = worksheet
                        .Cell(row, 3)
                        .GetString()
                        .Trim();

                    // D = Куда
                    string to = worksheet
                        .Cell(row, 4)
                        .GetString()
                        .Trim();

                    // Пустую строку пропускаем
                    if (string.IsNullOrWhiteSpace(from) &&
                        string.IsNullOrWhiteSpace(routePath) &&
                        string.IsNullOrWhiteSpace(priceText) &&
                        string.IsNullOrWhiteSpace(to))
                    {
                        continue;
                    }

                    // Если это случайная строка-заголовок,
                    // пропускаем её.
                    if (from.Equals("Откуда", StringComparison.OrdinalIgnoreCase) ||
                        routePath.Equals("Маршрут", StringComparison.OrdinalIgnoreCase) ||
                        priceText.Equals("Цена", StringComparison.OrdinalIgnoreCase) ||
                        to.Equals("Куда", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    // Все четыре поля должны присутствовать
                    if (string.IsNullOrWhiteSpace(from) ||
                        string.IsNullOrWhiteSpace(routePath) ||
                        string.IsNullOrWhiteSpace(to))
                    {
                        continue;
                    }

                    // Цена
                    decimal price;

                    // Сначала пытаемся получить число непосредственно
                    // из Excel.
                    var priceCell = worksheet.Cell(row, 3);

                    if (priceCell.DataType == XLDataType.Number)
                    {
                        price = priceCell.GetDouble() switch
                        {
                            double value => Convert.ToDecimal(value)
                        };
                    }
                    else
                    {
                        // Если цена хранится как текст:
                        // 105
                        // 105,00
                        // 105.00
                        // €105
                        // €105,00
                        priceText = priceText
                            .Replace("€", "")
                            .Replace("EUR", "", StringComparison.OrdinalIgnoreCase)
                            .Trim();

                        if (!decimal.TryParse(
                                priceText,
                                NumberStyles.Any,
                                new CultureInfo("uk-UA"),
                                out price))
                        {
                            if (!decimal.TryParse(
                                    priceText,
                                    NumberStyles.Any,
                                    CultureInfo.InvariantCulture,
                                    out price))
                            {
                                continue;
                            }
                        }
                    }

                    // Создаём маршрут
                    routes.Add(new RouteModel
                    {
                        From = from,
                        RoutePath = routePath,
                        Price = price,
                        To = to
                    });
                }
            }
            catch
            {
                // При ошибке чтения возвращаем пустой список.
                // Приложение не должно падать.
                return new List<RouteModel>();
            }

            return routes;
        }

        // Информация о файле
        public (bool Exists, long Size, DateTime? Modified) GetExcelFileInfo()
        {
            var filePath = GetExcelFilePath();

            if (!File.Exists(filePath))
                return (false, 0, null);

            var info = new FileInfo(filePath);

            return (
                true,
                info.Length,
                info.LastWriteTime
            );
        }

        // Скачать Excel
        public byte[] GetExcelFileBytes()
        {
            var filePath = GetExcelFilePath();

            if (!File.Exists(filePath))
                throw new FileNotFoundException(
                    "Файл маршрутов не найден.",
                    filePath);

            return File.ReadAllBytes(filePath);
        }

        // Загрузка нового Excel
        public async Task SaveExcelFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException(
                    "Файл Excel не выбран.");

            var filePath = GetExcelFilePath();

            var folder = Path.GetDirectoryName(filePath)!;

            Directory.CreateDirectory(folder);

            // Временный файл.
            // Это позволяет не держать Routes.xlsx открытым
            // во время загрузки.
            var tempFile = Path.Combine(
                folder,
                "Routes_upload.tmp");

            try
            {
                // Сначала записываем новый файл во временный.
                await using (var stream = new FileStream(
                    tempFile,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None))
                {
                    await file.CopyToAsync(stream);
                }

                // Проверяем, что Excel действительно открывается.
                using (var testStream = new FileStream(
                    tempFile,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read))
                {
                    using var workbook = new XLWorkbook(testStream);

                    if (!workbook.Worksheets.Any())
                    {
                        throw new InvalidOperationException(
                            "Excel-файл не содержит листов.");
                    }
                }

                // Удаляем старый файл.
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Перемещаем новый файл на место старого.
                File.Move(tempFile, filePath);
            }
            finally
            {
                // Удаляем временный файл,
                // если он остался.
                if (File.Exists(tempFile))
                {
                    try
                    {
                        File.Delete(tempFile);
                    }
                    catch
                    {
                        // Ничего не делаем.
                    }
                }
            }
        }
    }
}