using Microsoft.AspNetCore.Identity;

namespace Persistence.Entities
{
    public class User : IdentityUser
    {
        public User()
        {
            UserName = Id; // TODO: check.
        }
    }
}
