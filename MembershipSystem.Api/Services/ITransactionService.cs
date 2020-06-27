using MembershipSystem.Api.DTOs;
using MembershipSystem.Api.Models;
using MembershipSystem.Api.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MembershipSystem.Api.Services
{
    public interface ITransactionService 
    {
        BalanceDto GetBalance(string cardId);
        BalanceDto AddAmount(AdjustAmount adjustAmount);
    }
}
