using LuxTravel.app.Core;
using static LuxTravel.app.Enums.Enums;

namespace LuxTravel.app.Models;

public class Booking : Base
{
    public int UserId { get; set; }
    public int TourId { get; set; }
    public int NumberOfPeople { get; set; } = 1;
    public decimal TotalPrice { get; set; }
    public decimal DiscountApplied { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public DateTime BookingDate { get; set; } = DateTime.Now;
    public DateTime? CancellationDate { get; set; }
    public string CancellationReason { get; set; } = string.Empty;

    public User User { get; set; }
    public Tour Tour { get; set; }
}
