using System.ComponentModel.DataAnnotations;
namespace EurasiaBus.Models;
public class BusStop
{
    public int BusStopId { get; set; }
    [Required, StringLength(100)] public string Name { get; set; } = "";
    [StringLength(100)] public string? City { get; set; }
    [StringLength(100)] public string? Country { get; set; }
    [StringLength(300)] public string? Address { get; set; }
    public bool IsActive { get; set; } = true;
}
