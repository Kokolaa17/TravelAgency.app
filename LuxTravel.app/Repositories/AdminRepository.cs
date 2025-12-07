using LuxTravel.app.Data;
using LuxTravel.app.Models;
using LuxTravel.app.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace LuxTravel.app.Repositories;

internal class AdminRepository : IAdminRepository
{
    DataContext dataContext = new DataContext();
    public Agency GetAgencyById(int id)
    {
        return dataContext.Agencies
            .Include(o => o.Owner)
            .FirstOrDefault(a => a.Id == id);
    }

    public List<Agency> GetAgencyList()
    {
        return dataContext.Agencies
            .Include(o => o.Owner)
            .Where(a => a.Status == Enums.Enums.AgencyStatus.Approved).ToList();
    }

    public List<Agency> GetPendingAgencies()
    {
        return dataContext.Agencies.Where(a => a.Status == Enums.Enums.AgencyStatus.Pending).ToList();
    }

    public List<Tour> GetPendingTours()
    {
        return dataContext.Tours.Where(t => t.Status == Enums.Enums.TourStatus.Draft).ToList();
    }

    public Tour GetTourById(int id)
    {
        return dataContext.Tours
            .Include(t => t.Agency)
            .FirstOrDefault(t => t.Id == id);
    }

    public List<Tour> GetTourList()
    {
        return dataContext.Tours
            .Include (t => t.Agency)
            .Where(t => t.Status == Enums.Enums.TourStatus.Published).ToList();
    }

    public User GetUserById(int id)
    {
        return dataContext.Users
            .Include(u => u.Bookings)
            .Include(u => u.AgencyReviews)
            .FirstOrDefault(u => u.Id == id);
    }

    public List<User> GetUsersList()
    {
        return dataContext.Users.ToList();
    }
    public void UpdateAgency(Agency agency)
    {
        dataContext.Agencies.Update(agency);
        dataContext.SaveChanges();
    }

    public void UpdateTour(Tour tour)
    {
        dataContext.Tours.Update(tour);
        dataContext.SaveChanges();
    }
}
