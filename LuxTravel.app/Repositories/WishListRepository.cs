using LuxTravel.app.Data;
using LuxTravel.app.Models;
using LuxTravel.app.Repositories.Interfaces;

namespace LuxTravel.app.Repositories;

internal class WishListRepository : IWishListRepository
{
    DataContext DataContext = new DataContext();
    public Wishlist AddToWishList(Wishlist wish)
    {
        DataContext.Wishlists.Add(wish);
        DataContext.SaveChanges();
        return wish;
    }

    public List<Wishlist> GetWishListByUser(User logedInUser)
    {
        return DataContext.Wishlists.Where(w => w.UserId == logedInUser.Id).ToList();
    }

    public Wishlist removeFromWishList(Tour tour)
    {
        var wish = DataContext.Wishlists.FirstOrDefault(w => w.TourId == tour.Id);

        DataContext.Wishlists.Remove(wish);
        DataContext.SaveChanges();
        return wish;
    }
}
