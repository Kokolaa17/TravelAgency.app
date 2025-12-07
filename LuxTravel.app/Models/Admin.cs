using LuxTravel.app.Core;
using LuxTravel.app.Enums;
using static LuxTravel.app.Enums.Enums;

namespace LuxTravel.app.Models;

public class Admin : Base
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public userRole Role { get; set; } = userRole.Admin;
}
