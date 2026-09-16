namespace SubastaYa.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string Entity { get; set; } = string.Empty;
        public int EntityId { get; set; }
        public string Action { get; set; } = string.Empty;
        public int? UserId { get; set; }
        public string? DetailJson { get; set; }
        public DateTime CreatedAt { get; set; }

        //-------
        public User? User { get; set; }
    }
}
