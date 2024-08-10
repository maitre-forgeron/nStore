using CatalogService.Api.Extensions;
using CatalogService.Api.Models;
using CatalogService.Application.Dtos;
using CatalogService.Application.Products.Commands;
using CatalogService.Application.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CatalogService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ClientIdPolicy")]
    public class ProductsController : ControllerBase
    {
        private readonly ISender _mediator;

        public ProductsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}", Name = nameof(GetProductAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetProductAsync([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var product = await _mediator.Send(new GetProductQuery(id));

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet(Name = nameof(GetProductsAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetProductsAsync([FromQuery] ProductsQuery query)
        {
            var products = await _mediator.Send(new GetProductsQuery(new GetProductsDto(query.Page, query.Limit, query.CategoryId)));

            Response.AddPaginationHeader(products, nameof(GetProductsAsync), query, Url, true);

            return Ok(products);
        }

        [Authorize(Policy = "ManagerPolicy")]
        [HttpPost(Name = nameof(AddProductAsync))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> AddProductAsync([FromBody] AddProductDto dto)
        {
            if (dto == null || (dto != null && dto.CategoryId < 0))
            {
                return BadRequest();
            }

            var product = await _mediator.Send(new AddProductCommand(dto));

            return CreatedAtRoute(nameof(GetProductAsync), new { id = product.Id }, product);
        }

        [Authorize(Policy = "ManagerPolicy")]
        [HttpPut(Name = nameof(UpdateProductAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateProductAsync([FromBody] UpdateProductDto dto)
        {
            if (dto == null || (dto != null && dto.CategoryId <= 0))
            {
                return BadRequest();
            }

            var productId = await _mediator.Send(new UpdateProductCommand(dto));

            if (productId == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize(Policy = "ManagerPolicy")]
        [HttpDelete("{id}", Name = nameof(DeleteProductAsync))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            await _mediator.Send(new DeleteProductCommand(id));

            return NoContent();
        }
    }
}
