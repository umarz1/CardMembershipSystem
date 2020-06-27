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
        private readonly IMembersCommandText _membersCommandText;
        private readonly string _connStr;
        private readonly IExecuters _executers;

        public MemberRepository(IConfiguration configuration, IMembersCommandText membersCommandText, IExecuters executers)
        {
            _membersCommandText = membersCommandText;
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("Dapper");
            _executers = executers;
        }

        public MemberDto GetMember(string cardId)
        {
            var member = _executers.ExecuteCommand<Member>(_connStr, conn =>
            conn.Query<Member>(_membersCommandText.GetMemberByCardId, new { @CardId = cardId }).SingleOrDefault());

            return new MemberDto
            {
                Name = member.Name
            };
        }

        public MemberDto AddMember(NewMember member)
        {
            var createdMember = new MemberDto();
            try
            {
                _executers.ExecuteCommand(_connStr, conn =>
                {
                    var addMember= conn.Query<NewMember>(_membersCommandText.AddMember,
                        new { EmployeeId = member.EmployeeId, Name = member.Name, Email = member.Email, Mobile = member.Mobile});

                    var addCard = conn.Query<NewMember>(_membersCommandText.AddCard,
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
