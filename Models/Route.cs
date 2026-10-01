
using System.ComponentModel.DataAnnotations;

namespace EurasiaBus.Models
{
    public class Route
    {
        public int Id { get; set; }

        [Display(Name = "Откуда")]
        public string From { get; set; } = string.Empty;

        [Display(Name = "Маршрут")]
        public string RoutePath { get; set; } = string.Empty;

        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Display(Name = "Куда")]
        public string To { get; set; } = string.Empty;
    }
}

