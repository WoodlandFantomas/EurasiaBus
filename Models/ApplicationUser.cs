using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EurasiaBus.Models;

public class ApplicationUser : IdentityUser
{
    [StringLength(100)] public string? FirstName { get; set; }
    [StringLength(100)] public string? LastName { get; set; }
    public ICollection<Booking> Bookings { get; set; } = [];
    public ICollection<Passenger> Passengers { get; set; } = [];
}
