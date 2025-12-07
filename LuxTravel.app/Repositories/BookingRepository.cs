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

    public Agency GetAgencyByTourId(int tourId)
    {
        return dataContext.Tours
            .Include(t => t.Agency)
            .Where(t => t.Id == tourId)
            .Select(t => t.Agency)
            .FirstOrDefault();
    }

    public User GetUserById(int userId)
    {
        return dataContext.Users.FirstOrDefault(u => u.Id == userId);
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

    public void UpdateUser(User logedInUser)
    {
        var userToUpdate = dataContext.Users
           .Include(u => u.OwnedAgency)
           .Include(Agency => Agency.OwnedAgency.Tours)
           .Include(Agency => Agency.OwnedAgency.Reviews)
           .Include(u => u.Bookings)
           .Include(u => u.Wishlists)
           .FirstOrDefault(user => user.Id == logedInUser.Id);

        dataContext.Users.Update(userToUpdate);
        dataContext.SaveChanges();
    }

    public void UpdateUserTwo(User LogedInUser)
    {
        dataContext.Users.Update(LogedInUser);
        dataContext.SaveChanges();
    }

    public List<Booking> UserBookings(User logedInUser)
    {
        return dataContext.Bookings.Where(b => b.UserId == logedInUser.Id).ToList();
    }
    public Tour GetTourById(int id)
    {
        return dataContext.Tours.FirstOrDefault(tour => tour.Id == id);
    }

    public void SaveChanges()
    {
        dataContext.SaveChanges();
    }

    public Tour FindTourById(int tourID)
    {
        return dataContext.Tours.FirstOrDefault(t => t.Id ==  tourID);
    }
    public void UpdateTour(Tour tour)
    {
        dataContext.Tours.Update(tour);
        dataContext.SaveChanges();
    }

    public void Save()
    {
        dataContext.SaveChanges();
    }


    public void UpdateAgency(Agency agency)
    {
        var agencyToUpdate = dataContext.Agencies
            .Include(a => a.Owner)
            .Include(a => a.Tours)
            .Include(a => a.Reviews)
            .FirstOrDefault(a => a.Id == agency.Id);

        dataContext.Agencies.Update(agencyToUpdate);
        dataContext.SaveChanges();
    }

    public void UpdateAgencyTwo(Agency agency)
    {
        dataContext.Agencies.Update(agency);
        dataContext.SaveChanges();
    }

    public Tour GetTourWithAgency(int tourId)
    {
        return dataContext.Tours
            .Include(t => t.Agency)             
            .FirstOrDefault(t => t.Id == tourId);
    }
}
