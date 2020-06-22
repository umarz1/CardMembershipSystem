using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MembershipSystem.Api.Models;
using MembershipSystem.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace MembershipSystem.Api.Controllers
{
    [ApiController]
    [Route("")]
    public class CardUserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public CardUserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("users/{cardId}")]
        public ActionResult<User> GetUserByCardId(string cardId)
        {
            var user = _userRepository.GetUserByCardId(cardId);
            return Ok(user);
        }

        [HttpPost("users")]
        public ActionResult AddUser(User user)
        {
            _userRepository.AddUser(user);
            return Ok(user);
        }

    }
}
