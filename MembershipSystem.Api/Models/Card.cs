using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MembershipSystem.Api.Models
{
    public class Card
    {
        public string CardId { get; set; }
        public string EmployeeId { get; set; }
        public string Pin { get; set; }
    }
}
