using MembershipSystem.Api.DTOs;
using MembershipSystem.Api.Models;
using MembershipSystem.Api.Requests;

namespace MembershipSystem.Api.Services
{
    public interface ITransactionsRepository
    {
        BalanceDto GetBalance(string cardId);
        BalanceDto AddAmount(AdjustAmount adjustAmount);
    }
}
