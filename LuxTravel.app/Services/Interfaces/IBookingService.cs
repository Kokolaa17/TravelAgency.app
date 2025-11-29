using LuxTravel.app.Models;

namespace LuxTravel.app.Services.Interfaces;

public interface IBookingService
{
    void BookTour(Tour tour, User logedInUser, Agency agency);
    void ViewUserBookings(User logedInUser);
}
