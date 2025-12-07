using LuxTravel.app.Data;
using LuxTravel.app.Helpers;
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
        Admin logedInAdmin = null;
        Logging logger = new Logging();

        while (logedInUser == null && logedInAdmin == null)
        {
            string asciiArt = " __        __   _                            _____       _               _____                    _ \r\n \\ \\      / /__| | ___ ___  _ __ ___   ___  |_   _|__   | |   _   ___  _|_   _| __ __ ___   _____| |\r\n  \\ \\ /\\ / / _ \\ |/ __/ _ \\| '_ ` _ \\ / _ \\   | |/ _ \\  | |  | | | \\ \\/ / | || '__/ _` \\ \\ / / _ \\ |\r\n   \\ V  V /  __/ | (_| (_) | | | | | |  __/   | | (_) | | |__| |_| |>  <  | || | | (_| |\\ V /  __/ |\r\n    \\_/\\_/ \\___|_|\\___\\___/|_| |_| |_|\\___|   |_|\\___/  |_____\\__,_/_/\\_\\ |_||_|  \\__,_| \\_/ \\___|_|\r\n                                                                                                    ";

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(asciiArt);
            Console.WriteLine("--------------------------------------------------------------------------------------------------");
            Console.WriteLine();
            Console.ResetColor();

            Console.WriteLine("1. 🔐 Log In");
            Console.WriteLine("2. 📝 Register");
            Console.WriteLine("3. 🔑 Reset Password");
            Console.WriteLine("4. ⚙️ Log In As Admin");
            Console.WriteLine("0. ❌ Exit");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Select an option: ");
            Console.ResetColor();

            var userChoice = Console.ReadKey();

            switch (userChoice.KeyChar) 
            { 
                case '1':
                    logger.LogMessage("Attempting user login.", new User { UserName = "Guest" });
                    logedInUser = UserService.UserLogIn();
                    break;
                case '2':
                    logger.LogMessage("Attempting user registration.", new User { UserName = "Guest" });
                    Console.Clear();
                    UserService.UserRegistration();
                    break;
                case '3':
                    logger.LogMessage("Attempting password reset.", new User { UserName = "Guest" });
                    Console.Clear();
                    UserService.resetPassword();
                    break;
                case '4':
                    logger.LogMessage("Attempting to log in as admin", new User { UserName = "Guest" });
                    logedInAdmin = UserService.AdminLogIn();
                    break;
                case '0':
                    logger.LogMessage("Application exited by user.", new User { UserName = "Guest" });
                    Environment.Exit(0);
                break;
                default:
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInvalid option. Please try again.");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Press any key to continue...");
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.Clear();
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
                        logger.LogMessage("Viewing all tours.", logedInUser);
                        TourService.SeeAllTour(logedInUser);
                        break;
                    case '2':
                        logger.LogMessage("Searching filtered tours.", logedInUser);
                        TourService.SearchFilteredTours(logedInUser);
                        break;
                    case '3':
                        WishListService wishListService = new WishListService();
                        logger.LogMessage("Viewing wish list.", logedInUser);
                        wishListService.getWishListByUserId(logedInUser);
                        break;
                    case '4':
                        logger.LogMessage("Viewing user bookings.", logedInUser);
                        BookingService bookingService = new BookingService();
                        bookingService.ViewUserBookings(logedInUser);
                        break;
                    case '5':
                        logger.LogMessage("Managing balance.", logedInUser);
                        UserService.ManageBalance(logedInUser);
                        break;
                    case '6':
                        logger.LogMessage("Viewing profile information.", logedInUser);
                        UserService.ProfileInformation(logedInUser);
                        break;
                    case '7':
                        if (logedInUser.Role == LuxTravel.app.Enums.Enums.userRole.Customer)
                        {
                            logger.LogMessage("Registering agency.", logedInUser);
                            UserService.RegisterAgency(logedInUser);
                        }
                        else
                        {
                            logger.LogMessage("Managing agency.", logedInUser);
                            UserService.ManageAgency(logedInUser);
                        }
                        break;
                    case '8':
                        logger.LogMessage("Viewing all agencies.", logedInUser);
                        UserService.ViewAllAgencies(logedInUser);
                        break;
                    case '0':
                        logger.LogMessage("User logged out.", logedInUser);
                        logedInUser = null;
                        Console.Clear();
                        break;
                    default:
                        Console.Clear();
                        break;
                }
            }
        }
        while(logedInAdmin != null)
        {
            AdminService AdminService = new AdminService();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("⚙️ Admin Panel");
            Console.WriteLine("--------------");
            Console.ResetColor();
            Console.WriteLine("1. 💼 View Pending Agencies");
            Console.WriteLine("2. 🎫 View Pending Tours");
            Console.WriteLine("3. 🏢 View All Approved Agencies");
            Console.WriteLine("4. ✈️ View All Approved Tours");  
            Console.WriteLine("5. 👥 View All Users");           
            Console.WriteLine("0. ❌ Exit");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Select an option: ");
            Console.ResetColor();

            var adminChoice = Console.ReadKey();
            switch (adminChoice.KeyChar)
            {
                case '1':
                    logger.LogMessage("Viewing pending agencies.", new User { UserName = "Admin" });
                    AdminService.ViewPendingAgencies();
                    break;
                case '2':
                    logger.LogMessage("Viewing pending tours", new User { UserName = "Admin" });
                    AdminService.ViewPendingTours();
                    break;
                case '3':
                    logger.LogMessage("Viewing all agencies.", new User { UserName = "Admin" });
                    AdminService.ViewAllAgencies();
                    break;
                case '4':
                    logger.LogMessage("Viewing all approved tours.", new User { UserName = "Admin" });

                    break;
                case '5':
                    logger.LogMessage("Viewing all users.", new User { UserName = "Admin" });
                    AdminService.ViewAllUsers();
                    break;
                case '0':
                    Console.Clear();
                    Environment.Exit(0);
                    break;
                default:
                    Console.Clear();
                    break;
            }
        }
    }
}

