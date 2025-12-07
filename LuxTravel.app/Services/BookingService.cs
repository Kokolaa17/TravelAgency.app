using LuxTravel.app.Data;
using LuxTravel.app.Helpers;
using LuxTravel.app.Migrations;
using LuxTravel.app.Models;
using LuxTravel.app.Repositories;
using LuxTravel.app.Repositories.Interfaces;
using LuxTravel.app.Services.Interfaces;

namespace LuxTravel.app.Services;

public class BookingService : IBookingService
{
    Logging logger = new Logging();

    public void BookTour(Tour tour, User logedInUser, Agency agency)
    {
        Console.Clear();

        BookingRepository bookingRepository = new BookingRepository();
        if (bookingRepository.HasExistingBooking(logedInUser.Id, tour.Id))
        {
            logger.LogMessage("User attempted to book a tour they have already booked.", logedInUser);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ You have already booked this tour!");
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
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine($"🔖 Booking {tour.Name}");
            Console.WriteLine("----------------------");
            Console.ResetColor();
            Console.WriteLine($"👥 Available spots: {tour.AvailableSpots}");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Enter number of participants to book: ");

            var input = Console.ReadLine();
            if (!int.TryParse(input, out int participants) || participants <= 0)
            {
                logger.LogMessage("User entered an invalid number of participants for booking.", logedInUser);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Invalid number of participants.");
                Console.ResetColor();
                return;
            }
            else
            {
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine($"💵 Tour price: {tour.Price * participants} {tour.Currency}. Are you sure you want to proceed with the payment?");
                Console.WriteLine();
                Console.WriteLine("1. ✅ Yes");
                Console.WriteLine("2. ❌ No");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("Select a option: ");

                var choice = Console.ReadKey().KeyChar;
                Console.ResetColor();

                if (choice == '1')
                {
                    var totalAmount = participants * tour.Price;
                    var availableSpots = tour.MaxParticipants - tour.CurrentParticipants;

                    if (logedInUser.Balance < totalAmount)
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("❌ Insufficient funds!");
                        Console.ResetColor();
                        logger.LogMessage("Booking failed due to insufficient user funds.", logedInUser);
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Press any key to continue...");
                        Console.ResetColor();
                        Console.ReadKey();
                        Console.Clear();
                        return;
                    }

                    if (availableSpots < participants)
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"❌ Only {availableSpots} spots available!");
                        Console.ResetColor();
                        logger.LogMessage($"Booking failed due to insufficient spots. Requested: {participants}, Available: {availableSpots}", logedInUser);
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Press any key to continue...");
                        Console.ResetColor();
                        Console.ReadKey();
                        Console.Clear();
                        return;
                    }

                    Booking newBooking = new Booking
                    {
                        TourId = tour.Id,
                        UserId = logedInUser.Id,
                        NumberOfPeople = participants,
                        BookingDate = DateTime.Now,
                        TotalPrice = totalAmount 
                    };

                    var twoInfo = bookingRepository.GetTourWithAgency(tour.Id);
                    var Trackeduser = bookingRepository.GetUserById(logedInUser.Id);
                    // apply changes
                    Trackeduser.Balance -= totalAmount;
                    twoInfo.Agency.TotalEarnings += totalAmount;
                    logedInUser.Balance = Trackeduser.Balance;
                    twoInfo.Agency.TotalBookings += participants;
                    twoInfo.Agency.Balance += totalAmount;
                    twoInfo.CurrentParticipants += participants;

                    // save changes for user + tour + agency
                    bookingRepository.UpdateUserTwo(Trackeduser);
                    bookingRepository.Save();

                    // save booking separately
                    bookingRepository.BookTour(newBooking);

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("✅ Booking successful!");
                    Console.WriteLine("");
                    Console.ResetColor();
                    logger.LogMessage($"User successfully booked tour: {tour.Name} for {participants} participants.", logedInUser);
                    

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Press any key to return to the tours list...");
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }
                else
                {
                    logger.LogMessage("User cancelled the booking process.", logedInUser);
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ Booking cancelled.");
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

    public void ViewUserBookings(User logedInUser)
    {
        Console.Clear();
        BookingRepository bookingRepository = new BookingRepository();

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine($"🎫 {logedInUser.UserName} Bookings:");
        Console.WriteLine("------------------------");
        Console.ResetColor();
        var bookings = bookingRepository.UserBookings(logedInUser).ToList();

        if (bookings.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No bookings found.");
            Console.ResetColor();

            logger.LogMessage("User has no bookings", logedInUser);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Press any key to return to the previous menu...");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
            return;
        }
        else
        {
            logger.LogMessage($"User has {bookings.Count} bookings.", logedInUser);

            foreach (var booking in bookings)
            {
                var tour = bookingRepository.GetTourById(booking.TourId);
                Console.WriteLine($"[{booking.Id}] 🔖 {tour.Name} | {tour.StartingPoint} - {tour.Destination} | Participants: {booking.NumberOfPeople} | Date: {booking.BookingDate}");
            }

            Console.WriteLine();
            Console.WriteLine("[0] ❌ Exit");


            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Choose an ID to remove the booking, or enter 0 to exit: ");
            Console.ResetColor();
            var input = Console.ReadLine();

            if (int.TryParse(input, out int bookingId) && bookingId != 0)
            {
                var bookingToRemove = bookings.FirstOrDefault(b => b.Id == bookingId);

                if (bookingToRemove != null)
                {
                    var tour = bookingRepository.GetTourById(bookingToRemove.TourId);
                    var refundedAmount = bookingToRemove.NumberOfPeople * tour.Price;

                    logedInUser.Balance += refundedAmount;
                    var tourAgency = bookingRepository.GetAgencyByTourId(bookingToRemove.TourId);
                    tour.CurrentParticipants -= bookingToRemove.NumberOfPeople;
                    tourAgency.TotalBookings -= bookingToRemove.NumberOfPeople;
                    tourAgency.TotalEarnings -= bookingToRemove.TotalPrice;
                    tourAgency.Balance -= refundedAmount;


                    bookingRepository.UpdateUser(logedInUser);
                    bookingRepository.RemoveBooking(bookingToRemove);



                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine();
                    Console.WriteLine("✅ Booking removed successfully.");
                    Console.WriteLine($"💵 {refundedAmount} is recived back to your account!");
                    Console.ResetColor();

                    logger.LogMessage(user: logedInUser, message: $"Booking ID {bookingId} removed. Refunded Amount: {refundedAmount}.");

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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ Booking ID not found.");
                    Console.ResetColor();

                    logger.LogMessage($"User entered invalid booking ID {bookingId} for removal.", user: logedInUser);

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Press any key to continue...");
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }
            }
            else
            {
                logger.LogMessage("User exited the bookings menu without making changes.", user: logedInUser);
                Console.Clear();
                return;
            }
        }
    }
}
