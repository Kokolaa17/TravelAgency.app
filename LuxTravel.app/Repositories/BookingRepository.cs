using LuxTravel.app.Data;
using LuxTravel.app.Models;
using LuxTravel.app.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LuxTravel.app.Repositories;

internal class BookingRepository : IBookingRepository
{
    DataContext dataContext = new DataContext();
    public Booking BookTour(Booking newBooking)
    {
        dataContext.Bookings.Add(newBooking);
        dataContext.SaveChanges();

        return newBooking;
    }

    public List<User> GetBookingsByTourId(int tourId)
    {
        return dataContext.Bookings
            .Include(b => b.User)
            .Include(b => b.Tour)
            .Where(b => b.TourId == tourId)
            .Select(b => b.User)
            .ToList();
    }

    public bool HasExistingBooking(int userId, int tourId)
    {
        return dataContext.Bookings
            .Include(b => b.User)
            .Include(b => b.Tour)
            .Any(b => b.UserId == userId && b.TourId == tourId);
    }

    public Booking RemoveBooking(Booking booking)
    {
        dataContext.Bookings.Remove(booking);
        dataContext.SaveChanges();
        return booking;
    }

    public List<Booking> UserBookings(User logedInUser)
    {
        return dataContext.Bookings.Where(b => b.UserId == logedInUser.Id).ToList();
    }
}
