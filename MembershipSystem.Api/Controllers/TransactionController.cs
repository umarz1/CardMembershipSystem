using MembershipSystem.Api.DTOs;
using MembershipSystem.Api.Models;
using MembershipSystem.Api.Requests;
using MembershipSystem.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace MembershipSystem.Api.Controllers
{
    [ApiController]
    [Route("")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("transactions/balance/{cardId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BalanceDto> GetLatestBalance(string cardId)
        {
            var balance = _transactionService.GetBalance(cardId);

            if (balance == null)
            {
                return NotFound("Please top up to initialize card");
            }

            return Ok(balance);
        }

        [HttpPost("transactions/amend-amount")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult AddAmount([FromBody]AdjustAmount adjustAmount)
        {
            if(adjustAmount.Amount < 0)
            {
                return BadRequest("Amount must greater than zero");
            }

            var response = _transactionService.AddAmount(adjustAmount);

            if (response == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetLatestBalance), new { CardId = adjustAmount.CardId }, response);
        }
    }
}
