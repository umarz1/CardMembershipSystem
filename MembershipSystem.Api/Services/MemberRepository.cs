using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using MembershipSystem.Api.DTOs;
using MembershipSystem.Api.Models;
using MembershipSystem.Api.Services.ExecuteCommands;
using MembershipSystem.Api.Services.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MembershipSystem.Api.Services
{
    public class MemberRepository: IMembershipRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ICommandText _commandText;
        private readonly string _connStr;
        private readonly IExecuters _executers;

        public MemberRepository(IConfiguration configuration, ICommandText commandText, IExecuters executers)
        {
            _commandText = commandText;
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("Dapper");
            _executers = executers;
        }

        public EmployeeDto GetMember(string cardId)
        {
            var employee = _executers.ExecuteCommand<Employee>(_connStr, conn =>
            conn.Query<Employee>(_commandText.GetEmployeeByCardId, new { @CardId = cardId }).SingleOrDefault());

            return new EmployeeDto
            {
                Name = employee.Name
            };
        }

        public EmployeeDto AddMember(Member member)
        {
            var createdMember = new EmployeeDto();
            try
            {
                _executers.ExecuteCommand(_connStr, conn =>
                {
                    var addEmployee = conn.Query<Member>(_commandText.AddEmployee,
                        new { EmployeeId = member.EmployeeId, Name = member.Name, Email = member.Email, Mobile = member.Mobile});

                    var addCard = conn.Query<Member>(_commandText.AddCard,
                        new { CardId = member.CardId, EmployeeId = member.EmployeeId, Pin= member.Pin });
                });

                createdMember = GetMember(member.CardId);
            }
            catch (SqlException ex) when (ex.Number == 2627 | ex.Number == 547)
            {
                return null;
            }

            return createdMember;
        }
    }
}
