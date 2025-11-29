using LuxTravel.app.Models;

namespace LuxTravel.app.Repositories.Interfaces;

internal interface IBookingRepository
{
    Booking BookTour(Booking newBooking);
    List<Booking> UserBookings(User logedInUser); 
    Booking RemoveBooking(Booking booking);
    bool HasExistingBooking(int userId, int tourId);
    List<User> GetBookingsByTourId(int tourId);
}
