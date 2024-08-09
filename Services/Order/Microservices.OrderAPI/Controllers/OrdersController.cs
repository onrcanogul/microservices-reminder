using MediatR;
using Microservices.OrderApplication.Feature.Commands.CreateOrder;
using Microservices.OrderApplication.Feature.Commands.DeleteOrder;
using Microservices.OrderApplication.Feature.Commands.UpdateOrderItem;
using Microservices.OrderApplication.Feature.Queries.GetOrderById;
using Microservices.OrderApplication.Feature.Queries.GetOrderByUserId;
using Microservices.OrderApplication.Feature.Queries.GetOrders;
using Microservices.Shared.Base;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IMediator mediator) : CustomBaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get()
            => ControllerResponse((await mediator.Send(new GetOrdersQueryRequest())).OrdersResponse);
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] GetOrderByIdQueryRequest request)
            => ControllerResponse((await mediator.Send(request)).OrderItemResponse);
        [HttpGet("get-by-user-id/{UserId}")]
        public async Task<IActionResult> GetByUser([FromRoute] GetOrdersByUserIdQueryRequest request)
            => ControllerResponse((await mediator.Send(request)).Orders);
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderCommandRequest request)
            => ControllerResponse((await mediator.Send(request)).Response);
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute]DeleteOrderCommandRequest request)
            => ControllerResponse((await mediator.Send(request)).Response);
        [HttpPut]
        public async Task<IActionResult> UpdateOrderItem(UpdateOrderItemCommandRequest request)
            => ControllerResponse((await mediator.Send(request)).Response);
    }
}
