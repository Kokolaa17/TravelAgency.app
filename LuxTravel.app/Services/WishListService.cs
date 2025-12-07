using LuxTravel.app.Helpers;
using LuxTravel.app.Models;
using LuxTravel.app.Repositories;
using LuxTravel.app.Services.Interfaces;

namespace LuxTravel.app.Services;

public class WishListService : IWishListService
{
    WishListRepository WishListRepository = new WishListRepository();
    TourRepository TourRepository = new TourRepository();
    Logging loger = new Logging();
    public void AddToWishList(int userId, int tourId, User logedInUser)
    {
        Wishlist wishlist = new Wishlist()
        {
            UserId = userId,
            TourId = tourId
        };

        if (WishListRepository.GetWishListByUser(new User { Id = userId }).Any(w => w.TourId == tourId))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n❌ This tour is already in your wishlist!\n");
            Console.ResetColor();
            Console.WriteLine();

            loger.LogMessage($"Tour already in wishlist", logedInUser);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Press any key to continue...");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
            return;
        }
        else
        {
            var tour = TourRepository.GetTourById(tourId);
            WishListRepository.AddToWishList(wishlist);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✅ '{tour.Name}' has been added to your wishlist!\n");
            Console.ResetColor();

            loger.LogMessage($"Tour {tour.Name} added to wishlist", logedInUser);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Press any key to continue...");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
            return;
        }
    }

    public void getWishListByUserId(User logedInUser)
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine($"{logedInUser.UserName} Wish List: ");
        Console.WriteLine("------------------------------------");
        Console.ResetColor();

        var wishlists = WishListRepository.GetWishListByUser(logedInUser);
        var toursInWishList = wishlists.ToList().Select(w => TourRepository.GetTourById(w.TourId)).ToList();
        var userBooking = new BookingRepository().UserBookings(logedInUser).ToList();


        foreach (var tour in toursInWishList)
        {
            if(userBooking.Any(b => b.TourId == tour.Id))
            {
                continue;
            }
            Console.WriteLine($"[{tour.Id}] 🌍 {tour.Name} - {tour.Destination} - {tour.Price} {tour.Currency}");
        }

        if(toursInWishList.Count == 0)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Your wishlist is empty.");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Press any key to continue...");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
            return;
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("[0] ❌ Exit");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Choose a tour by ID, or enter 0 to exit: ");
            string input = Console.ReadLine();
            int tourId = -1;
            Console.ResetColor();
            if (!int.TryParse(input, out tourId) || tourId < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n❌ Invalid input format. Please enter a positive number or 0.");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
                return; 
            }

            if (tourId == 0)
            {
                Console.Clear();
                return;
            }
            else
            {
                TourRepository tourRepository = new TourRepository();
                var tourDetails = toursInWishList.FirstOrDefault(tour => tour.Id == tourId);
                if (tourDetails != null)
                {
                    Console.Clear();

                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine($"🌍 {tourDetails.Name}");
                    Console.WriteLine("----------------------");
                    Console.ResetColor();

                    Console.WriteLine($"📌 Starting point: {tourDetails.StartingPoint}");
                    Console.WriteLine($"📍 Destination: {tourDetails.Destination}");
                    Console.WriteLine($"📆 Duration: {tourDetails.DurationDays} days, {tourDetails.DurationNights} nights");
                    Console.WriteLine($"👥 Available Spots: {tourDetails.AvailableSpots}");
                    Console.WriteLine("➕ Includes:");
                    foreach (var include in tourDetails.Includes)
                    {
                        Console.WriteLine($" - {include}");
                    }
                    Console.WriteLine($"📝 Description: {tourDetails.Description}");
                    Console.WriteLine();
                    Console.WriteLine($"💲 Price: {tourDetails.Price} {tourDetails.Currency}");
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("----------------------");
                    Console.ResetColor();
                    Console.WriteLine("1. 🔖 Book a tour");
                    Console.WriteLine("2. 🚫 Remove from wish list ");
                    Console.WriteLine("0. ❌ Exit");
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("Choose an option: ");
                    var option = Console.ReadLine();
                    Console.ResetColor();

                    if (option == "0")
                    {
                        Console.Clear();
                        return;
                    }
                    else if (option == "1")
                    {
                        // for booking
                        AgencyRepository agencyRepository = new();
                        BookingService bookingService = new BookingService();
                        var agency = agencyRepository.GetAgencyByOwnerId(logedInUser.Id);

                        bookingService.BookTour(tourDetails, logedInUser, agency);
                    }
                    else if(option == "2")
                    {
                        WishListRepository.removeFromWishList(tourDetails);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n✅ '{tourDetails.Name}' has been removed from your wishlist!\n");
                        Console.ResetColor();
                        Console.WriteLine();

                        loger.LogMessage($"Tour {tourDetails.Name} removed from wishlist", logedInUser);

                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("Press any key to continue...");
                        Console.ResetColor();
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.ResetColor();

                        loger.LogMessage($"Invalid option selected in wishlist", logedInUser);

                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Press any key to continue...");
                        Console.ResetColor();
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid tour ID. Please try again.");
                    Console.ResetColor();

                    loger.LogMessage($"Invalid tour ID entered in wishlist", logedInUser);

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Press any key to continue...");
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
    }
}
