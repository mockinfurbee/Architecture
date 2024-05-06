using ArchitectureCommonDomainLib.Common;

namespace ArchitectureCommonDomainLib.Entities
{
    public class User : BaseAuditableEntity
    {
        public string Guid { get; set; }
        public string PasswordHash { get; set; }
    }
}
