using LuxTravel.app.Data;
using LuxTravel.app.Models;
using LuxTravel.app.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using static LuxTravel.app.Enums.Enums;

namespace LuxTravel.app.Repositories;

internal class AgencyRepository : IAgencyRepository
{
    private readonly DataContext dataContext = new DataContext();

    public Tour RegisterTour(Tour newTour)
    {
        dataContext.Add(newTour);
        dataContext.SaveChanges();
        return newTour;
    }

    public Agency GetAgencyByOwnerId(int ownerId)
    {
        return dataContext.Agencies.FirstOrDefault(a => a.OwnerId == ownerId);
    }

    public List<Tour> GetAllTours()
    {
        return dataContext.Tours.ToList();
    }

    public User GetAgencyOwnerById(int agencyId)
    {
        return dataContext.Users.FirstOrDefault(u => u.AgencyId == agencyId);
    }

    public User GetUserById(int userId)
    {
        return dataContext.Users
            .Include(u => u.OwnedAgency)
            .FirstOrDefault(u => u.Id == userId);
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
        dataContext.Agencies.Update(agency); // ✅ Use the parameter
        dataContext.SaveChanges();
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

    public void UpdateUserTwo(User logedInUser)
    {
        dataContext.Users.Update(logedInUser);
        dataContext.SaveChanges();
    }

    public Tour RemoveTour(int tourId)
    {
        var tour = dataContext.Tours
        .Include(t => t.Bookings)
        .Include(t => t.Wishlists)
        .FirstOrDefault(t => t.Id == tourId);

        dataContext.Bookings.RemoveRange(tour.Bookings);
        dataContext.Wishlists.RemoveRange(tour.Wishlists);
        dataContext.Tours.Remove(tour);
        dataContext.SaveChanges(); 

        return null;
    }

    public Agency DeleteAgency(Agency agency)
    {
        var tours = dataContext.Tours.Where(t => t.AgencyId == agency.Id).ToList();
        var reviews = dataContext.AgencyReviews.Where(r => r.AgencyId == agency.Id).ToList();
        dataContext.AgencyReviews.RemoveRange(reviews);
        dataContext.Tours.RemoveRange(tours);
        dataContext.Agencies.Remove(agency);

        dataContext.SaveChanges();

        return agency;
    }

    public void UpdateDatabase()
    {
        dataContext.SaveChanges();
    }

    public Tour GetTourById(int tourId)
    {
        return dataContext.Tours
            .Include(t => t.Agency)
            .Include(t => t.Bookings)
            .FirstOrDefault(t => t.Id == tourId);
    }

    public List<AgencyReview> ViewAgencyReviews(Agency agency)
    {
        return dataContext.AgencyReviews
            .Include(ar => ar.User)
            .Where(ar => ar.AgencyId == agency.Id)
            .ToList();
    }

    public Agency GetAgencyById(int agencyId)
    {
        return dataContext.Agencies
            .Include(a => a.Owner)
            .Include(a => a.Tours)
            .Include(a => a.Reviews)
            .FirstOrDefault(a => a.Id == agencyId);
    }

    public List<Tour> GetAgencyTours(int agencyId)
    {
        return dataContext.Tours
            .Where(t => t.AgencyId == agencyId)
            .ToList();
    }

    public Tour GetSelectedToir(int tourId)
    {
        return dataContext.Tours.FirstOrDefault(t => t.Id == tourId);
    }
}