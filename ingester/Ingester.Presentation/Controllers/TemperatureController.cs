using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ingester.Application.Commands;
using Ingester.Application.DataContracts.Dtos;
using Ingester.Application.DataContracts.Requests;
using Ingester.Application.DataContracts.Responses;
using Ingester.Application.Queries;
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

        [HttpGet]
        public async Task<ActionResult<TemperatureResponse>> Get()
        {
            var temperatures = await mediator.Send(new GetTemperatureListQuery());
            var response = new TemperatureResponse
            {
                Temperatures = temperatures
            };
            return base.Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TemperatureDto>> Get(string id)
        {
            var temperature = await mediator.Send(new GetTemperatureQuery(id));
            if (temperature == null)
            {
                return NotFound(null);
            }
            return base.Ok(temperature);
        }

        [HttpPost]
        public async Task<ActionResult<TemperatureDto>> Post([FromBody] TemperatureRequest temperatureRequest)
        {
            var id = await mediator.Send(new PostTemperatureCommand(temperatureRequest));
            var temperature = await mediator.Send(new GetTemperatureQuery(id));
            return CreatedAtAction(nameof(Get), new { id }, temperature);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TemperatureDto>> Put(string id, [FromBody] TemperatureRequest temperatureRequest)
        {
            var success = await mediator.Send(new PutTemperatureCommand(id, temperatureRequest));
            if (success)
            {
                return await Get(id);
            }
            return NotFound("Entity not found");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var success = await mediator.Send(new DeleteTemperatureCommand(id));
            if (success)
            {
                return Ok();
            }
            return NotFound("Entity not found");
        }
    }
}
