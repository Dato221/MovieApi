namespace SimplifiedIMDBApi.Entities
{
    public class BaseEntity
    {
        public string CreatedBy { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; } 
        public bool IsActive { get; set; } = true; 
    }
}
