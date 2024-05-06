using ArchitectureCommonDomainLib.Entities;

namespace Application.CQRS.Users.Commands.CreateUser
{
    public class CreatedUserEvent : UserEvent
    {
        public CreatedUserEvent(User user) : base(user)
        {
        }
    }
}
