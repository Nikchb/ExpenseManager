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
        public async Task<IActionResult> Get()
        {
            var result = await categoryService.Get(UserId);
            if (result.Succeeded)
            {
                return Ok(result.Response);
            }
            return BadRequest(result.Response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await categoryService.Get(UserId, id);
            if (result.Succeeded)
            {
                return Ok(result.Response);
            }
            return BadRequest(result.Response);
        }

        [HttpPost]        
        public async Task<IActionResult> Post([FromBody] CreateCategoryModel model)
        {
            var result = await categoryService.Create(UserId, model);
            if (result.Succeeded)
            {
                return Ok(result.Response);
            }
            return BadRequest(result.Response);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CategoryModel model)
        {
            var result = await categoryService.Update(UserId, model);
            if (result.Succeeded)
            {
                return Ok(result.Response);
            }
            return BadRequest(result.Response);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await categoryService.Delete(UserId, id);
            if (result.Succeeded)
            {
                return Ok(result.Response);
            }
            return BadRequest(result.Response);
        }
    }
}
