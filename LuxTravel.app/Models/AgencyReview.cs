using LuxTravel.app.Core;

namespace LuxTravel.app.Models;

public class AgencyReview : Base
{
    public int AgencyId { get; set; }
    public int UserId { get; set; }
    public int Rating { get; set; } 
    public string Comment { get; set; }
    public Agency Agency { get; set; }
    public User User { get; set; }
}
