using System.Collections.Generic;
using System.Linq;
using Dapper;
using MembershipSystem.Api.DTOs;
using MembershipSystem.Api.Models;
using MembershipSystem.Api.Services.ExecuteCommands;
using MembershipSystem.Api.Services.Queries;
using Microsoft.Extensions.Configuration;

namespace MembershipSystem.Api.Services
{
    public class UserRepositoryFake: IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ICommandText _commandText;
        private readonly string _connStr;
        private readonly IExecuters _executers;

        public UserRepositoryFake(IConfiguration configuration, ICommandText commandText, IExecuters executers)
        {
            _commandText = commandText;
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("Dapper");
            _executers = executers;
        }

        public List<UserDto> users = new List<UserDto>()
        {
            new UserDto
            {
                CardId = "ByDJ0lbYcPkzp2Ja",
                Name ="Test User",
            }
        };

        public UserDto AddUser(User user)
        {
            throw new System.NotImplementedException();
        }

        public UserDto GetUserByCardId(string cardId)
        {
            var user = users.Where(u => u.CardId == cardId).FirstOrDefault();

            if(user == null)
            {
                return null;
            }

            return user;
        }

        //public void AddUser(User user)
        //{
        //    _executers.ExecuteCommand(_connStr, conn =>
        //    {
        //        var query = conn.Query<User>(_commandText.AddUser,
        //            new { EmployeeId = user.EmployeeId,  Name = user.Name, Email = user.Email, Mobile = user.Mobile });
        //    });
        //}
    }
}
