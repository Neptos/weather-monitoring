using System.Threading.Tasks;
using Ingester.Application.DataContracts.Requests;
using Ingester.Application.Handlers.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ingester.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrecipitationController : ControllerBase
    {
        private readonly IMediator mediator;

        public PrecipitationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] DataPointRequest request)
        {
            request.Type = "Precipitation";
            await mediator.Send(new PostDataPointRequest(request));
            return Ok();
        }
    }
}
