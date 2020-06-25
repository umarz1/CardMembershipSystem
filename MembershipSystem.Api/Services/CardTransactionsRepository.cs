﻿using System;
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
    public class CardTransactionsRepository : ICardTransactionsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ICommandText _commandText;
        private readonly string _connStr;
        private readonly IExecuters _executers;

        public CardTransactionsRepository(IConfiguration configuration, ICommandText commandText, IExecuters executers)
        {
            _commandText = commandText;
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("Dapper");
            _executers = executers;
        }

        public CardTransactionDto GetBalance(string cardId)
        {
            var lastTransaction = _executers.ExecuteCommand<CardTransaction>(_connStr, conn =>
            conn.Query<CardTransaction>(_commandText.GetLatestCardTransactionByCardId, new { @CardId = cardId }).SingleOrDefault());

            return new CardTransactionDto
            {
                Date = lastTransaction.Date,
                Balance = lastTransaction.Balance
            };
        }

        public CardTransactionDto AddAmount(AdjustAmount adjustAmount )
        {
            var lastestBalance = new CardTransactionDto();

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
