using Microservices.Shared.Base;
using Microservices.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : CustomBaseController
    {
        [HttpPost]
        public IActionResult ReceivePayment()
            => ControllerResponse(ServiceResponse<NoContent>.Success(204));
    }
}
