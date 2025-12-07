using LuxTravel.app.Data;
using LuxTravel.app.Helpers;
using LuxTravel.app.Models;
using LuxTravel.app.Repositories;
using LuxTravel.app.Repositories.Interfaces;
using LuxTravel.app.Services.Interfaces;
using LuxTravel.app.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System.Transactions;

namespace LuxTravel.app.Services;

internal class AgencyService : IAgencyService
{
    private readonly AgencyRepository agencyRepository = new AgencyRepository();
    private readonly Logging logger = new Logging();

    public void CustomerReviews(Agency agency, User logedInUser)
    {
        Console.Clear();
        logger.LogMessage($"Agency {agency.Name} is viewing customer reviews.", logedInUser);

        Console.WriteLine("💬 Customer Reviews");
        Console.WriteLine("-------------------");

        var reviews = agencyRepository.ViewAgencyReviews(agency);

        if (reviews.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No reviews available for your agency yet.");
            Console.ResetColor();
            logger.LogMessage($"No reviews found for agency {agency.Name}.", logedInUser);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            Console.ResetColor();
        }
        else
        {
            logger.LogMessage($"Found {reviews.Count} reviews for agency {agency.Name}.", logedInUser);

            foreach (var review in reviews)
            {
                var user = agencyRepository.GetUserById(review.UserId);
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
        }
    }

    public void ManageTours(Agency agency, User logedInUser)
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("📂 Manage Tours");
        Console.WriteLine("----------------");
        Console.ResetColor();

        var agencyTours = agencyRepository.GetAgencyTours(agency.Id); // ✅ Use repository

        foreach (var tour in agencyTours)
        {
            if (tour.Status == Enums.Enums.TourStatus.Draft)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if(tour.Status == Enums.Enums.TourStatus.Published)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (tour.Status == Enums.Enums.TourStatus.Completed)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }
            else if (tour.Status == Enums.Enums.TourStatus.Full)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
            }
            else if (tour.Status == Enums.Enums.TourStatus.Cancelled)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.WriteLine($"[{tour.Id}] 📍 {tour.StartingPoint} - {tour.Destination} - {tour.Price} {tour.Currency} [{tour.Status}]");

            Console.ResetColor();
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

            logger.LogMessage($"Agency {agency.Name} entered invalid input while managing tours.", logedInUser);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Press any key to continue...");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
            return;
        }
        else if (tourId == 0)
        {
            logger.LogMessage($"Agency {agency.Name} exited tour management.", logedInUser);
            Console.Clear();
            return;
        }
        else
        {
            // ✅ Use repository instead of creating new context
            var selectedTour = agencyRepository.GetTourById(tourId);

            if (selectedTour == null)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("⚠️ No tour found with the given ID.");
                Console.ResetColor();
                Console.WriteLine();

                logger.LogMessage($"Agency {agency.Name} attempted to access a non-existent tour with ID {tourId}.", logedInUser);

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
                return;
            }
            else
            {
                var tourDetails = agencyRepository.GetTourById(tourId);
                logger.LogMessage(input + " tour selected for management by agency " + agency.Name, logedInUser);
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"🌍 {selectedTour.Name} details:");
                Console.WriteLine("----------------------");
                Console.ResetColor();

                Console.WriteLine($"📌 Starting Point: {tourDetails.StartingPoint}");
                Console.WriteLine($"📍 Destination: {tourDetails.Destination}");
                Console.WriteLine($"📆 Duration: {tourDetails.DurationDays} days, {tourDetails.DurationNights} nights");
                Console.WriteLine($"👥 Maximum Participant: {tourDetails.MaxParticipants}");
                Console.WriteLine($"👥 Current Participants: {tourDetails.CurrentParticipants}");
                Console.WriteLine($"📝 Description: {tourDetails.Description}");
                Console.WriteLine($"💲 Price: {tourDetails.Price} {tourDetails.Currency}");
                Console.WriteLine($"💰 Income from this tour: {tourDetails.Price * tourDetails.CurrentParticipants} {tourDetails.Currency}");
                Console.WriteLine();
                if (tourDetails.Status == Enums.Enums.TourStatus.Draft)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else if (tourDetails.Status == Enums.Enums.TourStatus.Published)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else if (tourDetails.Status == Enums.Enums.TourStatus.Completed)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                }
                else if (tourDetails.Status == Enums.Enums.TourStatus.Full)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                else if (tourDetails.Status == Enums.Enums.TourStatus.Cancelled)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                Console.WriteLine($"[Status : {tourDetails.Status}]");
                Console.ResetColor();
                if (tourDetails.Status == Enums.Enums.TourStatus.Cancelled)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"❌ Tour \"{tourDetails.Name}\" is rejected");
                    Console.WriteLine($"⚠️ Reject Reason : {tourDetails.RejectReason}");
                    Console.ResetColor();
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("🗑️ Do you want to delete the rejected tour?");
                    Console.ResetColor();

                    Console.WriteLine();
                    Console.WriteLine("1. ✅ Yes");
                    Console.WriteLine("2. ❌ No");
                    Console.WriteLine("0. 🛑 Exit");
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("Select an option: ");
                    Console.ResetColor();

                    var choice = Console.ReadKey();
                    switch(choice.KeyChar)
                    {
                        case '1':
                            var trackedAgency = agencyRepository.GetAgencyById(agency.Id);
                            trackedAgency.TotalToursCreated -= 1;
                            agencyRepository.UpdateAgencyTwo(trackedAgency);

                            logger.LogMessage($"Agency : {logedInUser.OwnedAgency.Name} deleted tour aplication", logedInUser);
                            agencyRepository.RemoveTour(tourDetails.Id);

                            Console.WriteLine();
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Your tour creation application has been successfully deleted!");
                            Console.WriteLine("You may submit a new application at any time.");
                            Console.ResetColor();
                            Console.WriteLine();

                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();
                            Console.Clear();
                            Console.ResetColor();
                            return;
                        case '2':
                            logger.LogMessage($"Agency : {logedInUser.OwnedAgency.Name} has not deleted tour aplication", logedInUser);
                            Console.Clear();
                            return;
                        case '3':
                            logger.LogMessage($"Returned to menu", logedInUser);
                            Console.Clear();
                            return;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("❌ Invalid option selected.");
                            Console.ResetColor();

                            logger.LogMessage($"Agency {agency.Name} selected invalid option in profile view.", logedInUser);

                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();
                            Console.Clear();
                            Console.ResetColor();
                            break;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine();
                    Console.WriteLine("----------------------");
                    Console.ResetColor();

                    Console.WriteLine("1. 🗑  Delete tour");
                    Console.WriteLine("0. ❌ Exit");

                    var choice = Console.ReadKey();

                    if (choice.KeyChar == '1')
                    {
                        Console.WriteLine();
                        Console.WriteLine();

                        var trackedAgency = agencyRepository.GetAgencyById(agency.Id);
                        var totalRefund = selectedTour.Price * selectedTour.CurrentParticipants;

                        trackedAgency.TotalToursCreated -= 1;
                        trackedAgency.TotalEarnings -= totalRefund;
                        trackedAgency.TotalBookings -= selectedTour.CurrentParticipants;
                        trackedAgency.Balance -= totalRefund;


                        var userRefunds = selectedTour.Bookings
                            .GroupBy(b => b.UserId)
                            .Select(g => new
                            {
                                UserId = g.Key,
                                TotalRefund = g.Sum(b => b.TotalPrice)
                            });

                        foreach (var refund in userRefunds)
                        {
                            var user = agencyRepository.GetUserById(refund.UserId);
                            if (user != null)
                            {

                                user.Balance += refund.TotalRefund;

                                if (logedInUser.Id == user.Id)
                                {
                                    logedInUser.Balance = user.Balance;
                                }

                                agencyRepository.UpdateUserTwo(user);

                            }
                        }

                        var firstUserAfter = agencyRepository.GetUserById(selectedTour.Bookings.First().UserId);

                        agencyRepository.UpdateAgencyTwo(trackedAgency);

                        agencyRepository.RemoveTour(selectedTour.Id);


                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("✅ Tour deleted successfully and users refunded.");
                        Console.ResetColor();

                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        Console.ResetColor();
                        logger.LogMessage($"Tour '{selectedTour.Name}' deleted by agency {agency.Name}. Refunded {selectedTour.Bookings.Count} users.", logedInUser);
                    }



                    else if (choice.KeyChar == '0')
                    {
                        logger.LogMessage($"Agency {agency.Name} exited tour management for tour ID {tourId}.", logedInUser);
                        Console.Clear();
                        return;
                    }
                }
            }
        }
    }

    public void RegisterNewTour(Agency agency, User logedInUser)
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

        if (agency.Balance < 50)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Insufficient funds in your agency balance to register a new tour.");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            logger.LogMessage($"Agency {agency.Name} attempted to register a new tour but had insufficient funds.", logedInUser);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            Console.ResetColor();
            return;
        }
        else
        {
            Console.Write("Enter Tour Name\n>> ");
            string tourName = Console.ReadLine() ?? "";

            Console.Write("Enter Starting Point\n>> ");
            string startingPoint = Console.ReadLine() ?? "";

            Console.Write("Enter Destination\n>> ");
            string destination = Console.ReadLine() ?? "";

            Console.Write("Enter Tour Description\n>> ");
            string description = Console.ReadLine() ?? "";

            Console.Write("Enter Maximum Participants\n>> ");
            int.TryParse(Console.ReadLine(), out int maxParticipants);

            Console.Write("Enter Start Date (yyyy-mm-dd)\n>> ");
            DateTime.TryParse(Console.ReadLine(), out DateTime startDate);

            Console.Write("Enter End Date (yyyy-mm-dd)\n>> ");
            DateTime.TryParse(Console.ReadLine(), out DateTime endDate);

            Console.Write("Enter Minimum Age\n>> ");
            int.TryParse(Console.ReadLine(), out int minAge);

            Console.Write("Enter What Tour Includes (comma-separated)\n>> ");
            string includesInput = Console.ReadLine() ?? "";
            List<string> includes = includesInput
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToList();

            Console.Write("Enter Price\n>> ");
            decimal.TryParse(Console.ReadLine(), out decimal price);

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
                AgencyId = agency.Id,
                CurrentParticipants = 0,
                Status = Enums.Enums.TourStatus.Draft,
            };

            var tourValidationResult = tourValidator.Validate(newTour);

            if (!tourValidationResult.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var error in tourValidationResult.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                logger.LogMessage($"Agency {agency.Name} attempted to register a new tour but validation failed: {string.Join("; ", tourValidationResult.Errors.Select(e => e.ErrorMessage))}", logedInUser);

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
                var registeredTour = agencyRepository.RegisterTour(newTour);

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ Tour named: **{registeredTour.Name}** registered successfully!");
                Console.ResetColor();

                // ❗ NEVER use the passed `agency` object
                var trackedAgency = agencyRepository.GetAgencyByOwnerId(logedInUser.Id);

                trackedAgency.Balance -= 50;
                trackedAgency.TotalToursCreated += 1;

                agencyRepository.UpdateAgencyTwo(trackedAgency);

                var fresh = agencyRepository.GetAgencyById(agency.Id);   

                logger.LogMessage($"{tourName} tour registered successfully by agency {trackedAgency.Name}", logedInUser);

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
                Console.ResetColor();
            }
        }
    }

    public void ViewAgnecyDetails(Agency agency, User logedInUser)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("🏢 Agency Profile");
        Console.WriteLine("------------------");
        Console.ResetColor();

        Console.WriteLine($"🆔 Agency Name: {agency.Name}");
        Console.WriteLine($"🪪 Owner: {agency.Owner.UserName}");
        Console.WriteLine($"🌍 Ageny country: {agency.Country}");
        Console.WriteLine($"🏙️ Agency city: {agency.City}");
        Console.WriteLine($"📬 Agency address: {agency.Address}");
        Console.WriteLine($"📃 Description: {agency.Description}");
        Console.WriteLine($"🧮 Total Tours Created: {agency.TotalToursCreated}");
        Console.WriteLine($"📅 Established On: {agency.CreatedAt.ToShortDateString()}");
        Console.WriteLine();
        Console.WriteLine("1. 📝 Change property");
        Console.WriteLine("0. ❌ Exit");
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("Select option: ");
        Console.ResetColor();

        var choice = Console.ReadKey().KeyChar;

        switch (choice)
        {
            case '1':
                logger.LogMessage("Selected change agency property", logedInUser);
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("🏢 Agency Profile");
                Console.WriteLine("------------------");
                Console.ResetColor();

                Console.WriteLine("1. 🆔 Change Agency Name");
                Console.WriteLine("2. 🌍 Change Agency Country");
                Console.WriteLine("3. 🏙️ Change Agency city");
                Console.WriteLine("4. 📬 Change Agency address");
                Console.WriteLine("5. 📃 Change Description");
                Console.WriteLine("0. ❌ Exit");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("Enter your choice: ");
                var agencyOwner = Console.ReadKey();

                switch (agencyOwner.KeyChar)
                {
                    case '1':
                        logger.LogMessage("Selected change agency name", logedInUser);
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.WriteLine("🏢 Change Agency Name");
                        Console.WriteLine("---------------------");
                        Console.ResetColor();

                        Console.WriteLine($"Agency name: {agency.Name}");
                        Console.WriteLine();

                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("Enter new agency name: ");   // FIXED TEXT
                        string inputNewName = Console.ReadLine();
                        Console.ResetColor();

                        if (agency.Name == inputNewName ||
                            string.IsNullOrWhiteSpace(inputNewName) ||
                            inputNewName.Length < 3)
                        {
                            logger.LogMessage("Changing agency name failed", logedInUser);

                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Cannot change the agency name. The new name must be at least 3 characters long, non-empty, and different from the current name.");
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
                            logger.LogMessage($"Changed agency name from \"{agency.Name}\" to \"{inputNewName}\"", logedInUser);

                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Agency name successfully changed from \"{agency.Name}\" to \"{inputNewName}\".");
                            Console.ResetColor();

                            agency.Name = inputNewName;
                            agencyRepository.UpdateAgencyTwo(agency);

                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Press any key to continue...");
                            Console.ResetColor();
                            Console.ReadKey();
                            Console.Clear();
                            return;
                        }

                    case '2':
                        logger.LogMessage("Selected change agency country", logedInUser);
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.WriteLine("🏢 Change Agency Country");
                        Console.WriteLine("------------------------");
                        Console.ResetColor();

                        Console.WriteLine($"Agency country: {agency.Country}");

                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("Enter new agnecy country: ");
                        string inputNewCountry = Console.ReadLine();
                        Console.ResetColor();

                        if (agency.Country == inputNewCountry || string.IsNullOrWhiteSpace(inputNewCountry) || inputNewCountry.Length < 3)
                        {
                            logger.LogMessage("Changing agency country failed", logedInUser);

                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Cannot change the agency country. The new country must be at least 3 characters long, non-empty, and different from the current country.");
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
                            logger.LogMessage($"Changed country name from \"{agency.Country}\" to \"{inputNewCountry}\"", logedInUser);
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Agency country successfully changed from \"{agency.Country}\" to \"{inputNewCountry}\".");
                            Console.ResetColor();

                            agency.Country = inputNewCountry;
                            agencyRepository.UpdateAgencyTwo(agency);

                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Press any key to continue...");
                            Console.ResetColor();
                            Console.ReadKey();
                            Console.Clear();
                            return;
                        }

                    case '3':
                        // UNCHANGED
                        logger.LogMessage("Selected change agency city", logedInUser);
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.WriteLine("🏢 Change Agency City");
                        Console.WriteLine("---------------------");
                        Console.ResetColor();

                        Console.WriteLine($"Agency country: {agency.City}");

                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("Enter new agnecy city: ");
                        string inputNewCity = Console.ReadLine();
                        Console.ResetColor();

                        if (agency.Country == inputNewCity || string.IsNullOrWhiteSpace(inputNewCity) || inputNewCity.Length < 3)
                        {
                            logger.LogMessage("Changing agency city failed", logedInUser);

                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Cannot change the agency city. The new city must be at least 3 characters long, non-empty, and different from the current city.");
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
                            logger.LogMessage($"Changed agency city from \"{agency.City}\" to \"{inputNewCity}\"", logedInUser);
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Agency city successfully changed from \"{agency.City}\" to \"{inputNewCity}\".");
                            Console.ResetColor();

                            agency.City = inputNewCity;
                            agencyRepository.UpdateAgencyTwo(agency);

                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Press any key to continue...");
                            Console.ResetColor();
                            Console.ReadKey();
                            Console.Clear();
                            return;
                        }

                    case '4':
                        logger.LogMessage("Selected change agency adress", logedInUser);
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.WriteLine("📬 Change Agency Address");
                        Console.WriteLine("-------------------------");
                        Console.ResetColor();
                        Console.WriteLine($"Agency adress: {agency.Address}");

                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("Enter new address: ");
                        string inputNewAdress = Console.ReadLine();
                        Console.ResetColor();

                        if (agency.Country == inputNewAdress || string.IsNullOrWhiteSpace(inputNewAdress) || inputNewAdress.Length < 3)
                        {
                            logger.LogMessage("Changing agency adress failed", logedInUser);

                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Cannot change the agency adress. The new adress must be at least 3 characters long, non-empty, and different from the current adress.");
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
                            logger.LogMessage($"Changed agency adress from \"{agency.Address}\" to \"{inputNewAdress}\"", logedInUser);
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Agency adress successfully changed from \"{agency.Address}\" to \"{inputNewAdress}\".");
                            Console.ResetColor();

                            agency.Address = inputNewAdress;
                            agencyRepository.UpdateAgencyTwo(agency);

                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Press any key to continue...");
                            Console.ResetColor();
                            Console.ReadKey();
                            Console.Clear();
                            return;
                        }

                    case '5':
                        logger.LogMessage("Selected change agency description", logedInUser);
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.WriteLine("📬 Change Agency Description");
                        Console.WriteLine("-------------------------");
                        Console.ResetColor();
                        Console.WriteLine($"Agency description : {agency.Description}");

                        Console.WriteLine();

                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("Enter new address: ");
                        Console.ResetColor();
                        string inputNewDescription = Console.ReadLine();

                        if (agency.Country == inputNewDescription || string.IsNullOrWhiteSpace(inputNewDescription) || inputNewDescription.Length < 10)
                        {
                            logger.LogMessage("Changing agency adress failed", logedInUser);

                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Cannot change the agency description. The new description must be at least 10 characters long, non-empty, and different from the current description.");
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
                            logger.LogMessage($"Changed agency description from \"{agency.Description}\" to \"{inputNewDescription}\"", logedInUser);
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Agency description successfully changed from \"{agency.Description}\" to \"{inputNewDescription}\".");
                            Console.ResetColor();

                            agency.Description = inputNewDescription;
                            agencyRepository.UpdateAgencyTwo(agency);

                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Press any key to continue...");
                            Console.ResetColor();
                            Console.ReadKey();
                            Console.Clear();
                            return;
                        }

                    case '0':
                        logger.LogMessage($"Agency {agency.Name} exited profile view.", logedInUser);
                        Console.Clear();
                        return;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("❌ Invalid option selected.");
                        Console.ResetColor();

                        logger.LogMessage($"Agency {agency.Name} selected invalid option in profile view.", logedInUser);

                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        Console.ResetColor();
                        break;
                }
                break;

            case '0':
                Console.Clear();
                return;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Invalid choice. Please try again.");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ResetColor();

                Console.ReadKey();
                Console.Clear();
                return;

                break;
        }
    }


    public void ViewFinancalOverview(Agency agency, User logedInUser)
    {
        Console.Clear();
        var trackedAgency = agencyRepository.GetAgencyByOwnerId(logedInUser.Id);
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("💰 Financial Overview");
        Console.WriteLine("-----------------------");
        Console.ResetColor();

        Console.WriteLine($"Balance: {trackedAgency.Balance} GEL");
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
                logger.LogMessage($"Agency {agency.Name} is attempting to withdraw funds.", logedInUser);
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("💸 Withdraw");
                Console.WriteLine("------------");
                Console.ResetColor();

                Console.Write("Enter amount to withdraw into personal account:\n>> ");
                string input = Console.ReadLine();

                if (!decimal.TryParse(input, out decimal withdrawAmount))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("⚠️ Invalid amount. Please enter a valid number.");

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.ResetColor();

                    logger.LogMessage($"Agency {agency.Name} entered invalid withdrawal amount: {input}.", logedInUser);

                    Console.ResetColor();
                    return;
                }
                else if (agency.Balance < withdrawAmount)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ Insufficient funds in your agency balance to make this withdrawal.");
                    Console.ResetColor();

                    logger.LogMessage("withdrawal failed due to insufficient funds in agency " + agency.Name, logedInUser);

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.ResetColor();
                }
                else
                {
                    var user = agencyRepository.GetAgencyOwnerById(logedInUser.Id);
                    var agencyy = agencyRepository.GetAgencyById(agency.Id);
                    user.Balance += withdrawAmount;
                    agencyy.Balance -= withdrawAmount;
                    logedInUser.Balance = user.Balance;
                    agency.Balance = agencyy.Balance;

                    agencyRepository.UpdateUserTwo(user);
                    agencyRepository.UpdateAgencyTwo(agencyy);

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"✅ Withdrew {withdrawAmount} to your personal account successfully!");
                    Console.ResetColor();

                    logger.LogMessage(withdrawAmount + " withdrawn successfully by agency " + agency.Name, logedInUser);

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.ResetColor();
                }
                break;

            case '0':
                logger.LogMessage($"Agency {agency.Name} exited financial overview.", logedInUser);
                Console.Clear();
                break;
        }
    }

    public void ViewPerformacneStatistics(Agency agency, User logedInUser)
    {
        var updatedAgency = agencyRepository.GetAgencyById(agency.Id);
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("📊 Performance Statistics");
        Console.WriteLine("---------------------------");
        Console.ResetColor();

        Console.WriteLine($"🗺️ Total Tours Created: {updatedAgency.TotalToursCreated}");
        Console.WriteLine($"📔 Total Bookings: {updatedAgency.TotalBookings}");
        Console.WriteLine($"💵 Total Earnings: {updatedAgency.TotalEarnings} GEL");
        Console.WriteLine();
        Console.WriteLine($"💰 Current Balance: {updatedAgency.Balance} GEL");
        Console.WriteLine();
        Console.WriteLine($"0. ❌ Exit");

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("Press zero to exit: ");
        Console.ResetColor();

        if (Console.ReadKey().KeyChar == '0')
        {
            logger.LogMessage($"Agency {updatedAgency.Name} exited performance statistics.", logedInUser);
            Console.Clear();
        }
    }

    public void GenerateAgencyInvoicePdf(Agency agency, User loggedInUser)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("📄 Generate Agency Invoice");
        Console.WriteLine("---------------------------");
        Console.ResetColor();

        Console.WriteLine("Generating invoice with all-time financial data...");
        Console.WriteLine();

        var invoiceService = new InvoiceService();
        invoiceService.GenerateAgencyInvoice(agency);

        var logger = new Logging();
        logger.LogMessage($"Invoice generated for agency {agency.Name}", loggedInUser);
    }
}