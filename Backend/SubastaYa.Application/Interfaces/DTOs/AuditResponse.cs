using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Interfaces.DTOs
{
    public class AuditResponse
    {
        public int Id { get; set; }
        public string Entity { get; set; }
        public int EntityId { get; set; }
        public string Action { get; set; }
        public int? UserId { get; set; }
        public string? DetailJson { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
