using MembershipSystem.Api.DTOs;
using MembershipSystem.Api.Models;
using MembershipSystem.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MembershipSystem.Api.Controllers
{
    [ApiController]
    [Route("")]
    public class EmployeeController : ControllerBase
    {
        private readonly IMembershipRepository _userRepository;

        public EmployeeController(IMembershipRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("members/{cardId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<EmployeeDto> GetMember(string cardId)
        {
            var member = _userRepository.GetMember(cardId);

            if (member == null)
            {
                return NotFound();
            }

            return Ok(member);
        }

        [HttpPost("members")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult AddMember([FromBody]Member member)
        {
            var response = _userRepository.AddMember(member);

            if (response == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetMember), new { CardId = member.CardId }, response);
        }

    }
}
