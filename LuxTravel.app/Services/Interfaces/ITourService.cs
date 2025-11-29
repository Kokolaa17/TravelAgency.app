using LuxTravel.app.Models;

namespace LuxTravel.app.Services.Interfaces;

internal interface ITourService
{
    void SeeAllTour(User logedInUser);
    void FilterByPrice();
    void FilterByDestination();
    void FilterByDuration();
    void FilterByMinAge();
    void GetTourById(int tourId);
    void SearchFilteredTours(User logedInUser);
}
