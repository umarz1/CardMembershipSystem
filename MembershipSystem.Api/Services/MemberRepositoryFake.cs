using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security;
using Dapper;
using MembershipSystem.Api.DTOs;
using MembershipSystem.Api.Models;
using MembershipSystem.Api.Services.ExecuteCommands;
using MembershipSystem.Api.Services.Queries;
using Microsoft.Extensions.Configuration;

namespace MembershipSystem.Api.Services
{
    public class MemberRepositoryFake : IMembershipRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ICommandText _commandText;
        private readonly string _connStr;
        private readonly IExecuters _executers;

        public MemberRepositoryFake(IConfiguration configuration, ICommandText commandText, IExecuters executers)
        {
            _commandText = commandText;
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("Dapper");
            _executers = executers;
        }

        public Dictionary<string, Employee> employees = new Dictionary<string, Employee>()
        {
            {"9997", new Employee{ EmployeeId="9997", Name= "Test User1", Email = "test.user1@hotmail.com", Mobile ="07745314222"} },
            {"9998", new Employee{ EmployeeId="9998", Name = "Test User2", Email = "test.user2@hotmail.com", Mobile ="07745314222"} },
            {"9999", new Employee{ EmployeeId="9999", Name = "Test User2", Email = "test.user2@hotmail.com", Mobile ="07745314222"} } 
        };

        public Dictionary<string, Card> cards = new Dictionary<string, Card>()
        {
            {"VyDJ0lbYcPkzp2Ju", new Card{ CardId ="VyDJ0lbYcPkzp2Ju",  EmployeeId="9997", Pin= "1234"} },
            {"2sJ6fY4lYJaZFX9w", new Card{ CardId ="2sJ6fY4lYJaZFX9w",  EmployeeId="9998", Pin= "1235"} },
            {"Bl5vKfTP7Q7jW4PH", new Card{ CardId ="Bl5vKfTP7Q7jW4PH",  EmployeeId="9999", Pin= "1236"} },
        };

        public EmployeeDto GetMember(string cardId)
        {
            var card = cards.Where(c => c.Key == cardId).FirstOrDefault();

            if(card.Value == null)
            {
                return null;
            }

            var emp = employees.Where(e => e.Value.EmployeeId == card.Value.EmployeeId).FirstOrDefault();

            return new EmployeeDto { Name = emp.Value.Name };
        }

        public EmployeeDto AddMember(Member user)
        {
            var employee = new Employee()
            {
                EmployeeId = user.EmployeeId,
                Name = user.Name,
                Email = user.Email,
                Mobile = user.Mobile
            };

            var card = new Card()
            {
                CardId = user.CardId,
                EmployeeId = user.EmployeeId,
                Pin = user.Pin
            };

            if (employees.TryAdd(user.EmployeeId, employee) && cards.TryAdd(user.CardId, card))
            {
                return new EmployeeDto { Name = employee.Name };
            }
            else
            {
                return null;
            }
        }

        //public UserDto AddUser(User user)
        //{
        //    var createdUser = users.Where(u => u.CardId == user.CardId).FirstOrDefault();

        //    if (createdUser != null)
        //    {
        //        return null;

        //    return createdUser;
        //}
    }
}
