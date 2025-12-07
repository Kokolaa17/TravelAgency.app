using LuxTravel.app.Data;
using LuxTravel.app.Helpers;
using LuxTravel.app.Models;
using LuxTravel.app.Repositories.Interfaces;

namespace LuxTravel.app.Repositories;

internal class TourRepository : ITourRepository
{
    public DataContext DataContext = new DataContext();

    public List<Tour> FilterByDestination(string destination)
    {
        return DataContext.Tours.Where(tour => tour.Destination.ToLower() == destination.ToLower() && tour.Status == Enums.Enums.TourStatus.Published).ToList();
    }

    public List<Tour> FilterByMinAge (int minAge)
    {
        return DataContext.Tours.Where(tour => tour.MinAge <= minAge && tour.Status == Enums.Enums.TourStatus.Published).ToList();
    }

    public List<Tour> FilterByDuration(int minDays, int maxDays)
    {
        return DataContext.Tours.AsEnumerable()
               .Where(tour => tour.DurationDays >= minDays && tour.DurationDays <= maxDays && tour.Status == Enums.Enums.TourStatus.Published)
               .ToList();
    }

    public List<Tour> FilterByPrice(decimal minPrice, decimal maxPrice)
    {
        return DataContext.Tours.Where(tour => tour.Price >= minPrice && tour.Price <= maxPrice && tour.Status == Enums.Enums.TourStatus.Published).ToList();
    }

    public Tour GetTourById(int id)
    {
        return DataContext.Tours.FirstOrDefault(tour => tour.Id == id && tour.Status == Enums.Enums.TourStatus.Published);
    }

    public List<Tour> SeeAllTour()
    {
        return DataContext.Tours
            .Where(t => t.Status == Enums.Enums.TourStatus.Published)
            .ToList();
    }

    public void CreateBooking(int userId, int tourId, int numberOfPeople)
    {
        var user = DataContext.Users.FirstOrDefault(u => u.Id == userId);
        var tour = DataContext.Tours.FirstOrDefault(t => t.Id == tourId);

        if (tour == null || tour.AvailableSpots < numberOfPeople)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Tour not found or not enough available spots!");
            Console.ResetColor();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Press any key to continue...");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }
        else
        {
            Booking newBooking = new Booking
            {
                UserId = userId,
                TourId = tourId,
                NumberOfPeople = numberOfPeople,
                BookingDate = DateTime.Now
            };

            decimal pricePerPerson = tour.IsOnSale && tour.SalePrice.HasValue ? tour.SalePrice.Value : tour.Price;
            newBooking.TotalPrice = pricePerPerson * numberOfPeople;

            Random random = new Random();
            int confrimationCode = random.Next(1000, 9999);
            tour.CurrentParticipants += numberOfPeople;

            EmailSender emailSender = new EmailSender();
            emailSender.SendBookingConfrimation(user.Email, "Confrim Booking", $"<!DOCTYPE html><html lang=\"en\"><head> <meta charset=\"UTF-8\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"> <title>Booking Confirmation - LuxTravel</title></head><body style=\"margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4;\"> <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"background-color: #f4f4f4; padding: 20px 0;\"> <tr> <td align=\"center\"> <table width=\"600\" cellpadding=\"0\" cellspacing=\"0\" style=\"background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0,0,0,0.1);\"> <!-- Header --> <tr> <td style=\"background-color: #1e40af; padding: 40px 30px; text-align: center;\"> <h1 style=\"margin: 0; color: #ffffff; font-size: 32px; font-weight: bold;\">LuxTravel</h1> <p style=\"margin: 10px 0 0 0; color: #dbeafe; font-size: 16px;\">Your Journey Awaits</p> </td> </tr> <!-- Success Message --> <tr> <td style=\"padding: 40px 30px 20px 30px; text-align: center;\"> <div style=\"display: inline-block; background-color: #dcfce7; border-radius: 50%; width: 80px; height: 80px; line-height: 80px; margin-bottom: 20px;\"> <span style=\"color: #16a34a; font-size: 48px;\">✓</span> </div> <h2 style=\"margin: 0 0 10px 0; color: #1e293b; font-size: 28px;\">Booking Confirmed!</h2> <p style=\"margin: 0; color: #64748b; font-size: 16px;\">Thank you for choosing LuxTravel</p> </td> </tr> <!-- Confirmation Code --> <tr> <td style=\"padding: 20px 30px;\"> <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"background-color: #eff6ff; border-radius: 8px; border: 2px solid #3b82f6;\"> <tr> <td style=\"padding: 30px; text-align: center;\"> <p style=\"margin: 0 0 10px 0; color: #1e40af; font-size: 14px; font-weight: bold; text-transform: uppercase; letter-spacing: 1px;\">Confirmation Code</p> <p style=\"margin: 0; color: #1e40af; font-size: 42px; font-weight: bold; letter-spacing: 8px; font-family: 'Courier New', monospace;\">{confrimationCode}</p> </td> </tr> </table> </td> </tr> <!-- Booking Details --> <tr> <td style=\"padding: 20px 30px;\"> <h3 style=\"margin: 0 0 20px 0; color: #1e293b; font-size: 20px; border-bottom: 2px solid #e2e8f0; padding-bottom: 10px;\">Booking Details</h3> <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\"> <tr> <td style=\"padding: 12px 0; border-bottom: 1px solid #e2e8f0;\"> <p style=\"margin: 0; color: #64748b; font-size: 14px;\">Tour Name</p> <p style=\"margin: 5px 0 0 0; color: #1e293b; font-size: 16px; font-weight: bold;\">{tour.Name}</p> </td> </tr> <tr> <td style=\"padding: 12px 0; border-bottom: 1px solid #e2e8f0;\"> <p style=\"margin: 0; color: #64748b; font-size: 14px;\">Booking Date</p> <p style=\"margin: 5px 0 0 0; color: #1e293b; font-size: 16px;\">{newBooking.BookingDate}</p> </td> </tr> <tr> <td style=\"padding: 12px 0; border-bottom: 1px solid #e2e8f0;\"> <p style=\"margin: 0; color: #64748b; font-size: 14px;\">Tour Dates</p> <p style=\"margin: 5px 0 0 0; color: #1e293b; font-size: 16px;\">{tour.StartDate} - {tour.EndDate}</p> </td> </tr> <tr> <td style=\"padding: 12px 0; border-bottom: 1px solid #e2e8f0;\"> <p style=\"margin: 0; color: #64748b; font-size: 14px;\">Number of Participants</p> <p style=\"margin: 5px 0 0 0; color: #1e293b; font-size: 16px;\">{tour.CurrentParticipants}</p> </td> </tr> <tr> <td style=\"padding: 12px 0; border-bottom: 1px solid #e2e8f0;\"> <p style=\"margin: 0; color: #64748b; font-size: 14px;\">Discount Applied</p> <p style=\"margin: 5px 0 0 0; color: #16a34a; font-size: 16px; font-weight: bold;\"></p> </td> </tr> <tr> <td style=\"padding: 20px 0;\"> <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"background-color: #1e40af; border-radius: 8px;\"> <tr> <td style=\"padding: 20px;\"> <p style=\"margin: 0; color: #dbeafe; font-size: 14px; text-transform: uppercase; letter-spacing: 1px;\">Total Price</p> <p style=\"margin: 10px 0 0 0; color: #ffffff; font-size: 32px; font-weight: bold;\">{newBooking.TotalPrice} {tour.Currency}</p> </td> </tr> </table> </td> </tr> </table> </td> </tr> <!-- Important Information --> <tr> <td style=\"padding: 20px 30px;\"> <div style=\"background-color: #fef3c7; border-left: 4px solid #f59e0b; padding: 15px; border-radius: 4px;\"> <p style=\"margin: 0; color: #92400e; font-size: 14px; line-height: 1.6;\"> <strong>Important:</strong> Please save this confirmation code. You'll need it to access your booking details and during check-in. </p> </div> </td> </tr> <!-- Footer --> <tr> <td style=\"background-color: #f8fafc; padding: 30px; text-align: center; border-top: 1px solid #e2e8f0;\"> <p style=\"margin: 0 0 10px 0; color: #64748b; font-size: 14px;\">Need help? Contact us at:</p> <p style=\"margin: 0 0 5px 0;\"> <a href=\"mailto:support@luxtravel.com\" style=\"color: #1e40af; text-decoration: none; font-size: 14px;\">support@luxtravel.com</a> </p> <p style=\"margin: 0 0 20px 0;\"> <a href=\"tel:+1234567890\" style=\"color: #1e40af; text-decoration: none; font-size: 14px;\">+1 (234) 567-890</a> </p> <p style=\"margin: 0; color: #94a3b8; font-size: 12px;\"> © 2024 LuxTravel. All rights reserved. </p> </td> </tr> </table> </td> </tr> </table></body></html>");
            Console.WriteLine("Enter Confrimation Code To End Booking: ");
            Console.Write(">> ");

            int enteredCode = int.Parse(Console.ReadLine());
            if (enteredCode != confrimationCode)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Confirmation Code! Booking Failed.");
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
                DataContext.Bookings.Add(newBooking);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Booking created successfully.");
                Console.ResetColor();

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
