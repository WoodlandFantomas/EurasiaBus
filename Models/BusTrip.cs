using System.ComponentModel.DataAnnotations;
namespace EurasiaBus.Models;
public class BusTrip
{
    public int BusTripId { get; set; }
    [Required] public int BusId { get; set; }
    public Bus Bus { get; set; } = null!;
    [Required] public int DepartureStopId { get; set; }
    public BusStop DepartureStop { get; set; } = null!;
    [Required] public int ArrivalStopId { get; set; }
    public BusStop ArrivalStop { get; set; } = null!;
    [Required] public DateTime DepartureDateTime { get; set; }
    [Required] public DateTime ArrivalDateTime { get; set; }
    [Range(0, 999999)] public decimal Price { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<BusTripSeat> Seats { get; set; } = [];
    public ICollection<Booking> Bookings { get; set; } = [];
}
