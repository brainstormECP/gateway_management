using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GatewayManagement.Models;
using GatewayManagement.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GatewayManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GatewayController : ControllerBase
    {
        private readonly ILogger<GatewayController> _logger;

        private GatewayRepository _repo { get; set; }

        public GatewayController(GatewayRepository repo, ILogger<GatewayController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Gateway>> Get()
        {
            return await _repo.FindAll();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var gateway = await _repo.FindById(id.Value);
            if (gateway == null)
            {
                return NotFound();
            }
            return Ok(gateway);
        }

        [HttpGet("CheckSerial/{serialNumber}")]
        public async Task<IActionResult> CheckSerialNumber(string serialNumber)
        {
            if (serialNumber == null)
            {
                return BadRequest();
            }
            return Ok(await _repo.SerialNumberExists(0, serialNumber));
        }
        [HttpPost]
        public async Task<ActionResult<Gateway>> Post(Gateway gateway)
        {
            try
            {
                if (gateway == null)
                {
                    return BadRequest();
                }
                var result = await _repo.Create(gateway);
                if (result.Status)
                {
                    return CreatedAtAction(nameof(Get), new { id = gateway.Id }, gateway);
                }
                else
                {
                    return BadRequest(result.Detail);
                }
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Error saving the Gateway. " + ex.Message);
            }
            catch(Exception ex){
                return BadRequest("Error saving the Gateway. " + ex.Message);
            }

        }

        // PUT: api/TodoItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Gateway gateway)
        {
            if (id != gateway.Id)
            {
                return BadRequest();
            }
            var result = await _repo.Update(gateway);
            if (result.Status)
            {
                return Ok();
            }
            return BadRequest(result.Detail);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Gateway>> Delete(int id)
        {
            var result = await _repo.Delete(id);
            if (result.Status)
            {
                return Ok();
            }
            return BadRequest(result.Detail);
        }
    }
}
