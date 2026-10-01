namespace EurasiaBus.ViewModels;

public class TicketPdfViewModel
{
    public int BookingId { get; set; }


public string PnrCode { get; set; } = "";

    public string PassengerName { get; set; } = "";

    public string Email { get; set; } = "";

    public string Phone { get; set; } = "";

    public string PassportNo { get; set; } = "";

    public string FromCity { get; set; } = "";

    public string ToCity { get; set; } = "";

    public DateTime DepartureDateTime { get; set; }

    public string BusName { get; set; } = "";

    public string SeatNumber { get; set; } = "";

    public decimal Price { get; set; }

    public string PaymentStatus { get; set; } = "";

    public string PaymentMethod { get; set; } = "";

    public string TransactionId { get; set; } = "";

    public DateTime BookingDate { get; set; }


}
