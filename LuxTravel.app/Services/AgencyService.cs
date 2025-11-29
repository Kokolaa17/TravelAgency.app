using LuxTravel.app.Data;
using LuxTravel.app.Models;
using LuxTravel.app.Repositories;
using LuxTravel.app.Repositories.Interfaces;
using LuxTravel.app.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System.Transactions;

namespace LuxTravel.app.Services;

internal class AgencyService : IAgencyService
{
    AgencyRepository agencyRepository = new AgencyRepository();
    public void CustomerReviews(Agency agency)
    {
        Console.Clear();

        Console.WriteLine("💬 Customer Reviews");
        Console.WriteLine("-------------------");

        var reviews = agencyRepository.ViewAgencyReviews(agency);

        if (reviews.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No reviews available for your agency yet.");
            Console.ResetColor();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            Console.ResetColor();
        }
        else
        {
            foreach (var review in reviews)
            {
                var user = agencyRepository.getUser(review.UserId);

                Console.WriteLine($"⭐ Rating: {review.Rating}/5");
                Console.WriteLine($"🗨️ Comment: {review.Comment}");
                Console.WriteLine($"👤 By User: {user.UserName} on {review.CreatedAt}");
                Console.WriteLine("-------------------");

            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            Console.ResetColor();
            return;
        }

    }

    public void ManageTours(Agency agency)
    {
        
        DataContext dataContext = new DataContext();

        Console.Clear();
        Console.WriteLine("📂 Manage Tours");
        Console.WriteLine("----------------");

        
        var agencyTours = dataContext.Tours
            .Where(t => t.AgencyId == agency.Id)
            .ToList();

        foreach (var tour in agencyTours)
        {
            Console.WriteLine($"[{tour.Id}] 📍 {tour.StartingPoint} - {tour.Destination} - {tour.Price} {tour.Currency}");
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
        else if (tourId == 0)
        {
            Console.Clear();
            return;
        }
        else
        {
            var selectedTour = dataContext.Tours
                .Include(t => t.Bookings)
                .FirstOrDefault(t => t.Id == tourId);

            if (selectedTour == null)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("⚠️ No tour found with the given ID.");
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
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"🌍 {selectedTour.Name} details:");
                Console.WriteLine("----------------------");
                Console.ResetColor();
                Console.WriteLine($"📌 Starting Point: {selectedTour.StartingPoint}");
                Console.WriteLine($"📍 Destination: {selectedTour.Destination}");
                Console.WriteLine($"📆 Duration: {selectedTour.DurationDays} days, {selectedTour.DurationNights} nights");
                Console.WriteLine($"👥 Current Participant: {selectedTour.CurrentParticipants}");
                Console.WriteLine($"👥 Maximum Participant: {selectedTour.MaxParticipants}");
                Console.WriteLine($"📝 Description: {selectedTour.Description}");
                Console.WriteLine($"💲 Price: {selectedTour.Price} {selectedTour.Currency}");
                Console.WriteLine($"💰 Income from this tour: {selectedTour.Price * selectedTour.CurrentParticipants} {selectedTour.Currency}");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("----------------------");
                Console.ResetColor();
                Console.WriteLine("1. 🗑  Delete tour");
                Console.WriteLine("0. ❌ Exit");
                var choice = Console.ReadKey();

                if (choice.KeyChar == '1')
                {
                    Console.WriteLine();
                    Console.WriteLine();

                    
                    var trackedAgency = dataContext.Agencies.Find(agency.Id);

                    
                    trackedAgency.TotalToursCreated -= 1;
                    trackedAgency.TotalEarnings -= selectedTour.Price * selectedTour.CurrentParticipants;
                    trackedAgency.TotalBookings -= selectedTour.CurrentParticipants;
                    trackedAgency.Balance -= selectedTour.Price * selectedTour.CurrentParticipants;

                    
                    foreach (var booking in selectedTour.Bookings)
                    {
                        var user = dataContext.Users.Find(booking.UserId);
                        user.Balance += booking.TotalPrice;
                    }

                    
                    agencyRepository.removeTour(selectedTour.Id);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("✅ Tour deleted successfully.");
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.ResetColor();
                }
                else if (choice.KeyChar == '0')
                {
                    Console.Clear();
                    return;
                }
                return;
            }
        }
    }

    public void RegisterNewTour(Agency agency)
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("📝 Tour Registration");
        Console.WriteLine("--------------------");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Tour registration costs 50 GEL and will be deducted from your agency balance.");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("Press any key to continue...");
        Console.ReadKey();
        Console.Clear();
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("📝 Tour Registration");
        Console.WriteLine("--------------------");
        Console.ResetColor();

        if(agency.Balance < 50)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Insufficient funds in your agency balance to register a new tour.");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            Console.ResetColor();
            return;
        }
        else
        {
            Console.Write("Enter Tour Name\n>> ");
            string tourName = Console.ReadLine();

            Console.Write("Enter Starting Point\n>> ");
            string startingPoint = Console.ReadLine();

            Console.Write("Enter Destination\n>> ");
            string destination = Console.ReadLine();

            Console.Write("Enter Tour Description\n>> ");
            string description = Console.ReadLine();

            Console.Write("Enter Start Date (yyyy-mm-dd)\n>> ");
            DateTime startDate = DateTime.Parse(Console.ReadLine() ?? DateTime.Now.ToString());

            Console.Write("Enter End Date (yyyy-mm-dd)\n>> ");
            DateTime endDate = DateTime.Parse(Console.ReadLine() ?? DateTime.Now.ToString());

            Console.Write("Enter Maximum Participants\n>> ");
            int maxParticipants = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Enter Minimum Age\n>> ");
            int minAge = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Enter What Tour Includes (comma-separated list)\n>> ");
            string includesInput = Console.ReadLine();
            List<string> includes = includesInput.Split(',').Select(x => x.Trim()).ToList();

            Console.Write("Enter Price\n>> ");
            decimal price = decimal.Parse(Console.ReadLine());

            var tourValidator = new Validators.TourValidator();
            var newTour = new Tour
            {
                Name = tourName,
                StartingPoint = startingPoint,
                Destination = destination,
                Description = description,
                StartDate = startDate,
                EndDate = endDate,
                MaxParticipants = maxParticipants,
                MinAge = minAge,
                Includes = includes,
                Price = price,
                AgencyId = agency.Id
            };
            var tourValidationResult = tourValidator.Validate(newTour);

            if (!tourValidationResult.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var error in tourValidationResult.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                Console.ResetColor();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
                Console.ResetColor();
                return;
            }
            else
            {
                var registeredTour = agencyRepository.registerTour(newTour);
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ Tour named: **{registeredTour.Name}** registered successfully!");
                Console.WriteLine("");
                agency.Balance -= 50;
                agency.TotalToursCreated += 1;

                agencyRepository.UpdateAgency(agency);
                agencyRepository.UpdateDatabase();
                Console.ResetColor();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
                Console.ResetColor();
                return;
            }
        }
    }

        

    public void ViewAgnecyDetails(Agency agency)
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("🏢  Agency Profile");
        Console.WriteLine("------------------");
        Console.ResetColor();

        Console.WriteLine($"🆔 Agency Name: {agency.Name}");
        Console.WriteLine($"🪪 Owner: {agency.Owner.UserName}");
        Console.WriteLine($"🌍 Ageny country: {agency.Country}");
        Console.WriteLine($"🏙️ Agency city: {agency.City}");
        Console.WriteLine($"📬 Agency address: {agency.Address}");
        Console.WriteLine($"📝 Description: {agency.Description}");
        Console.WriteLine($"🧮 Total Tours Created: {agency.TotalToursCreated}");
        Console.WriteLine($"📅 Established On: {agency.CreatedAt.ToShortDateString()}");

        Console.WriteLine();
        Console.WriteLine("1. 🚫 Delete Agency");
        Console.WriteLine("0. ❌ Exit");
        var choice = Console.ReadKey().KeyChar;

        switch (choice)
        {
            case '1':
                agencyRepository.DeleteAgency(agency);
                var owner =  agencyRepository.getUser(agency.OwnerId);
                owner.Role = Enums.Enums.userRole.Customer;
                owner.AgencyId = null;
                owner.Balance += agency.Balance;

                agencyRepository.UpdateUser(owner);

                Console.WriteLine();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✅ Agency deleted successfully.");
                Console.ResetColor();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
                Console.ResetColor();
                break;
            case '0':
                Console.Clear();
                return;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Invalid option selected.");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
                Console.ResetColor();

                break;
        }          
    }

    public void ViewFinancalOverview(Agency agency)
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("💰  Financial Overview");
        Console.WriteLine("-----------------------");
        Console.ResetColor();
        Console.WriteLine($"Balance: {agency.Balance} GEL");
        Console.WriteLine();
        Console.WriteLine("1.💸 Withdraw");
        Console.WriteLine("0.❌ Exit");

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("Select an option: ");
        Console.ResetColor();
        var agencyOwner = agency.Owner;
        var choice = Console.ReadKey();

        switch (choice.KeyChar)
        {
            case '1':
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("💸 Withdraw");
                Console.WriteLine("------------");
                Console.ResetColor();
                Console.Write("Enter amount to withdraw into personal account:\n>> ");
                string input = Console.ReadLine();
                decimal withdrawAmount;

                if (!decimal.TryParse(input, out withdrawAmount))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("⚠️ Invalid amount. Please enter a valid number.");
                    Console.ResetColor();
                    return; 
                }
                else if (agency.Balance < withdrawAmount)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ Insufficient funds in your agency balance to make this withdrawal.");
                    Console.ResetColor();

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.ResetColor();
                }
                else
                {
                    agencyOwner.Balance += withdrawAmount;
                    agency.Balance -= withdrawAmount;

                    agencyRepository.UpdateUser(agencyOwner);
                    agencyRepository.UpdateDatabase();
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"✅ Withdrew {withdrawAmount} to your personal account successfully!");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.ResetColor();
                }
                break;
                case '0':
                Console.Clear();
                    return;
                break;
        }
    }

    public void ViewPerformacneStatistics(Agency agency)
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("📊  Performance Statistics");
        Console.WriteLine("---------------------------");
        Console.ResetColor();

        Console.WriteLine($"🗺️ Total Tours Created: {agency.TotalToursCreated}");
        Console.WriteLine($"📔 Total Bookings: {agency.TotalBookings}");
        Console.WriteLine($"💰 Total Earnings: {agency.TotalEarnings} GEL");
        Console.WriteLine();
        Console.WriteLine($"0. ❌ Exit");

        if (Console.ReadKey().KeyChar == '0')
        {
            Console.Clear();
            return;
        }
    }
}
