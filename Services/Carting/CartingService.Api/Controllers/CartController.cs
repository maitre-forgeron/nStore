using Asp.Versioning;
using CartingService.Application.Carts.Commands;
using CartingService.Application.Carts.Queries;
using CartingService.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NStore.Shared.Extensions;

namespace CartingService.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Policy = "ClientIdPolicy")]
    public class CartController : ControllerBase
    {
        private readonly ISender _mediator;

        public CartController(ISender mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves object with cartId and related items
        /// </summary>
        /// <param name="oId">Identifier type of UUID</param>
        /// <returns>Cart object</returns>
        [HttpGet("{oId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetItemsAsync(string oId)
        {
            if (string.IsNullOrWhiteSpace(oId) || !oId.IsUuid())
            {
                return BadRequest();
            }

            var response = await _mediator.Send(new GetItemsQuery(oId));

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        /// <summary>
        /// Adds item to cart
        /// </summary>
        /// <param name="request">Object containing item model and cartId(UUID)</param>
        /// <returns>Item</returns>
        [HttpPost(Name = nameof(AddItemToCartAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> AddItemToCartAsync([FromBody] AddItemRequest request)
        {
            if (!request.Oid.IsUuid() || request.Item.Id <= 0)
            {
                return BadRequest();
            }

            var item = await _mediator.Send(new AddItemToCartCommand(request));

            return Ok(item);
        }


        /// <summary>
        /// Removes item from the cart
        /// </summary>
        /// <param name="oId">Cart identifier type of UUID</param>
        /// <param name="itemId">Item identifier type of integer</param>
        [HttpDelete("{oId}/{itemId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult RemoveItemFromCart(string oId, int itemId)
        {
            if (string.IsNullOrWhiteSpace(oId) || itemId <= 0)
            {
                return BadRequest();
            }

            _mediator.Send(new RemoveItemFromCartCommand(oId, itemId));

            return NoContent();
        }
    }
}
