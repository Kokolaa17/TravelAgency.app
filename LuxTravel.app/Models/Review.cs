using LuxTravel.app.Core;

namespace LuxTravel.app.Models;

public class Review : Base
{
    public int TourId { get; set; }
    public int UserId { get; set; }
    public int Rating { get; set; }  // 1-5
    public string Comment { get; set; }
    public DateTime ReviewDate { get; set; } = DateTime.Now;
    public Tour Tour { get; set; }
    public User User { get; set; }
}
