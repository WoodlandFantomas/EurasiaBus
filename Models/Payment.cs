
using System.ComponentModel.DataAnnotations;

namespace EurasiaBus.Models;

public class Payment
{
    public int PaymentId { get; set; }

    // Связь с бронированием
    [Required]
    public int BookingId { get; set; }

    public Booking Booking { get; set; } = null!;

    // Сумма платежа
    [Range(0.01, 9999999999.99)]
    public decimal Amount { get; set; }

    // Способ оплаты: Card, PayPal и т.д.
    [Required]
    [StringLength(50)]
    public string PaymentMethod { get; set; } = "Card";

    // Статус: Paid, Pending, Failed, Refunded
    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Paid";

    // ID транзакции платежной системы
    [StringLength(100)]
    public string? TransactionId { get; set; }

    // Дата платежа
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
}

