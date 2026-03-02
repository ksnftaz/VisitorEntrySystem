namespace VisitorEntrySystem.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual string GetDisplayInfo()
        {
            return $"Record ID: {Id}, Created: {CreatedAt}";
        }
    }
}