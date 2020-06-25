using MembershipSystem.Api.DTOs;
using MembershipSystem.Api.Models;
using MembershipSystem.Api.Requests;
using MembershipSystem.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MembershipSystem.Api.Controllers
{
    [ApiController]
    [Route("")]
    public class CardTransactionController : ControllerBase
    {
        private readonly ICardTransactionsRepository _cardTransactionsRepository;

        public CardTransactionController(ICardTransactionsRepository cardTransactionsRepository)
        {
            _cardTransactionsRepository = cardTransactionsRepository;
        }

        [HttpGet("card-transactions/{cardId}/balance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CardTransactionDto> GetLatestBalance(string cardId)
        {
            var balance = _cardTransactionsRepository.GetBalance(cardId);

            if (balance == null)
            {
                return NotFound();
            }

            return Ok(balance);
        }

        [HttpPost("card-transactions/add-amount")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult AddAmount([FromBody]AdjustAmount adjustAmount)
        {
            var response = _cardTransactionsRepository.AddAmount(adjustAmount);

            if (response == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetLatestBalance), new { CardId = adjustAmount.CardId }, response);
        }
    }
}
