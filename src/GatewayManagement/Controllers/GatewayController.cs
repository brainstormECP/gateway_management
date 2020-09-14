using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GatewayManagement.Models;
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

        private DbContext _db { get; set; }

        public GatewayController(DbContext db, ILogger<GatewayController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Gateway>> Get()
        {
            return await _db.Set<Gateway>().ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var gateway = await _db.Set<Gateway>().FindAsync(id);
            if (gateway == null)
            {
                return NotFound();
            }
            return Ok(gateway);
        }

        [HttpGet("{serialNumber}")]
        public async Task<IActionResult> CheckSerialNumber(string serialNumber)
        {
            if (serialNumber == null)
            {
                return BadRequest();
            }
            return Ok(await SerialNumberExists(serialNumber));
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
                if (await SerialNumberExists(gateway.SerialNumber))
                {
                    return BadRequest("Already exists a Gateway with this serial number.");
                }
                _db.Set<Gateway>().Add(gateway);
                await _db.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { id = gateway.Id }, gateway);
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
        public async Task<IActionResult> Put(Guid id, Gateway gateway)
        {
            if (id != gateway.Id)
            {
                return BadRequest();
            }

            _db.Entry(gateway).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GatewayExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Gateway>> Delete(Guid id)
        {
            var gateway = await _db.Set<Gateway>().FindAsync(id);
            if (gateway == null)
            {
                return NotFound();
            }
            _db.Set<Gateway>().Remove(gateway);
            await _db.SaveChangesAsync();
            return gateway;
        }
        private async Task<bool> SerialNumberExists(string serialNumber)
        {
            return await _db.Set<Gateway>().AnyAsync(g => g.SerialNumber == serialNumber);
        }

        private bool GatewayExists(Guid id)
        {
            return _db.Set<Gateway>().Any(g => g.Id == id);
        }
    }
}
