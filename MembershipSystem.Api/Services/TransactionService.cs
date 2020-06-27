using MembershipSystem.Api.DTOs;
using MembershipSystem.Api.Models;
using MembershipSystem.Api.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;

namespace MembershipSystem.Api.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionsRepository _transactionsRepo;

        public TransactionService(ITransactionsRepository transactionsRepo)
        {
            _transactionsRepo = transactionsRepo;
        }

        public BalanceDto GetBalance(string cardId)
        {
            var balance = _transactionsRepo.GetBalance(cardId);
            return balance;
        }

        public BalanceDto AddAmount(AdjustAmount adjustAmount)
        {
            var addAmount = _transactionsRepo.AddAmount(adjustAmount);
            return addAmount;
        }
    }
}
