namespace VIKTALEA_Backend.Shared
{
    public class Controls
    {
        public int activate { get; set; } = 1;
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public DateTime? updatedAt { get; set; }
    }
}
