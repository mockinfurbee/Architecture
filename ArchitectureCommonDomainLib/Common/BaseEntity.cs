using ArchitectureCommonDomainLib.Common.Interfaces;

namespace ArchitectureCommonDomainLib.Common
{
    public class BaseEntity : IEntity
    {
        public string Guid { get; set; }
    }
}
