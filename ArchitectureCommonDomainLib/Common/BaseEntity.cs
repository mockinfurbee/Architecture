using ArchitectureCommonDomainLib.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ArchitectureCommonDomainLib.Common
{
    public class BaseEntity : IEntity
    {
        [Key]
        public string Guid { get; set; }
    }
}
