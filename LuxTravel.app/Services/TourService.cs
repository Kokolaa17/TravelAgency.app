using LuxTravel.app.Migrations;
using LuxTravel.app.Models;
using LuxTravel.app.Repositories;
using LuxTravel.app.Services.Interfaces;

namespace LuxTravel.app.Services;

internal class TourService : ITourService
{
    TourRepository TourRepository = new TourRepository();

    public void FilterByDestination()
    {
        Console.Clear();

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine($"🗺️ Filter by destination:");
        Console.WriteLine("---------------------------");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("Enter Destionation: ");
        string destination = Console.ReadLine() ?? "";
        Console.ResetColor();
        var filteredTours = TourRepository.FilterByDestination(destination);

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine($"🗺️ Tours to {destination}:");
        Console.WriteLine("----------------------");
        Console.ResetColor();

        if(filteredTours.Count == 0)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No tours found for the specified destination.");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Press any key to continue...");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
            return;
        }
        else
        {
            foreach (var tour in filteredTours)
            {
                Console.WriteLine($"[{tour.Id}] 🌍 {tour.Name} - {tour.Destination} - {tour.Price} {tour.Currency}");
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Press any key to continue...");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }
    }

    public void FilterByDuration()
    {
        Console.Clear();

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine($"🗺️ Filter by duration");
        Console.WriteLine("-----------------------");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("Enter minimum days: ");
        string input = Console.ReadLine() ?? "";
        Console.Write("Enter minimum maximum days: ");
        string input2 = Console.ReadLine() ?? "";
        Console.ResetColor();
        if (!int.TryParse(input, out int durationDays) || !int.TryParse(input2, out int durationNights))
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("⚠️ Invalid input. Please enter valid numbers for days and nights.");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Press any key to continue...");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
            return;
        }
        else
        {
            var filteredTours = TourRepository.FilterByDuration(durationDays, durationNights);
            if(filteredTours.Count == 0)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No tours found for the specified duration.");
                Console.ResetColor();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
                return;
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"🗺️ Tours with duration {durationDays} days and {durationNights} nights:");
                Console.WriteLine("-------------------------------------------------");
                Console.ResetColor();
                foreach (var tour in filteredTours)
                {
                    Console.WriteLine($"[{tour.Id}] 🌍 {tour.Name} - {tour.Destination} - {tour.Price} {tour.Currency}");
                }
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
            }
        }
            
    }

    public void FilterByPrice()
    {
        Console.Clear();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine($"🗺️ Filter by price");
        Console.WriteLine("-----------------------");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("Enter minimum price: ");
        string input = Console.ReadLine() ?? "";
        Console.Write("Enter maximum price: ");
        string input2 = Console.ReadLine() ?? "";
        Console.ResetColor();

        if(!decimal.TryParse(input, out decimal minPrice) || !decimal.TryParse(input2, out decimal maxPrice))
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("⚠️ Invalid input. Please enter valid numbers for prices.");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Press any key to continue...");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
            return;
        }
        else
        {
            var filteredTours = TourRepository.FilterByPrice(minPrice, maxPrice);
            if (filteredTours.Count == 0)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No tours found for the specified price range.");
                Console.ResetColor();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
                return;
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"🗺️ Tours with price between {minPrice} and {maxPrice}:");
                Console.WriteLine("-------------------------------------------------");
                Console.ResetColor();
                foreach (var tour in filteredTours)
                {
                    Console.WriteLine($"[{tour.Id}] 🌍 {tour.Name} - {tour.Destination} - {tour.Price} {tour.Currency}");
                }
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
            }
        }

    }

    public void FilterByMinAge()
    {
        Console.Clear();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine($"🗺️ Filter by age");
        Console.WriteLine("-----------------------");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("Enter your age: ");
        var input = Console.ReadLine() ?? "";
        Console.ResetColor();
        if(!int.TryParse(input, out int userAge))
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("⚠️ Invalid input. Please enter a valid age.");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Press any key to continue...");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
            return;
        }
        else
        {
            var filteredTours = TourRepository.FilterByMinAge(userAge);
            if (filteredTours.Count == 0)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"🗺️ Tours suitable for age {userAge} and above:");
                Console.WriteLine("-------------------------------------------------");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No tours found suitable for the specified age.");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
                return;
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"🗺️ Tours suitable for age {userAge} and above:");
                Console.WriteLine("-------------------------------------------------");
                Console.ResetColor();

                foreach (var tour in filteredTours)
                {
                    Console.WriteLine($"[{tour.Id}] 🌍 {tour.Name} - {tour.Destination} - {tour.Price} {tour.Currency}");
                }

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
            }
        }

    }

    public void GetTourById(int tourId)
    {
        var tour = TourRepository.GetTourById(tourId);

        Console.Clear();

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine($"{tour.Name} details:");
        Console.WriteLine("----------------------");
        Console.ResetColor();
        Console.WriteLine($"Destination: {tour.Destination}");
        Console.WriteLine($"Duration: {tour.DurationDays} days, {tour.DurationNights} nights");
        Console.WriteLine($"Available Spots: {tour.AvailableSpots}");
        Console.WriteLine($"Price: {tour.Price} {tour.Currency}");
        Console.WriteLine($"Agency: {tour.Agency}");
        Console.WriteLine($"Description: {tour.Description}");
    }

    public void SearchFilteredTours(User logedInUser)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine($"🗺️ Filter tours:");
        Console.WriteLine("------------------");
        Console.ResetColor();

        Console.WriteLine("1. 📍 Filter by Destination");
        Console.WriteLine("2. ⏳ Filter by Duration");
        Console.WriteLine("3. 💲 Filter by Price");
        Console.WriteLine("4. 🧳 Filter by Minimum Age");
        Console.WriteLine("0. ❌ Exit");

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("Select a filter option: ");
        string filterChoice = Console.ReadLine();
        Console.ResetColor();

        switch (filterChoice)
        {
            case "1":
                FilterByDestination();
                break;
            case "2":
                FilterByDuration();
                break;
            case "3":
                FilterByPrice();
                break;
            case "4":
                FilterByMinAge();
                break;
            case "0":
                Console.Clear();
                return;
            default:
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("⚠️ Invalid input. Please enter a valid option.");
                Console.ResetColor();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
                return;
        }
    }

    public void SeeAllTour(User logedInUser)
    {
        Console.Clear();
        List<Tour> tours = TourRepository.SeeAllTour();

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine($"🗺️ All tours:");
        Console.WriteLine("--------------");
        Console.ResetColor();

        if (tours.Count == 0)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No tours available at the moment.");
            Console.ResetColor();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Press any key to continue...");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
            return;
        }
        else
        {
            foreach (var tour in tours)
            {
                Console.WriteLine($"[{tour.Id}] 🌍 {tour.Name} - {tour.Destination} - {tour.Price} {tour.Currency}");
            }
        }
        Console.WriteLine();
        Console.WriteLine("[0] ❌ Exit");

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("Choose a tour by ID, or enter 0 to exit: ");
        string input = Console.ReadLine();
        Console.ResetColor();

        if (!int.TryParse(input, out int tourId))
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("⚠️ Invalid input. Please enter a valid ID.");
            Console.ResetColor();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Press any key to continue...");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
            return;
        }
        else if( tourId == 0)
        {
            Console.Clear();
            return;
        }
        else
        {
            var tour = TourRepository.GetTourById(tourId);
            if (tour != null)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"🌍 {tour.Name}");
                Console.WriteLine("----------------------");
                Console.ResetColor();

                Console.WriteLine($"📌 Starting point: {tour.StartingPoint}");
                Console.WriteLine($"📍 Destination: {tour.Destination}");
                Console.WriteLine($"📆 Duration: {tour.DurationDays} days, {tour.DurationNights} nights");
                Console.WriteLine($"👥 Available Spots: {tour.AvailableSpots}");
                Console.WriteLine("➕ Includes:");
                foreach (var include in tour.Includes)
                {
                    Console.WriteLine($" - {include}");
                }
                Console.WriteLine($"📝 Description: {tour.Description}");
                Console.WriteLine();
                Console.WriteLine($"💲 Price: {tour.Price} {tour.Currency}");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("----------------------");
                Console.ResetColor();
                Console.WriteLine("1. 🔖 Book a tour");
                Console.WriteLine("2. ⭐ Add to wish list");
                Console.WriteLine("0. ❌ Exit");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("Select a option: ");
                string detailsInput = Console.ReadLine()?.Trim() ?? "";
                Console.ResetColor();

                if (detailsInput == "0")
                {
                    Console.Clear();
                    return;
                }
                else if (detailsInput == "1")
                {
                    // for booking
                    AgencyRepository agencyRepository = new AgencyRepository();
                    BookingService bookingService = new BookingService();
                    var agency = agencyRepository.GetAgencyByOwnerId(logedInUser.Id);

                    bookingService.BookTour(tour, logedInUser, agency);
                }
                else if (detailsInput == "2")
                {
                    WishListService wishListService = new WishListService();
                    wishListService.AddToWishList(logedInUser.Id, tour.Id);
                }
                else
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("⚠️ Invalid input. Please enter a valid ID.");
                    Console.ResetColor();

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Press any key to continue...");
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }
            }
        }
    }
}
