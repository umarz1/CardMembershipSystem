using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MembershipSystem.Api.Requests
{
    public class AdjustAmount
    {
        [Required]
        [StringLength(16)]
        public string CardId { get; set; }
        [Required]
        [Range(1, 999.99)]
        public decimal Amount { get; set; }
    }
}
