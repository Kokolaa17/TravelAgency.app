using LuxTravel.app.Models;

namespace LuxTravel.app.Services.Interfaces;

internal interface IAgencyService
{
    void RegisterNewTour(Agency agency);
    void ManageTours(Agency agency);
    void CustomerReviews(Agency agency);
    void ViewFinancalOverview(Agency agency);
    void ViewPerformacneStatistics(Agency agency);
    void ViewAgnecyDetails(Agency agency);
}
