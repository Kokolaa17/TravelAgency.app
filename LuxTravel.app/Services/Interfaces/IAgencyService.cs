using LuxTravel.app.Models;
using System.Collections.Generic;

namespace LuxTravel.app.Services.Interfaces
{
    public interface IAgencyService
    {
        void CustomerReviews(Agency agency, User logedInUser);
        void ManageTours(Agency agency, User logedInUser);
        void RegisterNewTour(Agency agency, User logedInUser);
        void ViewAgnecyDetails(Agency agency, User logedInUser);
        void ViewFinancalOverview(Agency agency, User logedInUser);
        void ViewPerformacneStatistics(Agency agency, User logedInUser);
        void GenerateAgencyInvoicePdf(Agency agency, User loggedInUser);
    }
}
