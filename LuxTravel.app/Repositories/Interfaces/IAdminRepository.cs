using LuxTravel.app.Models;

namespace LuxTravel.app.Repositories.Interfaces;

internal interface IAdminRepository
{
    List<Agency> GetAgencyList();
    List<User> GetUsersList();
    List<Tour> GetTourList();
    List<Agency> GetPendingAgencies();
    List<Tour> GetPendingTours();
    User GetUserById(int id);
    Agency GetAgencyById(int id);
    Tour GetTourById(int id);
    void UpdateAgency(Agency agency);
    void UpdateTour(Tour tour);
}
