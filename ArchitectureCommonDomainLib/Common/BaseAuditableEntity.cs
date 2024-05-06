using ArchitectureCommonDomainLib.Common.Interfaces;

namespace ArchitectureCommonDomainLib.Common
{
    public class BaseAuditableEntity : BaseEntity, IAuditableEntity
    {
        #region Metadata:
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        #endregion
    }
}
