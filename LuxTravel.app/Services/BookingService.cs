using LuxTravel.app.Data;
using LuxTravel.app.Migrations;
using LuxTravel.app.Models;
using LuxTravel.app.Repositories;
using LuxTravel.app.Repositories.Interfaces;
using LuxTravel.app.Services.Interfaces;

namespace LuxTravel.app.Services;

public class BookingService : IBookingService
{

    public void BookTour(Tour tour, User logedInUser, Agency agency)
    {
        Console.Clear();

        BookingRepository bookingRepository = new BookingRepository();
        if (bookingRepository.HasExistingBooking(logedInUser.Id, tour.Id))
        {
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
                    if (participants <= tour.AvailableSpots && logedInUser.Balance >= tour.Price)
                    {
                        // for updates
                        AgencyRepository agencyRepository = new AgencyRepository();

                        tour.CurrentParticipants += participants;
                        

                        logedInUser.Balance -= tour.Price;
                        agency.TotalEarnings += tour.Price;
                        agency.TotalBookings += participants;
                        agency.Balance += tour.Price;

                        agencyRepository.UpdateUser(logedInUser);
                        agencyRepository.UpdateDatabase();


                        Booking newBooking = new Booking
                        {
                            TourId = tour.Id,
                            UserId = logedInUser.Id,
                            NumberOfPeople = participants,
                            BookingDate = DateTime.Now
                        };


                        // Save booking to database
                        bookingRepository.BookTour(newBooking);

                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("✅ Booking successful!");
                        Console.WriteLine("");
                        Console.ResetColor();

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
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("");
                        Console.WriteLine("Insufficient available spots or funds.");
                        Console.WriteLine("❌ Booking failed!");
                        Console.ResetColor();

                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Press any key to continue...");
                        Console.ResetColor();
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Booking cancelled.");
                    Console.ResetColor();
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
            foreach (var booking in bookings)
            {
                var tour = new TourRepository().GetTourById(booking.TourId);
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
                    TourRepository tourRepository = new TourRepository();
                    var tour = tourRepository.GetTourById(bookingToRemove.TourId);
                    var refundedAmount = bookingToRemove.NumberOfPeople * tour.Price;

                    logedInUser.Balance += refundedAmount;
                    tour.CurrentParticipants -= bookingToRemove.NumberOfPeople; 

                    bookingRepository.RemoveBooking(bookingToRemove);



                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine();
                    Console.WriteLine("✅ Booking removed successfully.");
                    Console.WriteLine($"💵 {refundedAmount} is recived back to your account!");
                    Console.ResetColor();

                    AgencyRepository agencyRepository = new AgencyRepository();
                    agencyRepository.UpdateUser(logedInUser);
                    agencyRepository.UpdateDatabase();


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
                Console.Clear();
                return;
            }
        }
    }
}
