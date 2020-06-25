using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MembershipSystem.Api.Requests
{
    public class AdjustAmount
    {
        public string CardId { get; set; }
        public decimal Amount { get; set; }
    }
}
