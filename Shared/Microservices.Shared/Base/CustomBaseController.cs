using Microservices.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Shared.Base
{
    public class CustomBaseController : ControllerBase
    {
        public IActionResult ControllerResponse<T>(ServiceResponse<T> response) 
            => new ObjectResult(response) { StatusCode = response.StatusCode };
    }
}
