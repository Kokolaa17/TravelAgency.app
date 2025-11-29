using LuxTravel.app.Models;

namespace LuxTravel.app.Repositories.Interfaces;

internal interface IWishListRepository
{
    Wishlist AddToWishList(Wishlist wish);

    List<Wishlist> GetWishListByUser(User logedInUser);
    Wishlist removeFromWishList(Tour tour);

}
