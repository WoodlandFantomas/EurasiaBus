using System.ComponentModel.DataAnnotations;
namespace EurasiaBus.Models;
public class BusTripSeat
{
    public int BusTripSeatId { get; set; }
    public int BusTripId { get; set; }
    public BusTrip BusTrip { get; set; } = null!;
    [Required, StringLength(10)] public string SeatNumber { get; set; } = "";
    public bool IsAvailable { get; set; } = true;
    public decimal Price { get; set; }
    public ICollection<Booking> Bookings { get; set; } = [];
}
