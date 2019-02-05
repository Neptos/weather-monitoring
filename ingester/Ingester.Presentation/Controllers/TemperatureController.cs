using System.Threading.Tasks;
using Ingester.Application.DataContracts.Requests;
using Ingester.Application.Handlers.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ingester.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureController : ControllerBase
    {
        private readonly IMediator mediator;

        public TemperatureController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TemperatureRequest temperatureRequest)
        {
            await mediator.Send(new PostTemperatureRequest(temperatureRequest));
            return Ok();
        }
    }
}
