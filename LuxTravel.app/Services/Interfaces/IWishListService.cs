using LuxTravel.app.Models;

namespace LuxTravel.app.Services.Interfaces;

public interface IWishListService
{
    void AddToWishList(int userId, int tourId, User logedInUser);
    void getWishListByUserId(User logedInUser);

}
