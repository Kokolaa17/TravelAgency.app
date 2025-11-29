using LuxTravel.app.Models;

namespace LuxTravel.app.Services.Interfaces;

public interface IWishListService
{
    void AddToWishList(int userId, int tourId);
    void getWishListByUserId(User logedInUser);

}
