using System.ComponentModel.DataAnnotations;
namespace EurasiaBus.ViewModels;
public class PassengerDetailsViewModel
{
    public int TripId { get; set; }
    public List<int> SeatIds { get; set; } = [];
    [Required] public string FirstName { get; set; } = "";
    [Required] public string LastName { get; set; } = "";
    [EmailAddress] public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? PassportNo { get; set; }
    public DateTime? BirthDate { get; set; }
}
