using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO.CategoryDtos;
using CodePulse.API.Repository.Implementation;
using CodePulse.API.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }


        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
        {
            //mapping dto to domaing model
            var category = new Category()
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            await _categoryRepository.CreateAsync(category);

            //mapping domain model to Dto response
            var response = new CategoryResponseDto()
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();

            //mapping domain model to dto

            var response = new List<CategoryResponseDto>();
            foreach (var category in categories)
            {
                response.Add(new CategoryResponseDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle
                });
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute]Guid id)
        {
            var categorie = await _categoryRepository.GetByIdAsync(id);

            if (categorie is null)
            {
                return NotFound();
            } 

            var response = new CategoryResponseDto()
            {
                Id = categorie.Id,
                Name = categorie.Name,
                UrlHandle = categorie.UrlHandle
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id,UpdateCategoryRequestDto request)
        {
            //Mapping dto to domain
            var updateCategory = new Category()
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            //saving domain
            updateCategory = await _categoryRepository.UpdateAsync(updateCategory);

            if (updateCategory is null)
            {
                return NotFound();
            }

            //mapping domain to dto
            var response = new CategoryResponseDto()
            {
                Id = updateCategory.Id,
                Name = updateCategory.Name,
                UrlHandle = updateCategory.UrlHandle
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {

            var deleteCategory = await _categoryRepository.DeleteAsync(id);


            if (deleteCategory is null)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
