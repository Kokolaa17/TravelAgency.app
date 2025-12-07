using LuxTravel.app.Helpers;
using LuxTravel.app.Models;
using LuxTravel.app.Repositories;
using LuxTravel.app.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxTravel.app.Services;

public class AdminService : IAdminService
{
    AdminRepository adminRepository = new AdminRepository();
    Logging logger = new Logging();
    public void ViewAllAgencies()
    {
        var agencies = adminRepository.GetAgencyList();

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("🌐  All Agencies");
        Console.WriteLine("-------------------");
        Console.ResetColor();
        foreach (var agency in agencies)
        {
            Console.WriteLine($"[{agency.Id}] 🏢 Agency: {agency.Name} | 🌍 Location: {agency.Country}, {agency.City} [{agency.CreatedAt.ToShortDateString()}]");
        }

        Console.WriteLine();
        Console.WriteLine("0. ❌ Exit");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("View details by Id or select zero to exit: ");
        var input = Console.ReadLine();
        Console.ResetColor();

        if (input == "0")
        {
            logger.LogMessage("exited agency view.", new User { UserName = "Admin" });
            Console.Clear();
            return;
        }
        else
        {
            int agencyId;
            if (int.TryParse(input, out agencyId))
            {
                var selectedAgency = adminRepository.GetAgencyById(agencyId);
                if (selectedAgency != null)
                {
                    logger.LogMessage("viewing agency: " + selectedAgency.Name, new User { UserName = "Admin" });
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("🏢  Agency Profile");
                    Console.WriteLine("------------------");
                    Console.ResetColor();

                    Console.WriteLine($"🆔 Agency Name: {selectedAgency.Name}");
                    Console.WriteLine($"🪪 Owner: {selectedAgency.Owner.UserName}");
                    Console.WriteLine($"🌍 Ageny country: {selectedAgency.Country}");
                    Console.WriteLine($"🏙️ Agency city: {selectedAgency.City}");
                    Console.WriteLine($"📬 Agency address: {selectedAgency.Address}");
                    Console.WriteLine($"📝 Description: {selectedAgency.Description}");
                    Console.WriteLine($"🧮 Total Tours Created: {selectedAgency.TotalToursCreated}");
                    Console.WriteLine($"📅 Established On: {selectedAgency.CreatedAt.ToShortDateString()}");
                    Console.WriteLine();

                    Console.WriteLine("0. ❌ Exit");

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("Press zero to exit: ");
                    var choice = Console.ReadKey();
                    Console.ResetColor();

                    switch (choice.KeyChar) 
                    {
                        case '0':
                            Console.Clear();
                            return;
                            break;
                        default:
                            Console.Clear();
                            return;
                            break;
                    }
                    
                }
                else
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("⚠️ Invalid Agency Id. Please try again.");
                    Console.ResetColor();
                    Console.WriteLine();

                    logger.LogMessage(input + " is invalid Agency Id in agency view.", new User { UserName = "Admin" });

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("⚠️ Invalid input. Please enter a valid Agency Id.");
                Console.ResetColor();
                Console.WriteLine();

                logger.LogMessage(input + " is invalid input in agency view.", new User { UserName = "Admin" });

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
                Console.ResetColor();
            }
        }
    }

    public void ViewAllUsers()
    {
        List<User> users = adminRepository.GetUsersList();
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("🌐 👥 All Users");
        Console.WriteLine("----------------");
        Console.ResetColor();

        foreach (var user in users) 
        {
            Console.WriteLine($"[{user.Id}] {user.UserName}, [{user.CreatedAt.ToShortDateString()}]");
        }

        Console.WriteLine();
        Console.WriteLine("0. ❌ Exit");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("View details by Id or select zero to exit: ");
        var input = Console.ReadLine();
        Console.ResetColor();

        if (input == "0")
        {
            logger.LogMessage("exited view users.", new User { UserName = "Admin" });
            Console.Clear();
            return;
        }
        else
        {
            int userId;
            if (int.TryParse(input, out userId))
            {
                var selectedUser = adminRepository.GetUserById(userId);
                if (selectedUser != null)
                {
                    logger.LogMessage("viewing user: " + selectedUser.UserName, new User { UserName = "Admin" });
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("👤 User Profile");
                    Console.WriteLine("---------------");
                    Console.ResetColor();

                    Console.WriteLine($"🧑 First Name: {selectedUser.FirstName}");
                    Console.WriteLine($"🧑 Last Name: {selectedUser.LastName}");
                    Console.WriteLine($"👤 Username: {selectedUser.UserName}");
                    Console.WriteLine($"📧 Email: {selectedUser.Email}");
                    Console.WriteLine($"🏷️ Role: {selectedUser.Role}");
                    Console.WriteLine($"💰 Balance: {selectedUser.Balance}");
                    Console.WriteLine();
                    Console.WriteLine($"📅 Joined On: {selectedUser.CreatedAt.ToShortDateString()}");
                    Console.WriteLine();

                    Console.WriteLine("0. ❌ Exit");

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("Press zero to exit: ");
                    var choice = Console.ReadKey();
                    Console.ResetColor();

                    switch (choice.KeyChar)
                    {
                        case '0':
                            Console.Clear();
                            return;
                            break;
                        default:
                            Console.Clear();
                            return;
                            break;
                    }

                }
                else
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("⚠️ Invalid User Id. Please try again.");
                    Console.ResetColor();
                    Console.WriteLine();

                    logger.LogMessage(input + " is invalid User Id in user view.", new User { UserName = "Admin" });

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("⚠️ Invalid input. Please enter a valid user Id.");
                Console.ResetColor();
                Console.WriteLine();

                logger.LogMessage(input + " is invalid input in user view.", new User { UserName = "Admin" });

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
                Console.ResetColor();
            }
        }


    }

    public void ViewPendingAgencies()
    {
        var agencies = adminRepository.GetPendingAgencies();

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("🌐 Pending Agencies");
        Console.WriteLine("-------------------");
        Console.ResetColor();
        foreach (var agency in agencies)
        {
            Console.WriteLine($"[{agency.Id}] 🏢 Agency: {agency.Name} | 🌍 Location: {agency.Country}, {agency.City} | Status : {agency.Status}");
        }

        Console.WriteLine();
        Console.WriteLine("0. ❌ Exit");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("View details by Id or select zero to exit: ");
        var input = Console.ReadLine();
        Console.ResetColor();

        if (input == "0")
        {
            logger.LogMessage("exited agency view.", new User { UserName = "Admin" });
            Console.Clear();
            return;
        }
        else
        {
            int agencyId;
            if (int.TryParse(input, out agencyId))
            {
                var selectedAgency = adminRepository.GetAgencyById(agencyId);
                if (selectedAgency != null)
                {
                    logger.LogMessage("viewing agency: " + selectedAgency.Name, new User { UserName = "Admin" });
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("🏢  Agency Profile");
                    Console.WriteLine("------------------");
                    Console.ResetColor();

                    Console.WriteLine($"🆔 Agency Name: {selectedAgency.Name}");
                    Console.WriteLine($"🪪 Owner: {selectedAgency.Owner.UserName}");
                    Console.WriteLine($"🌍 Ageny country: {selectedAgency.Country}");
                    Console.WriteLine($"🏙️ Agency city: {selectedAgency.City}");
                    Console.WriteLine($"📬 Agency address: {selectedAgency.Address}");
                    Console.WriteLine($"📝 Description: {selectedAgency.Description}");
                    Console.WriteLine($"🧮 Total Tours Created: {selectedAgency.TotalToursCreated}");
                    Console.WriteLine($"📅 Established On: {selectedAgency.CreatedAt.ToShortDateString()}");
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[Status] {selectedAgency.Status}");
                    Console.ResetColor();
                    Console.WriteLine();

                    Console.WriteLine("1. ✅ Approve Agency");
                    Console.WriteLine("2. 🚫 Reject Agency");
                    Console.WriteLine("0. ❌ Exit");

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("Select a option: ");
                    var choice = Console.ReadKey();
                    Console.ResetColor();

                    switch (choice.KeyChar)
                    {
                        case '1':
                            Console.WriteLine();
                            selectedAgency.Status = Enums.Enums.AgencyStatus.Approved;
                            adminRepository.UpdateAgency( selectedAgency );
                            logger.LogMessage($"approved agency: {selectedAgency}", new User { UserName = "Admin" });

                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"✔️ Agency \"{selectedAgency.Name}\" has been approved successfully!");
                            Console.ResetColor();
                            Console.WriteLine();

                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Press any key to continue...");
                            Console.ResetColor();
                            Console.ReadKey();
                            Console.Clear();
                            return;

                            break;
                        case '2':
                            logger.LogMessage($"Rejected agency: {selectedAgency}", new User { UserName = "Admin" });

                            Console.WriteLine();
                            Console.WriteLine();
                            Console.Write("Type reason: ");
                            string rejectReason = "";

                            do
                            {
                                Console.Write("Type the rejection reason:  ");
                                rejectReason = Console.ReadLine()?.Trim(); // Trim removes leading/trailing spaces
                            } while (string.IsNullOrEmpty(rejectReason));

                            Console.WriteLine($"Reason recorded: {rejectReason}");

                            selectedAgency.RejectReason = rejectReason;
                            selectedAgency.Status = Enums.Enums.AgencyStatus.Rejected;
                            adminRepository.UpdateAgency(selectedAgency);

                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"❌ Agency \"{selectedAgency.Name}\" has been rejected.");
                            Console.ResetColor();
                            Console.WriteLine();

                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Press any key to continue...");
                            Console.ResetColor();
                            Console.ReadKey();
                            Console.Clear();
                            return;
                            break;
                        case '0':
                            logger.LogMessage($"Returned to menu", new User { UserName = "Admin" });

                            Console.Clear();
                            return;
                            break;
                        default:
                            Console.Clear();
                            return;
                            break;
                    }

                }
                else
                {

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("⚠️ Invalid Agency Id. Please try again.");
                    Console.ResetColor();
                    Console.WriteLine();

                    logger.LogMessage(input + " is invalid Agency Id in agency view.", new User { UserName = "Admin" });

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("⚠️ Invalid input. Please enter a valid Agency Id.");
                Console.ResetColor();
                Console.WriteLine();

                logger.LogMessage(input + " is invalid input in agency view.", new User { UserName = "Admin" });

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
                Console.ResetColor();
            }
        }


    }

    public void ViewPendingTours()
    {
        var tours = adminRepository.GetPendingTours();

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("🧭 Pending Tours");
        Console.WriteLine("-------------------");
        Console.ResetColor();

        foreach (var tour in tours)
        {
            Console.WriteLine($"[{tour.Id}] 🧳 Tour: {tour.Name} | 📍 {tour.StartingPoint} ➝ {tour.Destination} | Status: {tour.Status} [{tour.CreatedAt.ToShortDateString}]");
        }

        Console.WriteLine();
        Console.WriteLine("0. ❌ Exit");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("View details by Id or select zero to exit: ");
        var input = Console.ReadLine();
        Console.ResetColor();

        if (input == "0")
        {
            logger.LogMessage("exited tour view.", new User { UserName = "Admin" });
            Console.Clear();
            return;
        }
        else
        {
            int tourId;
            if (int.TryParse(input, out tourId))
            {
                var selectedTour = adminRepository.GetTourById(tourId);
                if (selectedTour != null)
                {
                    logger.LogMessage("viewing tour: " + selectedTour.Name, new User { UserName = "Admin" });

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("🧳  Tour Profile");
                    Console.WriteLine("------------------");
                    Console.ResetColor();

                    Console.WriteLine($"🆔 Tour ID: {selectedTour.Id}");
                    Console.WriteLine($"📛 Name: {selectedTour.Name}");
                    Console.WriteLine($"📍 Starting Point: {selectedTour.StartingPoint}");
                    Console.WriteLine($"🎯 Destination: {selectedTour.Destination}");
                    Console.WriteLine($"📝 Description: {selectedTour.Description}");
                    Console.WriteLine($"🧑‍🤝‍🧑 Max Participants: {selectedTour.MaxParticipants}");
                    Console.WriteLine($"💲 Price: {selectedTour.Price}");
                    Console.WriteLine($"🕒 Duration: {selectedTour.DurationDays} days, {selectedTour.DurationNights} nights");
                    Console.WriteLine($"🏢 Agency: {selectedTour.Agency?.Name}");
                    Console.WriteLine($"📅 Created On: {selectedTour.CreatedAt.ToShortDateString()}");

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[Status] {selectedTour.Status}");
                    Console.ResetColor();
                    Console.WriteLine();

                    Console.WriteLine("1. ✅ Approve Tour");
                    Console.WriteLine("2. 🚫 Reject Tour");
                    Console.WriteLine("0. ❌ Exit");

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("Select an option: ");
                    var choice = Console.ReadKey();
                    Console.ResetColor();

                    switch (choice.KeyChar)
                    {
                        case '1':
                            Console.WriteLine();
                            selectedTour.Status = Enums.Enums.TourStatus.Published;
                            adminRepository.UpdateTour(selectedTour);
                            logger.LogMessage($"approved tour: {selectedTour.Name}", new User { UserName = "Admin" });

                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"✔️ Tour \"{selectedTour.Name}\" has been approved successfully!");
                            Console.ResetColor();
                            Console.WriteLine();

                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Press any key to continue...");
                            Console.ResetColor();
                            Console.ReadKey();
                            Console.Clear();
                            return;

                        case '2':
                            logger.LogMessage($"rejected tour: {selectedTour.Name}", new User { UserName = "Admin" });

                            Console.WriteLine();
                            Console.Write("Type the rejection reason: ");
                            string rejectReason = "";

                            do
                            {
                                Console.Write("Reason: ");
                                rejectReason = Console.ReadLine()?.Trim();
                            }
                            while (string.IsNullOrEmpty(rejectReason));

                            selectedTour.RejectReason = rejectReason;
                            selectedTour.Status = Enums.Enums.TourStatus.Cancelled;
                            adminRepository.UpdateTour(selectedTour);

                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"❌ Tour \"{selectedTour.Name}\" has been rejected.");
                            Console.ResetColor();
                            Console.WriteLine();

                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Press any key to continue...");
                            Console.ResetColor();
                            Console.ReadKey();
                            Console.Clear();
                            return;

                        case '0':
                            logger.LogMessage("Returned to menu", new User { UserName = "Admin" });
                            Console.Clear();
                            return;

                        default:
                            Console.Clear();
                            return;
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("⚠️ Invalid Tour Id. Please try again.");
                    Console.ResetColor();
                    Console.WriteLine();

                    logger.LogMessage(input + " is invalid Tour Id in tour view.", new User { UserName = "Admin" });

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("⚠️ Invalid input. Please enter a valid Tour Id.");
                Console.ResetColor();
                Console.WriteLine();

                logger.LogMessage(input + " is invalid input in tour view.", new User { UserName = "Admin" });

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

    }

    public void ViewAllTours()
    {
        var tours = adminRepository.GetTourList();

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("🗺️  All Tours");
        Console.WriteLine("-------------------");
        Console.ResetColor();
        foreach (var tour in tours)
        {
            Console.WriteLine($"[{tour.Id}] 🎫 Tour: {tour.Name} | 🏢 Agency: {tour.Agency.Name} [{tour.CreatedAt.ToShortDateString()}]");
        }

        Console.WriteLine();
        Console.WriteLine("0. ❌ Exit");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("View details by Id or select zero to exit: ");
        var input = Console.ReadLine();
        Console.ResetColor();

        if (input == "0")
        {
            logger.LogMessage("exited tour view.", new User { UserName = "Admin" });
            Console.Clear();
            return;
        }
        else
        {
            int tourId;
            if (int.TryParse(input, out tourId))
            {
                var selectedTour = adminRepository.GetTourById(tourId);
                if (selectedTour != null)
                {
                    logger.LogMessage("viewing tour: " + selectedTour.Name, new User { UserName = "Admin" });
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("🗺️  Tour Profile");
                    Console.WriteLine("------------------");
                    Console.ResetColor();

                    Console.WriteLine($"📍 Destination: {selectedTour.Destination}");
                    Console.WriteLine($"⏱️ Duration: {selectedTour.DurationDays} days, {selectedTour.DurationNights} nights");
                    Console.WriteLine($"🎟️ Available Spots: {selectedTour.AvailableSpots}");
                    Console.WriteLine($"💰 Price: {selectedTour.Price} {selectedTour.Currency}");
                    Console.WriteLine($"🏢 Agency: {selectedTour.Agency.Name}");
                    Console.WriteLine($"📝 Description: {selectedTour.Description}");
                    Console.WriteLine($"👥 Max Participants: {selectedTour.MaxParticipants}");
                    Console.WriteLine();
                    Console.WriteLine($"📅 Created On: {selectedTour.CreatedAt.ToShortDateString()}");
                    Console.WriteLine();
                    Console.WriteLine();

                    Console.WriteLine("0. ❌ Exit");

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("Press zero to exit: ");
                    var choice = Console.ReadKey();
                    Console.ResetColor();

                    switch (choice.KeyChar)
                    {
                        case '0':
                            Console.Clear();
                            return;
                            break;
                        default:
                            Console.Clear();
                            return;
                            break;
                    }

                }
                else
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("⚠️ Invalid Tour Id. Please try again.");
                    Console.ResetColor();
                    Console.WriteLine();

                    logger.LogMessage(input + " is invalid Tour Id in tour view.", new User { UserName = "Admin" });

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("⚠️ Invalid input. Please enter a valid Tour Id.");
                Console.ResetColor();
                Console.WriteLine();

                logger.LogMessage(input + " is invalid input in tour view.", new User { UserName = "Admin" });

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
                Console.ResetColor();
            }
        }
    }
}
