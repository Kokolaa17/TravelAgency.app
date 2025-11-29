using LuxTravel.app.Data;
using LuxTravel.app.Models;
using LuxTravel.app.Repositories;
using LuxTravel.app.Services;
using System; 

public class Program
{
    static void Main(string[] args)
    {
        UserService UserService = new UserService();
        TourService TourService = new TourService();
        User logedInUser = null;

        while (logedInUser == null)
        {
            string asciiArt = " __        __   _                            _____       _               _____                    _ \r\n \\ \\      / /__| | ___ ___  _ __ ___   ___  |_   _|__   | |   _   ___  _|_   _| __ __ ___   _____| |\r\n  \\ \\ /\\ / / _ \\ |/ __/ _ \\| '_ ` _ \\ / _ \\   | |/ _ \\  | |  | | | \\ \\/ / | || '__/ _` \\ \\ / / _ \\ |\r\n   \\ V  V /  __/ | (_| (_) | | | | | |  __/   | | (_) | | |__| |_| |>  <  | || | | (_| |\\ V /  __/ |\r\n    \\_/\\_/ \\___|_|\\___\\___/|_| |_| |_|\\___|   |_|\\___/  |_____\\__,_/_/\\_\\ |_||_|  \\__,_| \\_/ \\___|_|\r\n                                                                                                    ";

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(asciiArt);
            Console.WriteLine("--------------------------------------------------------------------------------------------------");
            Console.WriteLine();
            Console.ResetColor();

            Console.WriteLine("1. 🔐 Log In");
            Console.WriteLine("2. 📝 Register");
            Console.WriteLine("3. ❌ Exit");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Select an option: ");
            Console.ResetColor();
            
            var userChoice = Console.ReadKey();

            switch (userChoice.KeyChar) 
            { 
                case '1':
                    logedInUser = UserService.UserLogIn();
                    break;
                case '2':
                    Console.Clear();
                    UserService.UserRegistration();
                    break;
                case '3':
                Environment.Exit(0);
                break;
            }
        }
        while(logedInUser != null)
        {
            if (logedInUser.Role == LuxTravel.app.Enums.Enums.userRole.Customer || logedInUser.Role == LuxTravel.app.Enums.Enums.userRole.AgencyOwner)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("🛒 Customer Menu: ");
                Console.WriteLine("------------------");
                Console.ResetColor();
                Console.WriteLine("1. 🌍  View All Tours");
                Console.WriteLine("2. 🔍  Search Filtered Tours");
                Console.WriteLine("3. ⭐  View Wish List");
                Console.WriteLine("4. 🎫  View My Bookings");
                Console.WriteLine("5. 💰  Manage Balance");
                Console.WriteLine("6. 👤  Profile Information");

                if (logedInUser.Role == LuxTravel.app.Enums.Enums.userRole.Customer)
                    Console.WriteLine("7. 🏢  Register Agency");
                else
                    Console.WriteLine("7. 🛠️  Manage Agency");
                Console.WriteLine("8. 🏢 View All Agencies");
                Console.WriteLine("0. ❌ Log Out");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("Select an option: ");
                Console.ResetColor();

                var customerChoice = Console.ReadKey();
                switch (customerChoice.KeyChar)
                {
                    case '1':
                        TourService.SeeAllTour(logedInUser);
                        break;
                        case '2':
                            TourService.SearchFilteredTours(logedInUser);
                        break;
                    case '3':
                        WishListService wishListService = new WishListService();
                        wishListService.getWishListByUserId(logedInUser);
                        break;
                    case '4':
                        BookingService bookingService = new BookingService();
                        bookingService.ViewUserBookings(logedInUser);
                        break;
                    case '5':
                        UserService.ManageBalance(logedInUser);
                        break;
                    case '6':
                        UserService.ProfileInformation(logedInUser);
                        break;
                    case '7':
                        if(logedInUser.Role == LuxTravel.app.Enums.Enums.userRole.Customer)
                        {
                            UserService.RegisterAgency(logedInUser);
                        }
                        else
                        {
                            UserService.ManageAgency(logedInUser);
                        }
                        break;
                    case '8':
                        UserService.ViewAllAgencies(logedInUser);
                        break;
                    case '0':
                        logedInUser = null;
                        Console.Clear();
                        break;
                    default:
                        Console.Clear();
                        break;

                }
            }
        }
    }
}