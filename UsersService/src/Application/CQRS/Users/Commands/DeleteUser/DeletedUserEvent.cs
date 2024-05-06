using ArchitectureCommonDomainLib.Entities;

namespace Application.CQRS.Users.Commands.DeleteUser
{
    internal class DeletedUserEvent : UserEvent
    {
        public DeletedUserEvent(User user) : base(user)
        {
        }
    }
}
