using System.ComponentModel.DataAnnotations;
namespace EurasiaBus.Models;
public class Bus
{
    public int BusId { get; set; }
    [Required, StringLength(100)] public string Name { get; set; } = "";
    [Required, StringLength(50)] public string RegistrationNumber { get; set; } = "";
    [Range(1, 100)] public int SeatCount { get; set; }
    [StringLength(100)] public string? Model { get; set; }
    [StringLength(100)] public string? Carrier { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<BusTrip> Trips { get; set; } = [];
}
