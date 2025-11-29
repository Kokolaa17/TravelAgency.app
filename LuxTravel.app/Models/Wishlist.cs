using LuxTravel.app.Core;

namespace LuxTravel.app.Models;

public class Wishlist : Base
{
    public int UserId { get; set; }
    public int TourId { get; set; }
    public User User { get; set; }
    public Tour Tour { get; set; }
}
