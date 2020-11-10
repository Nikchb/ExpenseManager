using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerApp.Models.RecordModels;
using ServerApp.Services.Models;
using ServerApp.Services.RecordService;

namespace ServerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RecordController : CustomControllerBase
    {
        private readonly IRecordService recordService;

        public RecordController(IRecordService recordService)
        {
            this.recordService = recordService ?? throw new ArgumentNullException(nameof(recordService));
        }

        [HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(IEnumerable<RecordModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceError), StatusCodes.Status400BadRequest)]
        public IActionResult Get([FromBody] RecordsFilterModel model)
        {
            var result = recordService.Get(UserId, model);
            if (result.Succeeded)
            {
                return Ok(result.Response);
            }
            return BadRequest(result.Error);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(RecordModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(string id)
        {
            var result = await recordService.Get(UserId, id);
            if (result.Succeeded)
            {
                return Ok(result.Response);
            }
            return BadRequest(result.Error);
        }

        [HttpPost]
        [ProducesResponseType(typeof(RecordModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] CreateRecordModel model)
        {
            var result = await recordService.Create(UserId, model);
            if (result.Succeeded)
            {
                return Ok(result.Response);
            }
            return BadRequest(result.Error);
        }

        [HttpPut]
        [ProducesResponseType(typeof(RecordModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] UpdateRecordModel model)
        {
            var result = await recordService.Update(UserId, model);
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
            var result = await recordService.Delete(UserId, id);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Error);
        }
    }
}
