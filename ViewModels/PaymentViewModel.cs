using System.ComponentModel.DataAnnotations;
namespace EurasiaBus.ViewModels;
public class PaymentViewModel
{
    public int BookingId { get; set; }
    public decimal Amount { get; set; }
    [Required] public string PaymentMethod { get; set; } = "Card";
    [StringLength(20)] public string? CardNumber { get; set; }
    [StringLength(5)] public string? Expiry { get; set; }
    [StringLength(4)] public string? Cvv { get; set; }
}
