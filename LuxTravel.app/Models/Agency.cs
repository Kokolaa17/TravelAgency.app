using LuxTravel.app.Core;
using static LuxTravel.app.Enums.Enums;

namespace LuxTravel.app.Models;

public class Agency : Base
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Email { get; set; } 
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public AgencyStatus Status { get; set; }
    public int TotalToursCreated { get; set; }
    public int TotalBookings { get; set; }
    public decimal Balance { get; set; }
    public decimal TotalEarnings { get; set; }
    public string? RejectReason { get; set; } 

    // Owner relationship
    public int OwnerId { get; set; }
    public User Owner { get; set; }

    // Navigation properties
    public List<Tour> Tours { get; set; } = new List<Tour>();
    public List<AgencyReview> Reviews { get; set; } = new List<AgencyReview>();
}
