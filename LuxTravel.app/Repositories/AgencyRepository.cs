using LuxTravel.app.Data;
using LuxTravel.app.Models;
using LuxTravel.app.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using static LuxTravel.app.Enums.Enums;

namespace LuxTravel.app.Repositories;

internal class AgencyRepository : IAgencyRepository
{
    DataContext dataContext = new DataContext();
    public Tour registerTour(Tour newTour)
    {
        dataContext.Add(newTour);
        newTour.Status = Enums.Enums.TourStatus.Published;
        dataContext.SaveChanges();

        return newTour;
    }

    public Agency GetAgencyByOwnerId(int ownerId)
    {
        return dataContext.Agencies
            .Include(a=> a.Tours)
            .Include(a=> a.Owner)
            .FirstOrDefault(a => a.OwnerId == ownerId);
    }

    public List<Tour> GetAllTours()
    {
        return dataContext.Tours.ToList();
    }

    public User getUser(int userId)
    {
        return dataContext.Users
        .Include(u => u.OwnedAgency)  
        .FirstOrDefault(u => u.Id == userId);
    }

    public void UpdateAgency(Agency agency)
    {
        dataContext.Agencies.Update(agency);
    }

    public void UpdateUser(User user)
    {
       dataContext.Users.Update(user);
    }

    public Tour removeTour(int tourId)
    {
        var tourToRemove = dataContext.Tours
            .Include(t => t.Bookings)
            .Include(t => t.Wishlists)
            .Include(t => t.Agency)
            .FirstOrDefault(t => t.Id == tourId);

        dataContext.Wishlists.RemoveRange(dataContext.Wishlists.Where(w => w.TourId == tourId));
        dataContext.Bookings.Where(b => b.TourId == tourId).ToList().ForEach(b => dataContext.Bookings.Remove(b));
        dataContext.Tours.Remove(tourToRemove);

        dataContext.SaveChanges();
        return null;
    }

    public Agency DeleteAgency(Agency agency)
    {
        var tours = dataContext.Tours.Where(t => t.AgencyId == agency.Id).ToList();
        dataContext.Tours.RemoveRange(tours);
        dataContext.Agencies.Remove(agency);
        var reviews = dataContext.AgencyReviews.Where(r => r.AgencyId == agency.Id).ToList();
        dataContext.AgencyReviews.RemoveRange(reviews);
        dataContext.SaveChanges();
        return agency;
    }

    public void UpdateDatabase()
    {
        dataContext.SaveChanges();
    }

    public Tour getTourById(int tourId)
    {
        return dataContext.Tours
            .Include(t => t.Agency)    
            .Include(t => t.Bookings) 
            .FirstOrDefault(t => t.Id == tourId);
    }

    public List<AgencyReview> ViewAgencyReviews(Agency agency)
    {
        return dataContext.AgencyReviews.Include(ar => ar.User)    
            .Where(ar => ar.AgencyId == agency.Id)
            .ToList();
    }
}
