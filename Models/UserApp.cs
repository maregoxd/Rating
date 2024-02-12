using Microsoft.AspNetCore.Identity;

namespace Rating.Models
{
    public class UserApp : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
