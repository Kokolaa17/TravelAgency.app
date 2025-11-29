using LuxTravel.app.Models;

namespace LuxTravel.app.Repositories.Interfaces;

internal interface ITourRepository
{
    List<Tour> SeeAllTour();

    List<Tour> FilterByMinAge(int minAge);
    List<Tour> FilterByPrice(decimal minPrice, decimal maxPrice);
    List<Tour> FilterByDestination(string destination);
    List<Tour> FilterByDuration(int minDays, int maxDays);
    Tour GetTourById(int id); 
    void CreateBooking(int userId, int tourId, int numberOfPeople);

}
