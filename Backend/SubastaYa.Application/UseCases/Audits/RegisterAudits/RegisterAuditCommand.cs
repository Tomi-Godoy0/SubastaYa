using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.UseCases.Audits.RegisterAudits
{
    public class RegisterAuditCommand
    {
        public string Entity { get; set; } = string.Empty;
        public int EntityId { get; set; }
        public string Action { get; set; } = string.Empty;
        public int? UserId { get; set; }
        public string? DetailJson { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}