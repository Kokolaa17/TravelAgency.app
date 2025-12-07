using LuxTravel.app.Models;

namespace LuxTravel.app.Helpers;

public class Logging
{
    public void LogMessage(string message, User user)
    {
        using(StreamWriter writer = new StreamWriter(@"C:\\Users\\Giorgi\\Desktop\\TravelAgencyLogging\logs.txt", true))
        {
            writer.WriteLine($"[{DateTime.Now}] [User: {user.UserName}] {message}");
        }
    }
}
