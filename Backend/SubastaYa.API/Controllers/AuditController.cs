using Microsoft.AspNetCore.Mvc;
using SubastaYa.Application.Interfaces.Service.Audits;
using SubastaYa.Application.UseCases.Audits.CreateAudit;
using SubastaYa.Application.UseCases.Audits.GetAudit;
using SubastaYa.Application.UseCases.Audits.GetAudits;

namespace SubastaYa.API.Controllers
{
    [Route("api/v1/audits")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly ICreateAuditHandler _createAuditHandler;
        private readonly IGetAuditHandler _getAuditHandler;
        private readonly IGetAuditsHandler _getAuditsHandler;

        public AuditController(
            ICreateAuditHandler createAuditHandler,
            IGetAuditHandler getAuditHandler,
            IGetAuditsHandler getAuditsHandler)
        {
            _createAuditHandler = createAuditHandler;
            _getAuditHandler = getAuditHandler;
            _getAuditsHandler = getAuditsHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAudits(
            [FromQuery] GetAuditsQuery query)
        {
            var audits = await _getAuditsHandler.HandleAsync(query);

            return Ok(audits);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAudit(
            CreateAuditCommand command)
        {
            var audit = await _createAuditHandler.HandleAsync(command);

            return CreatedAtAction(
                nameof(GetAuditById),
                new { id = audit.Id },
                audit);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuditById(int id)
        {
            var audit = await _getAuditHandler.HandleAsync(
                new GetAuditQuery
                {
                    Id = id
                });

            return Ok(audit);
        }
    }
}