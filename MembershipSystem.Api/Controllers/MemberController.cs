using MembershipSystem.Api.DTOs;
using MembershipSystem.Api.Models;
using MembershipSystem.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MembershipSystem.Api.Controllers
{
    [ApiController]
    [Route("")]
    public class MemberController : ControllerBase
    {
        private readonly IMembershipRepository _membershipRepository;

        public MemberController(IMembershipRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }

        [HttpGet("members/{cardId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<MemberDto> GetMember(string cardId)
        {
            var member = _membershipRepository.GetMember(cardId);

            if (member == null)
            {
                return NotFound();
            }

            return Ok(member);
        }

        [HttpPost("members")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult AddMember([FromBody]NewMember member)
        {
            var response = _membershipRepository.AddMember(member);

            if (response == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetMember), new { CardId = member.CardId }, response);
        }

    }
}
