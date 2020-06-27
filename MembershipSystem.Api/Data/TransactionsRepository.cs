using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using MembershipSystem.Api.DTOs;
using MembershipSystem.Api.Models;
using MembershipSystem.Api.Requests;
using MembershipSystem.Api.Services.ExecuteCommands;
using MembershipSystem.Api.Services.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MembershipSystem.Api.Services
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ITransactionsCommandText _commandText;
        private readonly string _connStr;
        private readonly IExecuters _executers;

        public TransactionsRepository(IConfiguration configuration, ITransactionsCommandText commandText, IExecuters executers)
        {
            _commandText = commandText;
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("Dapper");
            _executers = executers;
        }

        public BalanceDto GetBalance(string cardId)
        {
            var lastTransaction = _executers.ExecuteCommand<Transaction>(_connStr, conn =>
            conn.Query<Transaction>(_commandText.GetLatestCardTransactionByCardId, new { @CardId = cardId }).SingleOrDefault());

            if(lastTransaction == null)
            {
                return null;
            }

            return new BalanceDto
            {
                Balance = lastTransaction.Balance
            };
        }

        public BalanceDto AddAmount(AdjustAmount adjustAmount )
        {
            var lastestBalance = new BalanceDto();

            try
            {
                _executers.ExecuteCommand(_connStr, conn =>
                {
                    var addAmount = conn.Query<AdjustAmount>(_commandText.AddAmountToCard,
                        new { TransactionId = Guid.NewGuid() ,CardId = adjustAmount.CardId, Amount = adjustAmount.Amount });
                });

                lastestBalance = GetBalance(adjustAmount.CardId);
            }
            catch (SqlException ex) when (ex.Number == 2627 | ex.Number == 547)
            {
                return null;
            }

            return lastestBalance;
        }
    }
}
