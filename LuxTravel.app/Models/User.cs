using LuxTravel.app.Core;
using static LuxTravel.app.Enums.Enums;

namespace LuxTravel.app.Models;
public class User : Base
{
    public userRole Role { get; set; } = userRole.Customer;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool IsEmailConfirmed { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public int PurchasesUntilDiscount { get; set; } = 0;
    public decimal Balance { get; set; } = 0;
    public decimal TotalSpent { get; set; } = 0;

    // Navigation properties
    public List<Tour> PurchasedTours { get; set; } = new List<Tour>();
    public List<Tour> FavoriteTours { get; set; } = new List<Tour>();
    public List<Booking> Bookings { get; set; } = new List<Booking>();
    public List<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
    public List<Review> Reviews { get; set; } = new List<Review>();
    public List<AgencyReview> AgencyReviews { get; set; } = new List<AgencyReview>();

    // Agency relationship
    public int? AgencyId { get; set; }
    public Agency OwnedAgency { get; set; } 
}
