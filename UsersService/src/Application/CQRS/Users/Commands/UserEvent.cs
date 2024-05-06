using ArchitectureCommonDomainLib.Common;
using ArchitectureCommonDomainLib.Entities;

namespace Application.CQRS.Users.Commands
{
    public class UserEvent : BaseEvent
    {
        public User User { get; }

        public UserEvent(User user)
        {
            User = user;
        }
    }
}
