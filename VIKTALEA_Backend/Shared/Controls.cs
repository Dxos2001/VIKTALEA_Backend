namespace VIKTALEA_Backend.Shared
{
    public class Controls
    {
        public bool activate { get; set; } = true;
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public DateTime? updatedAt { get; set; }
    }
}
