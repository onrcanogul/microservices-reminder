using Microservices.OrderApplication.Dtos;
using Microservices.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.OrderApplication.Feature.Queries.GetOrderByUserId
{
    public class GetOrdersByUserIdQueryResponse
    {
        public ServiceResponse<List<OrderDto>> Orders { get; set; }
    }
}
