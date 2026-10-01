using System.Globalization;
using EurasiaBus.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using RouteModel = EurasiaBus.Models.Route;

namespace EurasiaBus.Services
{
    public class RouteTextService
    {
        private readonly IWebHostEnvironment _environment;

        private const string RoutesFolder = "App_Data/Routes";
        private const string RoutesFileName = "Routes.txt";

        public RouteTextService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }


        // =========================================================
        // ПУТЬ К ФАЙЛУ
        // =========================================================

        private string GetFilePath()
        {
            var folder = Path.Combine(
                _environment.ContentRootPath,
                RoutesFolder);

            Directory.CreateDirectory(folder);

            return Path.Combine(
                folder,
                RoutesFileName);
        }


        public string GetRoutesFilePath()
        {
            return GetFilePath();
        }


        // =========================================================
        // ПРОВЕРКА ФАЙЛА
        // =========================================================

        public bool FileExists()
        {
            return System.IO.File.Exists(
                GetFilePath());
        }


        // =========================================================
        // ЧТЕНИЕ МАРШРУТОВ
        // =========================================================

        public List<RouteModel> GetRoutes()
        {
            var filePath = GetFilePath();

            if (!System.IO.File.Exists(filePath))
            {
                return new List<RouteModel>();
            }

            var routes = new List<RouteModel>();

            var lines = System.IO.File.ReadAllLines(
                filePath,
                new System.Text.UTF8Encoding(false));

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(
                    '|',
                    StringSplitOptions.None);

                if (parts.Length < 4)
                    continue;

                var from = parts[0].Trim();

                var routePath = parts[1].Trim();

                var priceText = parts[2].Trim();

                var to = parts[3].Trim();

                if (string.IsNullOrWhiteSpace(from) ||
                    string.IsNullOrWhiteSpace(routePath) ||
                    string.IsNullOrWhiteSpace(to))
                {
                    continue;
                }


                // -------------------------------------------------
                // Цена
                // -------------------------------------------------

                priceText =
                    priceText
                        .Replace("€", "")
                        .Replace(" ", "")
                        .Replace(',', '.');


                if (!decimal.TryParse(
                        priceText,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out decimal price))
                {
                    continue;
                }


                // -------------------------------------------------
                // Добавляем маршрут
                // -------------------------------------------------

                routes.Add(
                    new RouteModel
                    {
                        From = from,
                        RoutePath = routePath,
                        Price = price,
                        To = to
                    });
            }

            return routes;
        }


        // =========================================================
        // СОХРАНЕНИЕ МАРШРУТОВ
        // =========================================================

        public void SaveRoutes(
            IEnumerable<RouteModel> routes)
        {
            var filePath = GetFilePath();

            var lines = routes.Select(route =>
                string.Join(
                    "|",
                    route.From,
                    route.RoutePath,
                    route.Price.ToString(
                        "0.00",
                        CultureInfo.InvariantCulture),
                    route.To));

            System.IO.File.WriteAllLines(
                filePath,
                lines,
                new System.Text.UTF8Encoding(false));
        }


        // =========================================================
        // ПОЛУЧИТЬ BYTES ФАЙЛА
        // =========================================================

        public byte[] GetFileBytes()
        {
            var filePath = GetFilePath();

            if (!System.IO.File.Exists(filePath))
            {
                return Array.Empty<byte>();
            }

            return System.IO.File.ReadAllBytes(
                filePath);
        }


        // =========================================================
        // ЗАГРУЗКА TXT
        // =========================================================

        public void SaveUploadedFile(
            IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(
                    nameof(file));
            }

            if (file.Length == 0)
            {
                throw new ArgumentException(
                    "Загружаемый файл пустой.",
                    nameof(file));
            }

            var extension =
                Path.GetExtension(file.FileName);

            if (!extension.Equals(
                    ".txt",
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    "Разрешена загрузка только TXT-файлов.");
            }

            var filePath = GetFilePath();

            using var reader =
                new StreamReader(
                    file.OpenReadStream(),
                    new System.Text.UTF8Encoding(false),
                    detectEncodingFromByteOrderMarks: true);

            var text = reader.ReadToEnd();

            // Перезаписываем файл именно UTF-8
            System.IO.File.WriteAllText(
                filePath,
                text,
                new System.Text.UTF8Encoding(false));
        }


        // =========================================================
        // ПОЛУЧИТЬ ТЕКСТ ФАЙЛА
        // =========================================================

        public string GetFileText()
        {
            var filePath = GetFilePath();

            if (!System.IO.File.Exists(filePath))
            {
                return string.Empty;
            }

            return System.IO.File.ReadAllText(
                filePath,
                new System.Text.UTF8Encoding(false));
        }
    }
}