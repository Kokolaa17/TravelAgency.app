using LuxTravel.app.Models;

namespace LuxTravel.app.Services.Interfaces;

internal interface ITourService
{
    void FilterByDestination();
    void FilterByDuration();
    void FilterByPrice();
    void FilterByMinAge();
    void GetTourById(int tourId);
    void SearchFilteredTours(User loggedInUser);
    void SeeAllTour(User loggedInUser);
}
