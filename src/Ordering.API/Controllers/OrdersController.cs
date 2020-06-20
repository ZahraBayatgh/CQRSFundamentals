using MediatR;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Application.Commands;
using Ordering.API.Application.DTOs;
using Ordering.API.Queries;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using Ordering.API.Application.Queries;

namespace Ordering.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [Route("Order")]
        [HttpPost]
        public async Task<ActionResult<Result>> CreateOrderFromBasketDataAsync([FromBody] OrderDTO orderDTO)
        {
            var createOrderCommand = new CreateOrderCommand(orderDTO.Items, orderDTO.UserId, orderDTO.UserName, orderDTO.City, orderDTO.Street, orderDTO.Country, orderDTO.ZipCode);

            var result = await _mediator.Send(createOrderCommand);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Result>> GetOrderByIdAsync([FromRoute] int id)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(id));

            return Ok(order.Value);
        }
        [Route("cancel")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Result>> CancelOrderAsync([FromBody]CancelOrderDTO cancelOrderDTO)
        {
            var command = new CancelOrderCommand(cancelOrderDTO.OrderNumber);

            var commandResult = await _mediator.Send(command);

            if (commandResult.IsFailure)
            {
                return BadRequest();
            }

            return Ok();
        }

        [Route("ship")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Result>> ShipOrderAsync([FromBody]ShipOrderDTO shipOrderDTO)
        {
            var command = new ShipOrderCommand(shipOrderDTO.OrderNumber);

            var commandResult = await _mediator.Send(command);


            if (commandResult.IsFailure)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderSummary>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderSummary>>> GetOrdersAsync()
        {
            //This is a test userId, but you must receive this user number from request after authentication.
            int userId = 1;
            var orders = await _mediator.Send(new GetOrdersFromUserQuery(userId));

            return Ok(orders.Value);
        }
    }
}
