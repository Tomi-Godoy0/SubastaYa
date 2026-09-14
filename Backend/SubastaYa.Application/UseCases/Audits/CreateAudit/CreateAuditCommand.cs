using System;

namespace SubastaYa.Application.UseCases.Audits.CreateAudit
{
    public class CreateAuditCommand
    {
        public string Entity { get; set; } = string.Empty;

        public int EntityId { get; set; }

        public string Action { get; set; } = string.Empty;

        public int? UserId { get; set; }

        public string? DetailJson { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
