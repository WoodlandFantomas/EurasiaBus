using System.ComponentModel.DataAnnotations;
namespace EurasiaBus.ViewModels;
public class BusSearchViewModel
{
    [Required] public int DepartureStopId { get; set; }
    [Required] public int ArrivalStopId { get; set; }
    [DataType(DataType.Date)] public DateTime DepartureDate { get; set; } = DateTime.Today;
    [DataType(DataType.Date)] public DateTime? ReturnDate { get; set; }
    [Range(1, 10)] public int Passengers { get; set; } = 1;
    public bool RoundTrip { get; set; }
}
