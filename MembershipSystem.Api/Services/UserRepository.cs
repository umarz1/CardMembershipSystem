using System.Linq;
using Dapper;
using MembershipSystem.Api.Models;
using MembershipSystem.Api.Services.ExecuteCommands;
using MembershipSystem.Api.Services.Queries;
using Microsoft.Extensions.Configuration;

namespace MembershipSystem.Api.Services
{
    public class UserRepository: IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ICommandText _commandText;
        private readonly string _connStr;
        private readonly IExecuters _executers;

        public UserRepository(IConfiguration configuration, ICommandText commandText, IExecuters executers)
        {
            _commandText = commandText;
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("Dapper");
            _executers = executers;
        }

        public User GetUserByCardId(string cardId)
        {
            var user = _executers.ExecuteCommand<User>(_connStr, conn =>
            conn.Query<User>(_commandText.GetUserByCardId, new { @CardId = cardId }).SingleOrDefault());
            return user;
        }

        public void AddUser(User user)
        {
            _executers.ExecuteCommand(_connStr, conn =>
            {
                var query = conn.Query<User>(_commandText.AddUser,
                    new { EmployeeId = user.EmployeeId,  Name = user.Name, Email = user.Email, Mobile = user.Mobile });
            });
        }
    }
}
