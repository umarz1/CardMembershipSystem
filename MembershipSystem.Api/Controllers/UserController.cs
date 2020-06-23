using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MembershipSystem.Api.DTOs;
using MembershipSystem.Api.Models;
using MembershipSystem.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MembershipSystem.Api.Controllers
{
    [ApiController]
    [Route("")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("users/{cardId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public  ActionResult<UserDto> GetUserByCardId(string cardId)
        {
            var user =  _userRepository.GetUserByCardId(cardId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("users")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult AddUser(User user)
        {
           var response = _userRepository.AddUser(user);

            if(response == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetUserByCardId), new { CardId = user.CardId}, response);
        }

    }
}
