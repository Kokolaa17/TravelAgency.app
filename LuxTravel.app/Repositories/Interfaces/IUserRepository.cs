using LuxTravel.app.Models;

namespace LuxTravel.app.Repositories.Interfaces;

public interface IUserRepository
{
    User UserRegistration(User newUser);
    User UserLogIn(string emailOrPassword, string password);
    User getUserByEmail(string email);
    void updateUserPassword(User user);
    Agency RegisterAgency(Agency newAgency);
    List<Agency> ViewAllAgencies();
    void AddAgencyReview(AgencyReview newReview);
    AgencyReview GetAllAgencyRevies(User user, Agency agency);
    Agency DeleteAgency(Agency agency, User logedInUser);
}
