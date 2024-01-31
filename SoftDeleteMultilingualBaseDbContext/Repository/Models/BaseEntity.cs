namespace SoftDeleteMultilingualBaseDbContext.Repository.Models
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public string? Language { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? ModificationTime { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
