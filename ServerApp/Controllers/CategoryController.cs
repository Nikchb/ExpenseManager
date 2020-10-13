using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServerApp.Data;
using ServerApp.Data.Models;
using ServerApp.Models;
using ServerApp.Services.CategoryService;
using ServerApp.Services.Models;

namespace ServerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : CustomControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(IEnumerable<CategoryModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceError), StatusCodes.Status400BadRequest)]
        public IActionResult Get()
        {
            var result = categoryService.Get(UserId);
            if (result.Succeeded)
            {
                return Ok(result.Response);
            }
            return BadRequest(result.Error);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(CategoryModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(string id)
        {
            var result = await categoryService.Get(UserId, id);
            if (result.Succeeded)
            {
                return Ok(result.Response);
            }
            return BadRequest(result.Error);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CategoryModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] CreateCategoryModel model)
        {
            var result = await categoryService.Create(UserId, model);
            if (result.Succeeded)
            {
                return Ok(result.Response);
            }
            return BadRequest(result.Error);
        }

        [HttpPut]
        [ProducesResponseType(typeof(CategoryModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] CategoryModel model)
        {
            var result = await categoryService.Update(UserId, model);
            if (result.Succeeded)
            {
                return Ok(result.Response);
            }
            return BadRequest(result.Error);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await categoryService.Delete(UserId, id);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Error);
        }
    }
}
