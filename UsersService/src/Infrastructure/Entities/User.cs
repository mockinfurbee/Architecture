using Application.Interfaces.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Entities
{
    public class User : IdentityUser, IUser
    {

    }
}
