namespace ArchitectureCommonDomainLib.Common.Interfaces
{
    public interface IAuditableEntity : IEntity
    {
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
}
