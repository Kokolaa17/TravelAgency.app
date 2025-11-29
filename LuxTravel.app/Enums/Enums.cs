namespace LuxTravel.app.Enums;

public class Enums
{
    public enum userRole
    {
        Admin,
        Customer,
        AgencyOwner
    }

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Completed
    }

    public enum TourStatus
    {
        Draft,
        Published,
        Cancelled,
        Completed,
        Full
    }

    public enum AgencyStatus
    {
        Pending,      
        Approved,     
        Suspended,    
        Rejected      
    }
}
