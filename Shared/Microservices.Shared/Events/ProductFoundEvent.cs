using Microservices.Shared.Events.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Shared.Events
{
    public class ProductFoundEvent : IEvent
    {
        public int OrderId { get; set; }
        public string BuyerId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
