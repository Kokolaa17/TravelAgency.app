using LuxTravel.app.Models;

namespace LuxTravel.app.Repositories.Interfaces;

internal interface IAgencyRepository
{
    Agency GetAgencyByOwnerId(int ownerId);
    Tour registerTour(Tour newTour);
    List<Tour> GetAllTours();
    User getUser(int userId);
    Tour removeTour(int tourId);
    void UpdateAgency(Agency agency);
    void UpdateUser(User user);
    Agency DeleteAgency(Agency agency);
    void UpdateDatabase();
    Tour getTourById(int tourId);
    List<AgencyReview> ViewAgencyReviews(Agency agency);
}
