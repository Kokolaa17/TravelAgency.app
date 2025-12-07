using LuxTravel.app.Models;

namespace LuxTravel.app.Repositories.Interfaces;

internal interface IAgencyRepository
{
    Tour RegisterTour(Tour newTour);
    Agency GetAgencyByOwnerId(int ownerId);
    List<Tour> GetAllTours();
    User GetAgencyOwnerById(int agencyId);
    User GetUserById(int userId);
    void UpdateAgency(Agency agency);
    void UpdateAgencyTwo(Agency agency);
    void UpdateUser(User logedInUser);
    void UpdateUserTwo(User logedInUser);
    Tour RemoveTour(int tourId);
    Agency DeleteAgency(Agency agency);
    void UpdateDatabase();
    Tour GetTourById(int tourId);
    List<AgencyReview> ViewAgencyReviews(Agency agency);
    Agency GetAgencyById(int agencyId);
    List<Tour> GetAgencyTours(int agencyId);
    Tour GetSelectedToir(int tourId);
}
