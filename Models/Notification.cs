using System.ComponentModel.DataAnnotations;
namespace EurasiaBus.Models;
public class Notification
{
    public int NotificationId { get; set; }
    [Required] public string UserId { get; set; } = "";
    public ApplicationUser User { get; set; } = null!;
    [Required, StringLength(200)] public string Title { get; set; } = "";
    public string? Message { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }
}
