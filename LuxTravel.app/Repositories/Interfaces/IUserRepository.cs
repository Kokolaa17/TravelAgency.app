using LuxTravel.app.Models;

namespace LuxTravel.app.Repositories.Interfaces;

public interface IUserRepository
{
    User UserRegistration(User newUser);
    User UserLogIn(string emailOrPassword, string password);
    Agency RegisterAgency(Agency newAgency);
    List<Agency> ViewAllAgencies();
    void AddAgencyReview(AgencyReview newReview);
    AgencyReview GetAllAgencyRevies(User user, Agency agency);
}
