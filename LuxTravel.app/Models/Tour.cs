using LuxTravel.app.Core;
using System;
using static LuxTravel.app.Enums.Enums;

namespace LuxTravel.app.Models;

public class Tour : Base
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string StartingPoint { get; set; }
    public string Destination { get; set; }
    public decimal Price { get; set; }
    public bool IsOnSale { get; set; } = true;
    public decimal? SalePrice { get; set; }
    public string Currency { get; set; } = "GEL";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int DurationDays => (EndDate - StartDate).Days + 1; 
    public int DurationNights => (EndDate - StartDate).Days;
    public int MaxParticipants { get; set; }
    public int CurrentParticipants { get; set; }
    public int AvailableSpots => MaxParticipants - CurrentParticipants;
    public TourStatus Status { get; set; }
    public List<string> Includes { get; set; } = new List<string>();
    public int MinAge { get; set; }
    public string? RejectReason { get; set; }

    // Agency relationship
    public int AgencyId { get; set; }
    public Agency Agency { get; set; }

    // Navigation properties
    public List<Review> Reviews { get; set; } = new List<Review>();
    public List<Booking> Bookings { get; set; } = new List<Booking>();
    public List<Wishlist> Wishlists { get; set; } = new List<Wishlist>();

}