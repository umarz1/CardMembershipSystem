using MembershipSystem.Api.DTOs;
using MembershipSystem.Api.Models;
using MembershipSystem.Api.Requests;

namespace MembershipSystem.Api.Services
{
    public interface ICardTransactionsRepository
    {
        CardTransactionDto GetBalance(string cardId);
        CardTransactionDto AddAmount(AdjustAmount adjustAmount);
    }
}
