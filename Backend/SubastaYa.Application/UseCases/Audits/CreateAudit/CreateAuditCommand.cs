using System;
using System.ComponentModel.DataAnnotations;

namespace SubastaYa.Application.UseCases.Audits.CreateAudit
{
    public class CreateAuditCommand
    {
        [Required]
        public string Entity { get; set; } = string.Empty;

        public int EntityId { get; set; }
        [Required]
        public string Action { get; set; } = string.Empty;

        public int? UserId { get; set; }

        public string? DetailJson { get; set; }
    }
}
