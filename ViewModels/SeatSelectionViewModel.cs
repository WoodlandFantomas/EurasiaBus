namespace EurasiaBus.ViewModels;
public class SeatSelectionViewModel
{
    public int TripId { get; set; }
    public List<int> SelectedSeatIds { get; set; } = [];
}
