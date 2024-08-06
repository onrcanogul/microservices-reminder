using Microservices.Shared.Events.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Shared.Events
{
    public class ProductNotFoundEvent : IEvent
    {
        public int OrderId { get; set; }
        public string Message { get; set; }
    }
}
