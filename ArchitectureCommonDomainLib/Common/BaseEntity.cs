using ArchitectureCommonDomainLib.Common.Interfaces;

namespace ArchitectureCommonDomainLib.Common
{
    public class BaseEntity : IEntity
    {
        public Guid Id { get; set; }
    }
}
