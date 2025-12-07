using LuxTravel.app.Models;

namespace LuxTravel.app.Services.Interfaces;

internal interface IUserService
{
    User UserLogIn();
    void UserRegistration();
    void ManageAgency(User logedInUser);
    void RegisterAgency(User logedInUser);
    void ManageBalance(User logedInUser);
    void ProfileInformation(User logedInUser);
    void ViewAllAgencies(User logedInUser);
    void resetPassword();
}
