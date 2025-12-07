using LuxTravel.app.Data;
using LuxTravel.app.Helpers;
using LuxTravel.app.Models;
using LuxTravel.app.Repositories;
using LuxTravel.app.Repositories.Interfaces;
using LuxTravel.app.Services.Interfaces;
using LuxTravel.app.Validators;
using System.Net.Http.Headers;

namespace LuxTravel.app.Services;

public class UserService : IUserService
{
    UserRepository UserRepository = new UserRepository();
    Logging logger = new Logging();

    public static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key;

        while (true)
        {
            key = Console.ReadKey(intercept: true); 

            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                if (password.Length > 0)
                {
                    password = password[..^1];

                    Console.Write("\b \b");
                }
            }
            else if (!char.IsControl(key.KeyChar))
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        }

        return password;
    }

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
        string password = ReadPassword();


        var validator = new UserValidator();

        var logedInUser = UserRepository.UserLogIn(emailOrUsername, password);

        if(logedInUser != null)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Welcome {logedInUser.UserName}!");
            Console.ResetColor();

            logger.LogMessage("User logged in successfully.", logedInUser);

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

            logger.LogMessage("Failed login attempt.", new User { Email = emailOrUsername });

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            Console.ResetColor();
            return userLoggedIn = null;
        }
    }

    public Admin AdminLogIn()
    {
        User userLoggedIn = null;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("👤 Admin Login");
        Console.WriteLine("---------------");
        Console.ResetColor();

        Console.WriteLine("Enter Email Or Name: ");
        Console.Write(">> ");
        string emailOrUsername = Console.ReadLine();

        Console.WriteLine("Enter Password: ");
        Console.Write(">> ");
        string password = ReadPassword();

        var userLogedIn = UserRepository.AdminLogIn(emailOrUsername, password);

        if (userLogedIn != null)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Welcome {userLogedIn.UserName} !");
            Console.ResetColor();

            logger.LogMessage("Admin logged in successfully.", new User { UserName = "Admin"});

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            Console.ResetColor();

            return userLogedIn;
        }
        else
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No accounts found!");
            Console.ResetColor();

            logger.LogMessage("Failed login attempt.", new User { Email = emailOrUsername });

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            Console.ResetColor();
            return userLogedIn = null;
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
        string registrationPassword = ReadPassword();
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

            logger.LogMessage("Failed registration attempt.", newUser);
            return;
        }
        else
        {
           UserRepository.UserRegistration(newUser);
           logger.LogMessage("User registered successfully.", newUser);
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

        if(agency.Status == Enums.Enums.AgencyStatus.Approved)
        {
            Console.WriteLine("1. 📝  Register New Tour");
            Console.WriteLine("2. 📂  Manage Tours");
            Console.WriteLine("3. 💬  Customer Reviews");
            Console.WriteLine("4. 💰  Financial Overview");
            Console.WriteLine("5. 📊  Performance Statistics");
            Console.WriteLine("6. 🏢  Agency Profile");
            Console.WriteLine("7. 📄  Generate Agency Invoice");
            Console.WriteLine("0. ❌  Exit System");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Select an option: ");
            Console.ResetColor();

            var agencyOwner = Console.ReadKey();
            switch (agencyOwner.KeyChar)
            {
                case '1':
                    logger.LogMessage("selected register new tour.", logedInUser);
                    agencyService.RegisterNewTour(agency, logedInUser);
                    break;
                case '2':
                    logger.LogMessage("selected manage tours.", logedInUser);
                    agencyService.ManageTours(agency, logedInUser);
                    break;
                case '3':
                    logger.LogMessage("selected customer reviews.", logedInUser);
                    agencyService.CustomerReviews(agency, logedInUser);
                    break;
                case '4':
                    logger.LogMessage("selected financial overview.", logedInUser);
                    agencyService.ViewFinancalOverview(agency, logedInUser);
                    break;
                case '5':
                    logger.LogMessage("selected performance statistics.", logedInUser);
                    agencyService.ViewPerformacneStatistics(agency, logedInUser);
                    break;
                case '6':
                    logger.LogMessage("selected agency profile.", logedInUser);
                    agencyService.ViewAgnecyDetails(agency, logedInUser);
                    break;
                case '7':
                    logger.LogMessage("selected generate agency invoice.", logedInUser);
                    agencyService.GenerateAgencyInvoicePdf(agency, logedInUser);
                    break;
                case '0':
                    logger.LogMessage("exited agency management.", logedInUser);
                    Console.Clear();
                    return;
                default:
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("⚠️ Invalid input. Please enter a valid option.");
                    Console.ResetColor();
                    Console.WriteLine();
                    logger.LogMessage("invalid input in agency management.", logedInUser);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.ResetColor();
                    break;

            }
        }
        else if(agency.Status == Enums.Enums.AgencyStatus.Pending)
        {
            logger.LogMessage("Attempted to access agency menu while agency status is pending.", logedInUser);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⏳ Agency \"{agency.Name}\" is currently pending approval.");
            Console.WriteLine("⚠️ You cannot access agency management menus until your agency is approved.");
            Console.ResetColor();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            Console.ResetColor();
        }
        else if(agency.Status == Enums.Enums.AgencyStatus.Rejected)
        {
            logger.LogMessage($"Agency \"{agency.Name}\" has been rejected", new User { UserName = "Admin" });


            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ Agency \"{agency.Name}\" is rejected");
            Console.WriteLine($"⚠️ Reject Reason : {agency.RejectReason}");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("🔄 Do you want to retry creating the agency?");
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

            switch (choice.KeyChar) 
            {
                case '1':
                    logger.LogMessage("deleted agency creation aplication", logedInUser);
                    logedInUser.Role = Enums.Enums.userRole.Customer;
                    UserRepository.DeleteAgency(agency, logedInUser);
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Your agency creation application has been successfully deleted!");
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
                    break;
                case '2':
                    logger.LogMessage("has not deleted agency creation aplication", logedInUser);
                    Console.Clear();
                    return;
                case '3':
                    logger.LogMessage("returned to menu", logedInUser);
                    Console.Clear();
                    return;
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
            Status = Enums.Enums.AgencyStatus.Pending,
            RejectReason = null
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

            logger.LogMessage("Failed agency registration attempt.", logedInUser);
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
                logger.LogMessage(agencyName + " registered successfully.", logedInUser);
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                logger.LogMessage("Failed agency registration attempt due to incorrect verification code.", logedInUser);
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

        var trackedUser = UserRepository.getUserByEmail(logedInUser.Email);

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("💰  Manage Balance");
        Console.WriteLine("-----------------------");
        Console.ResetColor();
        Console.WriteLine($"Balance: {trackedUser.Balance} GEL");
        Console.WriteLine();
        Console.WriteLine("1.💳 Deposit");
        Console.WriteLine("2.💸 Withdraw");
        Console.WriteLine("0.❌ Exit");

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("Select an option: ");
        Console.ResetColor();
        var choice = Console.ReadKey();

        switch (choice.KeyChar)
        {
            case '1':
                Console.Clear();

                logger.LogMessage("Seleceted deposit", logedInUser);
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

                    logger.LogMessage("Invalid deposit amount entered.", logedInUser);

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.ResetColor();

                    return; 
                }

                Console.ResetColor();
                Console.WriteLine();

                trackedUser.Balance += depositAmount;

                UserRepository.UpdateUser(trackedUser);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ Deposited {depositAmount} to your account successfully!");
                Console.ResetColor();

                logger.LogMessage($"Deposited {depositAmount} to user account.", logedInUser);

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

                    logger.LogMessage("Attempted withdrawal without owning an agency.", logedInUser);

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

                        logger.LogMessage("Invalid withdrawal amount entered.", logedInUser);

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

                    if (trackedUser.Balance < withDrawAmount)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("❌ Insufficient funds in your agency balance to make this withdrawal.");
                        Console.ResetColor();

                        logger.LogMessage("Attempted withdrawal with insufficient funds.", logedInUser);

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
                        trackedUser.Balance -= withDrawAmount;
                        ownedAgency.Balance += withDrawAmount;

                        UserRepository.UpdateUser(trackedUser);
                        UserRepository.UpdateAgency(ownedAgency);



                        Console.WriteLine();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"✅ Withdrew {withDrawAmount} to your personal account successfully!");
                        Console.ResetColor();

                        logger.LogMessage(withDrawInput + " withdrawn to personal account.", logedInUser);

                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        Console.ResetColor();
                    }
                }
                break;
            case '0':
                logger.LogMessage("exited balance management.", logedInUser);
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

                logger.LogMessage("Invalid input in balance management.", logedInUser);

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
        Console.WriteLine();
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
            logger.LogMessage("exited agency view.", logedInUser);
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
                    logger.LogMessage("viewing agency: " + selectedAgency.Name, logedInUser);
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

                    Console.WriteLine("1. 💬 Add Comment About Agency");
                    Console.WriteLine("0. ❌ Exit");

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("Select an option: ");
                    var choice = Console.ReadKey();
                    Console.ResetColor();

                    switch (choice.KeyChar)
                    {
                        case '1':
                            logger.LogMessage("adding review for agency: " + selectedAgency.Name, logedInUser);
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
                                logger.LogMessage("Added review for agency: " + selectedAgency.Name, logedInUser);
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
                                logger.LogMessage("Attempted to add duplicate review for agency: " + selectedAgency.Name, logedInUser);
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
                            logger.LogMessage("exited agency profile view.", logedInUser);
                            Console.Clear();
                            return;
                        default:
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("⚠️ Invalid input. Please enter a valid option.");
                            Console.ResetColor();

                            logger.LogMessage(input + " is invalid input in agency profile view.", logedInUser);
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

                    logger.LogMessage(input + " is invalid Agency Id in agency view.", logedInUser);

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

                logger.LogMessage(input + " is invalid input in agency view.", logedInUser);

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
                Console.ResetColor();
            }
        }

    }

    public void resetPassword()
    {
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("🔑  Reset Password");
        Console.WriteLine("-------------------");
        Console.ResetColor();
        EmailSender emailSender = new EmailSender();
        Console.WriteLine("Enter your registered email: ");
        Console.Write(">> ");
        string email = Console.ReadLine();
        
        var user = UserRepository.getUserByEmail(email);
        if (user != null) 
        {
            var VerificationCode = new Random();
            int code = VerificationCode.Next(1000, 9999);

            emailSender.SendVerificationCode(email, "Reset Password", $"<!DOCTYPE html><html lang=\"en\"><head>    <meta charset=\"UTF-8\">    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">    <title>LuxTravel - Code Generator</title></head><body style=\"margin: 0; padding: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background: linear-gradient(135deg, #1e3c72 0%, #2a5298 100%); min-height: 100vh; display: flex; justify-content: center; align-items: center;\">        <div style=\"background: white; border-radius: 20px; box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3); padding: 50px; max-width: 500px; width: 90%; text-align: center;\">                <!-- Logo/Header -->        <div style=\"margin-bottom: 30px;\">            <h1 style=\"color: #1e3c72; margin: 0; font-size: 42px; font-weight: bold; letter-spacing: 1px;\">                ✈️ LuxTravel            </h1>            <p style=\"color: #666; margin: 10px 0 0 0; font-size: 16px;\">Your Journey, Our Priority</p>        </div>        <!-- Divider -->        <div style=\"height: 2px; background: linear-gradient(to right, transparent, #2a5298, transparent); margin: 30px 0;\"></div>        <!-- Code Generator Section -->        <div style=\"margin: 30px 0;\">            <h2 style=\"color: #1e3c72; font-size: 24px; margin-bottom: 20px;\">Email Verification Code</h2>                        <!-- Display Code -->            <div style=\"background: linear-gradient(135deg, #1e3c72 0%, #2a5298 100%); padding: 30px; border-radius: 15px; margin: 25px 0; box-shadow: 0 10px 25px rgba(30, 60, 114, 0.3);\">                <p style=\"color: rgba(255, 255, 255, 0.8); margin: 0 0 10px 0; font-size: 14px; text-transform: uppercase; letter-spacing: 2px;\">Your Verification Code</p>                <p style=\"color: white; font-size: 56px; font-weight: bold; letter-spacing: 8px; font-family: 'Courier New', monospace; text-shadow: 0 2px 10px rgba(0, 0, 0, 0.3); margin: 0;\">                    {code}                </p>            </div>        </div>        <!-- Info Text -->        <div style=\"margin-top: 30px; padding-top: 20px; border-top: 1px solid #e0e0e0;\">            <p style=\"color: #888; font-size: 13px; line-height: 1.6; margin: 0;\">                Enter the 4-digit verification code sent to your email address to confirm your email.            </p>        </div>    </div></body></html>");

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Verification code has been sent to your email. Please enter the code: ");
            Console.ResetColor();
            Console.Write(">> ");
            int inputCode = int.Parse(Console.ReadLine());

            if (inputCode == code)
            {
                Console.WriteLine();
                Console.WriteLine("Enter your new password: ");
                Console.Write(">> ");
                string newPassword = Console.ReadLine();
                user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                UserRepository.updateUserPassword(user);
                Console.ForegroundColor = ConsoleColor.Green;
                logger.LogMessage("Password reset successfully for user: " + user.UserName, user);
                Console.WriteLine("✅ Password has been reset successfully! Press any key to continue...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                logger.LogMessage("Failed password reset attempt due to incorrect verification code for user: " + user.UserName, user);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Incorrect verification code. Press any key to go back to menu...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
            }
        }
        else
        {
            logger.LogMessage("Failed password reset attempt due to unregistered email: " + email, new User { UserName = "Guest" });
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("❌ Email not found. Press any key to go back to menu...");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }
    }
}

