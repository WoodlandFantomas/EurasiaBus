using System.ComponentModel.DataAnnotations;
namespace EurasiaBus.Models;
public class Passenger
{
    public int PassengerId { get; set; }
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }
    [StringLength(50)] public string FirstName { get; set; } = "";
    [StringLength(50)] public string LastName { get; set; } = "";
    [EmailAddress, StringLength(100)] public string? Email { get; set; }
    [StringLength(20)] public string? PhoneNumber { get; set; }
    [StringLength(20)] public string? PassportNo { get; set; }
    [StringLength(50)] public string? Nationality { get; set; }
    [StringLength(20)] public string? Gender { get; set; }
    public DateTime? BirthDate { get; set; }
}
