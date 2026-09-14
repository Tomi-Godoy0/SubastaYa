using SubastaYa.Application.Interfaces.DTOs;
using SubastaYa.Application.UseCases.Audits.GetAudits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Interfaces.Service.Audits
{
    public interface IGetAuditsHandler
    {
        public Task<PagedResult<AuditResponse>> HandleAsync(
            GetAuditsQuery query);
    }
}