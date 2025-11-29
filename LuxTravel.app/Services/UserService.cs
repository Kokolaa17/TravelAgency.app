using LuxTravel.app.Data;
using LuxTravel.app.Helpers;
using LuxTravel.app.Models;
using LuxTravel.app.Repositories;
using LuxTravel.app.Repositories.Interfaces;
using LuxTravel.app.Services.Interfaces;
using LuxTravel.app.Validators;

namespace LuxTravel.app.Services;

internal class UserService : IUserService
{
    UserRepository UserRepository = new UserRepository();
    public User UserLogIn()
    {
        User userLoggedIn = null;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("👤 User Login");
        Console.WriteLine("-------------");
        Console.ResetColor();

        Console.WriteLine("Enter Email Or User Name: ");
        Console.Write(">> ");
        string emailOrUsername = Console.ReadLine();

        Console.WriteLine("Enter Password: ");
        Console.Write(">> ");
        string password = Console.ReadLine();

        var validator = new UserValidator();

        var logedInUser = UserRepository.UserLogIn(emailOrUsername, password);

        if(logedInUser != null)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Welcome {logedInUser.UserName}"!);
            Console.ResetColor();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            Console.ResetColor();

            return logedInUser;
        }
        else
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No accounts found!");
            Console.ResetColor();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            Console.ResetColor();
            return userLoggedIn = null;
        }
    }

    public void UserRegistration()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("👤 User Register");
        Console.WriteLine("----------------");
        Console.ResetColor();

        Console.WriteLine("Enter First Name: ");
        Console.Write(">> ");
        string firstName = Console.ReadLine();

        Console.WriteLine("Enter Last Name: ");
        Console.Write(">> ");
        string lastName = Console.ReadLine();

        Console.WriteLine("Enter User Name: ");
        Console.Write(">> ");
        string userName = Console.ReadLine();

        Console.WriteLine("Enter Email: ");
        Console.Write(">> ");
        string registrationEmail = Console.ReadLine();

        Console.WriteLine("Enter Password: ");
        Console.Write(">> ");
        string registrationPassword = Console.ReadLine();
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registrationPassword);

        User newUser = new User
        {
            FirstName = firstName,
            LastName = lastName,
            UserName = userName,
            Email = registrationEmail,
            Password = hashedPassword
        };

        var userValidator = new UserValidator();
        var userValidationResult = userValidator.Validate(newUser);

        if(!userValidationResult.IsValid)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var error in userValidationResult.Errors)
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
           UserRepository.UserRegistration(newUser);
        }
    }

    public void ManageAgency(User logedInUser)
    {
        AgencyService agencyService = new AgencyService();
        var agency = UserRepository.GetAgencyByOwnerId(logedInUser.Id);

        Console.Clear();

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("🌐 Manage Agency");
        Console.WriteLine("----------------");
        Console.ResetColor();

        Console.WriteLine("1. 📝  Register New Tour");
        Console.WriteLine("2. 📂  Manage Tours");
        Console.WriteLine("3. 💬  Customer Reviews");
        Console.WriteLine("4. 💰  Financial Overview");
        Console.WriteLine("5. 📊  Performance Statistics");
        Console.WriteLine("6. 🏢  Agency Profile");
        Console.WriteLine("7. ❌  Exit System");
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("Select an option: ");
        Console.ResetColor();

        var agencyOwner = Console.ReadKey();
        switch (agencyOwner.KeyChar)
        {
            case '1':
                agencyService.RegisterNewTour(agency);
                break;
            case '2':
                agencyService.ManageTours(agency);
                break;
            case '3':
                agencyService.CustomerReviews(agency);
                break;
            case '4':
                agencyService.ViewFinancalOverview(agency);
                break;
            case '5':
                agencyService.ViewPerformacneStatistics(agency);
                break;
            case '6':
                agencyService.ViewAgnecyDetails(agency);
                break;
            case '7':
                Console.Clear();
            return;
            default:
                Console.WriteLine();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("⚠️ Invalid input. Please enter a valid option.");
                Console.ResetColor();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
                Console.ResetColor();
                break;
                    
        }
    }

    public void RegisterAgency(User logedInUser)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("Creating Agency");
        Console.WriteLine("----------------");
        Console.ResetColor();

        Console.WriteLine("Enter Agency Name: ");
        Console.Write(">> ");
        string agencyName = Console.ReadLine();

        Console.WriteLine("Enter Agency Country: ");
        Console.Write(">> ");
        string agencyCountry = Console.ReadLine();

        Console.WriteLine("Enter Agency City: ");
        Console.Write(">> ");
        string agencyCity = Console.ReadLine();

        Console.WriteLine("Enter Agency Adress: ");
        Console.Write(">> ");
        string agencyAdress = Console.ReadLine();

        Console.WriteLine("Enter Agency Description: ");
        Console.Write(">> ");
        string agencyDescription = Console.ReadLine();

        Agency newAgency = new Agency
        {
            Name = agencyName,
            Country = agencyCountry,
            City = agencyCity,
            Address = agencyAdress,
            Description = agencyDescription,
            OwnerId = logedInUser.Id,
            Email = logedInUser.Email,
            TotalBookings = 0,
            TotalEarnings = 0,
            TotalToursCreated = 0,
            Balance = 0m,
        };

        var agencyValidator = new AgencyValidator();
        var agencyValidationResult = agencyValidator.Validate(newAgency);

        if (!agencyValidationResult.IsValid)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var error in agencyValidationResult.Errors)
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
            Random VerificationCode = new Random();
            int code = VerificationCode.Next(1000, 9999);
            EmailSender emailSender = new EmailSender();
            emailSender.SendVerificationCode(logedInUser.Email, "Verification Code", $"<!DOCTYPE html><html lang=\"en\"><head>    <meta charset=\"UTF-8\">    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">    <title>LuxTravel - Code Generator</title></head><body style=\"margin: 0; padding: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background: linear-gradient(135deg, #1e3c72 0%, #2a5298 100%); min-height: 100vh; display: flex; justify-content: center; align-items: center;\">        <div style=\"background: white; border-radius: 20px; box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3); padding: 50px; max-width: 500px; width: 90%; text-align: center;\">                <!-- Logo/Header -->        <div style=\"margin-bottom: 30px;\">            <h1 style=\"color: #1e3c72; margin: 0; font-size: 42px; font-weight: bold; letter-spacing: 1px;\">                ✈️ LuxTravel            </h1>            <p style=\"color: #666; margin: 10px 0 0 0; font-size: 16px;\">Your Journey, Our Priority</p>        </div>        <!-- Divider -->        <div style=\"height: 2px; background: linear-gradient(to right, transparent, #2a5298, transparent); margin: 30px 0;\"></div>        <!-- Code Generator Section -->        <div style=\"margin: 30px 0;\">            <h2 style=\"color: #1e3c72; font-size: 24px; margin-bottom: 20px;\">Email Verification Code</h2>                        <!-- Display Code -->            <div style=\"background: linear-gradient(135deg, #1e3c72 0%, #2a5298 100%); padding: 30px; border-radius: 15px; margin: 25px 0; box-shadow: 0 10px 25px rgba(30, 60, 114, 0.3);\">                <p style=\"color: rgba(255, 255, 255, 0.8); margin: 0 0 10px 0; font-size: 14px; text-transform: uppercase; letter-spacing: 2px;\">Your Verification Code</p>                <p style=\"color: white; font-size: 56px; font-weight: bold; letter-spacing: 8px; font-family: 'Courier New', monospace; text-shadow: 0 2px 10px rgba(0, 0, 0, 0.3); margin: 0;\">                    {code}                </p>            </div>        </div>        <!-- Info Text -->        <div style=\"margin-top: 30px; padding-top: 20px; border-top: 1px solid #e0e0e0;\">            <p style=\"color: #888; font-size: 13px; line-height: 1.6; margin: 0;\">                Enter the 4-digit verification code sent to your email address to confirm your email.            </p>        </div>    </div></body></html>");

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Verification code has been sent to your email.");
            Console.ResetColor();
            Console.WriteLine("Enter 4-digit code below to proceed:");

            Console.Write(">> ");
            int confirmationKey = int.Parse(Console.ReadLine());

            if (confirmationKey == code)
            {
                var registeredAgency = UserRepository.RegisterAgency(newAgency);

                using (var dataContext = new DataContext())
                {
                    logedInUser.AgencyId = registeredAgency.Id;
                    logedInUser.OwnedAgency = registeredAgency;
                    logedInUser.Role = LuxTravel.app.Enums.Enums.userRole.AgencyOwner;

                    dataContext.Users.Update(logedInUser);
                    dataContext.SaveChanges();
                }

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Agency {registeredAgency.Name}, registered successfuly! Press any key to continue...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Incorrect verification code. Press any key to go back to menu...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    public void ManageBalance(User logedInUser)
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("💰  Manage Balance");
        Console.WriteLine("-----------------------");
        Console.ResetColor();
        Console.WriteLine($"Balance: {logedInUser.Balance} GEL");
        Console.WriteLine();
        Console.WriteLine("1.💳 Deposit");
        Console.WriteLine("2.💸 Withdraw");
        Console.WriteLine("0.❌ Exit");

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("Select an option: ");
        Console.ResetColor();
        var choice = Console.ReadKey();

        // for updates 
        AgencyRepository agencyRepository = new AgencyRepository();

        switch (choice.KeyChar)
        {
            case '1':
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("💳  Deposit");
                Console.WriteLine("------------");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("Enter amount to deposit into your account: ");
                string input = Console.ReadLine();
                decimal depositAmount;

                if (!decimal.TryParse(input, out depositAmount))
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("⚠️ Invalid amount. Please enter a valid number.");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.ResetColor();

                    return; 
                }
                Console.ResetColor();
                Console.WriteLine();

                logedInUser.Balance += depositAmount;

                agencyRepository.UpdateUser(logedInUser);
                agencyRepository.UpdateDatabase();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ Deposited {depositAmount} to your account successfully!");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
                Console.ResetColor();
                break;
            case '2':
                if (logedInUser.AgencyId == null)
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ You do not own an agency to withdraw funds from.");
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
                    Console.Clear();

                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("💸 Withdraw");
                    Console.WriteLine("------------");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("Enter amount to withdraw into your agency account: ");
                    string withDrawInput = Console.ReadLine();
                    decimal withDrawAmount;
                    Console.ResetColor();

                    if (!decimal.TryParse(withDrawInput, out withDrawAmount))
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("⚠️ Invalid amount. Please enter a valid number.");
                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        Console.ResetColor();

                        return;
                    }
                    else
                    {
                        withDrawAmount = decimal.Parse(withDrawInput);
                    }

                    if (logedInUser.Balance < withDrawAmount)
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
                        var ownedAgency = UserRepository.GetAgencyByOwnerId(logedInUser.Id);
                        logedInUser.Balance -= withDrawAmount;
                        ownedAgency.Balance += withDrawAmount;

                        agencyRepository.UpdateUser(logedInUser);
                        agencyRepository.UpdateDatabase();



                        Console.WriteLine();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"✅ Withdrew {withDrawAmount} to your personal account successfully!");
                        Console.ResetColor();



                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        Console.ResetColor();
                    }
                }
                break;
            case '0':
                Console.Clear();
                return;

                break;
            default :
                Console.WriteLine();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("⚠️ Invalid input. Please enter a valid option.");
                Console.ResetColor();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
                Console.ResetColor();
                break;
        }
    }

    public void ProfileInformation(User logedInUser)
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("👤  Profile Information");
        Console.WriteLine("------------------------");
        Console.ResetColor();

        Console.WriteLine($"👤 First Name: {logedInUser.FirstName}");
        Console.WriteLine($"👤 Last Name: {logedInUser.LastName}");
        Console.WriteLine($"👤 User Name: {logedInUser.UserName}");
        Console.WriteLine($"📧 Email: {logedInUser.Email}");
        Console.WriteLine($"💼 Role: {logedInUser.Role}");
        Console.WriteLine();

        Console.WriteLine("0. ❌ Exit");

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Press any key to return to the previous menu...");
        Console.ResetColor();
        Console.ReadKey();
        Console.Clear();
        Console.ResetColor();
    }

    public void ViewAllAgencies(User logedInUser)
    {
        var agencies = UserRepository.ViewAllAgencies();

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("🌐  All Agencies");
        Console.WriteLine("-------------------");
        Console.ResetColor();
        foreach (var agency in agencies)
        {
            Console.WriteLine($"[{agency.Id}] 🏢 Agency: {agency.Name} | 🌍 Location: {agency.Country}, {agency.City}");
        }

        Console.WriteLine();
        Console.WriteLine("0. ❌ Exit");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("Select agency by Id or select zero to exit: ");
        var input = Console.ReadLine();
        Console.ResetColor();

        if(input == "0")
        {
            Console.Clear();
            return;
        }
        else
        {
            int agencyId;
            if (int.TryParse(input, out agencyId))
            {
                var selectedAgency = UserRepository.GetAgencyByOwnerId(agencyId);
                if (selectedAgency != null)
                {
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

                    Console.WriteLine("1. Add Comment About Agency");
                    Console.WriteLine("0. ❌ Exit");

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("Select an option: ");
                    var choice = Console.ReadKey();
                    Console.ResetColor();

                    switch (choice.KeyChar)
                    {
                        case '1':

                            var existingReview = UserRepository.GetAllAgencyRevies(logedInUser, selectedAgency);
                            if(existingReview == null)
                            {
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                                Console.WriteLine("💬  Add Agency Review");
                                Console.WriteLine("----------------------");
                                Console.ResetColor();
                                Console.WriteLine("Enter your review about the agency: ");
                                Console.Write(">> ");
                                string comment = Console.ReadLine();
                                Console.Write("Enter your rating (1-5): ");
                                int rating = int.Parse(Console.ReadLine());

                                AgencyReview newReview = new AgencyReview
                                {
                                    UserId = logedInUser.Id,
                                    AgencyId = selectedAgency.Id,
                                    CreatedAt = DateTime.Now,
                                    Rating = rating,
                                    Comment = comment,
                                    Agency = selectedAgency,
                                    User = logedInUser,
                                };
                                UserRepository.AddAgencyReview(newReview);
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("✅ Review added successfully! Press any key to continue...");
                                Console.ResetColor();
                                Console.ReadKey();
                                Console.Clear();
                            }
                            else
                            {
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                                Console.WriteLine("💬  Add Agency Review");
                                Console.WriteLine("----------------------");
                                Console.ResetColor();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("You have already reviewed this agency.");
                                Console.ResetColor();
                                Console.WriteLine();
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
                        default:
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("⚠️ Invalid input. Please enter a valid option.");
                            Console.ResetColor();
                            Console.WriteLine();
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
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("⚠️ Invalid Agency Id. Please try again.");
                    Console.ResetColor();
                    Console.WriteLine();
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
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
                Console.ResetColor();
            }
        }

    }
}

