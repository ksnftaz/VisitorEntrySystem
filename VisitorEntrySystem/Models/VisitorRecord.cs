namespace VisitorEntrySystem.Models
{
    public class VisitorRecord : BaseEntity
    {
        public string FullName { get; set; }
        public string ContactNumber { get; set; }
        public string Purpose { get; set; }
        public string HostName { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string RegisteredBy { get; set; }
        public int? AdminId { get; set; }
        public bool HasCheckedOut() => CheckOutTime != null;
    }
}