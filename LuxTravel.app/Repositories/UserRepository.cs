using LuxTravel.app.Data;
using LuxTravel.app.Helpers;
using LuxTravel.app.Models;
using LuxTravel.app.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LuxTravel.app.Repositories;

internal class UserRepository : IUserRepository
{
    public DataContext dataContext = new DataContext();

    public User UserLogIn(string emailOrPassword, string password)
    {
        var user = dataContext.Users.FirstOrDefault(u => u.Email == emailOrPassword || u.UserName == emailOrPassword);
        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return user;
        }
        return null; 
    }

    public User UserRegistration(User newUser)
    {
        if (dataContext.Users.Any(u => u.Email == newUser.Email || u.UserName == newUser.UserName))
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Email or Username already in use!");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Press any key to go back to menu...");
            Console.ReadKey();
            Console.Clear();
            Console.ResetColor();
            return null;
        }
        else
        {
            Random VerificationCode = new Random();
            int code = VerificationCode.Next(1000, 9999);

            EmailSender emailSender = new EmailSender();
            emailSender.SendVerificationCode(newUser.Email, "Verification Code", $"<!DOCTYPE html><html lang=\"en\"><head>    <meta charset=\"UTF-8\">    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">    <title>LuxTravel - Code Generator</title></head><body style=\"margin: 0; padding: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background: linear-gradient(135deg, #1e3c72 0%, #2a5298 100%); min-height: 100vh; display: flex; justify-content: center; align-items: center;\">        <div style=\"background: white; border-radius: 20px; box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3); padding: 50px; max-width: 500px; width: 90%; text-align: center;\">                <!-- Logo/Header -->        <div style=\"margin-bottom: 30px;\">            <h1 style=\"color: #1e3c72; margin: 0; font-size: 42px; font-weight: bold; letter-spacing: 1px;\">                ✈️ LuxTravel            </h1>            <p style=\"color: #666; margin: 10px 0 0 0; font-size: 16px;\">Your Journey, Our Priority</p>        </div>        <!-- Divider -->        <div style=\"height: 2px; background: linear-gradient(to right, transparent, #2a5298, transparent); margin: 30px 0;\"></div>        <!-- Code Generator Section -->        <div style=\"margin: 30px 0;\">            <h2 style=\"color: #1e3c72; font-size: 24px; margin-bottom: 20px;\">Email Verification Code</h2>                        <!-- Display Code -->            <div style=\"background: linear-gradient(135deg, #1e3c72 0%, #2a5298 100%); padding: 30px; border-radius: 15px; margin: 25px 0; box-shadow: 0 10px 25px rgba(30, 60, 114, 0.3);\">                <p style=\"color: rgba(255, 255, 255, 0.8); margin: 0 0 10px 0; font-size: 14px; text-transform: uppercase; letter-spacing: 2px;\">Your Verification Code</p>                <p style=\"color: white; font-size: 56px; font-weight: bold; letter-spacing: 8px; font-family: 'Courier New', monospace; text-shadow: 0 2px 10px rgba(0, 0, 0, 0.3); margin: 0;\">                    {code}                </p>            </div>        </div>        <!-- Info Text -->        <div style=\"margin-top: 30px; padding-top: 20px; border-top: 1px solid #e0e0e0;\">            <p style=\"color: #888; font-size: 13px; line-height: 1.6; margin: 0;\">                Enter the 4-digit verification code sent to your email address to confirm your email.            </p>        </div>    </div></body></html>");

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Verification code has been sent to your email.");
            Console.ResetColor();
            Console.WriteLine("Enter 4-digit code below to proceed:");

            Console.Write(">> ");
            var input = Console.ReadLine();
            
            if(!int.TryParse(input, out int confirmationKey))
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid code format. Press any key to go back to menu...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
                return null;
            }
            else if (confirmationKey == code)
            {
                dataContext.Users.Add(newUser);
                dataContext.SaveChanges();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Registration successful! Press any key to continue...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
                return newUser;
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Incorrect verification code. Press any key to go back to menu...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
                return null;
            }
        }
    }
    public Agency RegisterAgency(Agency newAgency)
    {
        dataContext.Add(newAgency);
        dataContext.SaveChanges();

        return newAgency;
    }

    public Agency GetAgencyByOwnerId(int ownerId)
    {
        return dataContext.Agencies.FirstOrDefault(a => a.OwnerId == ownerId);
    }

    public List<Agency> ViewAllAgencies()
    {
        return dataContext.Agencies.Include(a => a.Owner).ToList();
    }

    public void AddAgencyReview(AgencyReview newReview)
    {
        dataContext.AgencyReviews.Add(newReview);
        dataContext.SaveChanges();
    }
    public AgencyReview GetAllAgencyRevies(User user, Agency agency)
    {
        return dataContext.AgencyReviews.FirstOrDefault(ar => ar.UserId == user.Id && ar.AgencyId == agency.Id);
    }
}
