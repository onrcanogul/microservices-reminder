using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.OrderApplication.Feature.Queries.GetOrderByUserId
{
    public class GetOrdersByUserIdQueryRequest : IRequest<GetOrdersByUserIdQueryResponse>
    {
        public string UserId { get; set; } = null!;
    }
}
