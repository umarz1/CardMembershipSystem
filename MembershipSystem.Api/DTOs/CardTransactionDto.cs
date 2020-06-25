using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MembershipSystem.Api.DTOs
{
    public class CardTransactionDto
    {
        public DateTime Date { get; set; }
        public decimal Balance { get; set; }
    }
}
