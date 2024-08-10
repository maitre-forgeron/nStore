using CatalogService.Application.Categories.Commands;
using CatalogService.Application.Categories.Queries;
using CatalogService.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ClientIdPolicy")]
    public class CategoryController : ControllerBase
    {
        private readonly ISender _mediator;

        public CategoryController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = nameof(GetCategoriesAsync))]
        public async Task<ActionResult> GetCategoriesAsync()
        {
            var categories = await _mediator.Send(new GetCategoriesQuery());

            return Ok(categories);
        }

        [HttpGet("{id}", Name = nameof(GetCategoryAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetCategoryAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var category = await _mediator.Send(new GetCategoryQuery(id));

            if(category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [Authorize(Policy = "ManagerPolicy")]
        [HttpPost(Name = nameof(AddCategoryAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> AddCategoryAsync([FromBody] AddCategoryDto dto)
        {
            if (dto == null || (dto != null && dto.ParentId.HasValue && dto.ParentId.Value < 0))
            {
                return BadRequest();
            }

            var categoryId = await _mediator.Send(new AddCategoryCommand(dto));

            return Ok(categoryId);
        }

        [Authorize(Policy = "ManagerPolicy")]
        [HttpPut(Name = nameof(UpdateCategoryAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateCategoryAsync([FromBody] UpdateCategoryDto dto)
        {
            if (dto == null || (dto != null && dto.ParentId.HasValue && dto.ParentId.Value < 0))
            {
                return BadRequest();
            }

            var categoryId = await _mediator.Send(new UpdateCategoryCommand(dto));

            if (categoryId == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize(Policy = "ManagerPolicy")]
        [HttpDelete("{id}", Name = nameof(DeleteCategoryWithRelatedProductsAsync))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteCategoryWithRelatedProductsAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            await _mediator.Send(new DeleteCategoryCommand(id));

            return NoContent();
        }
    }
}
