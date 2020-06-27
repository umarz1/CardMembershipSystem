using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class TransactionsRepositoryFake : ITransactionsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ICommandText _commandText;
        private readonly string _connStr;
        private readonly IExecuters _executers;

        public TransactionsRepositoryFake(IConfiguration configuration, ICommandText commandText, IExecuters executers)
        {
            _commandText = commandText;
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("Dapper");
            _executers = executers;
        }

        public Dictionary<string, Transaction> transactions = new Dictionary<string, Transaction>()
        {
            {"a09425bb-f7af-4554-b201-f16bef4b7244", new Transaction{ TransactionId = Guid.Parse("a09425bb-f7af-4554-b201-f16bef4b7244"), CardId= "VyDJ0lbYcPkzp2Ju", Date = new DateTime(2020, 06, 03), Amount = 10, Balance= 10 } },
            {"b1e1df5c-d2dc-4ffb-99aa-af6836870c9e", new Transaction{ TransactionId = Guid.Parse("b1e1df5c-d2dc-4ffb-99aa-af6836870c9e"), CardId= "VyDJ0lbYcPkzp2Ju", Date = new DateTime(2020, 06, 13), Amount = 15, Balance= 25 } },
            {"f12cf400-996e-4702-bc51-0f188409c84b", new Transaction{ TransactionId = Guid.Parse("f12cf400-996e-4702-bc51-0f188409c84b"), CardId= "VyDJ0lbYcPkzp2Ju", Date = new DateTime(2020, 06, 20), Amount = 5, Balance= 30 } },
            {"115d2aea-7134-4933-8da2-81873a81d81f", new Transaction{ TransactionId = Guid.Parse("115d2aea-7134-4933-8da2-81873a81d81f"), CardId= "VyDJ0lbYcPkzp2Ju", Date = new DateTime(2020, 06, 22), Amount = 2.50M, Balance= 30.25M } },
        };

        public BalanceDto GetBalance(string cardId)
        {
            var latestTransaction = transactions.Where(t => t.Value.CardId == cardId).OrderByDescending(x => x.Value.Date).FirstOrDefault();

            return new BalanceDto
            {
                Balance = latestTransaction.Value.Balance
            };
        }

        public BalanceDto AddAmount(AdjustAmount adjustAmount )
        {
            var latestTransaction = transactions.Where(t => t.Value.CardId == adjustAmount.CardId).OrderByDescending(x => x.Value.Date).FirstOrDefault();

            if(latestTransaction.Value == null)
            {
                return null;
            }

            var newTransactionId = Guid.NewGuid();

            var newTransaction = new Transaction()
            {
                TransactionId = newTransactionId,
                CardId = adjustAmount.CardId,
                Amount = adjustAmount.Amount,
                Date = DateTime.Now,
                Balance = latestTransaction.Value.Balance + adjustAmount.Amount
            };

            if (transactions.TryAdd(newTransactionId.ToString(), newTransaction))
            {
                return GetBalance(adjustAmount.CardId);
            }
            else
            {
                return null;
            }
        }
    }
}
