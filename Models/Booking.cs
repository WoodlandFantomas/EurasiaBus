using System.ComponentModel.DataAnnotations;
namespace EurasiaBus.Models;
public class Booking
{
    public int BookingId { get; set; }
    [Required] public string UserId { get; set; } = "";
    public ApplicationUser User { get; set; } = null!;
    public int PassengerId { get; set; }
    public Passenger Passenger { get; set; } = null!;
    public int BusTripId { get; set; }
    public BusTrip BusTrip { get; set; } = null!;
    public int BusTripSeatId { get; set; }
    public BusTripSeat BusTripSeat { get; set; } = null!;
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;
    [Required, StringLength(20)] public string Status { get; set; } = "Pending";
    [StringLength(20)] public string? PnrCode { get; set; }
    public decimal TicketPrice { get; set; }
    public bool IsActive { get; set; } = true;
    public Payment? Payment { get; set; }
}
