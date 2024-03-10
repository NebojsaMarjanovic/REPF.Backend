using Microsoft.AspNetCore.Identity;

namespace AuthenticationService.DbSets
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
