using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace gateway_management.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly ILogger<DevicesController> _logger;
        private DbContext _db { get; set; }

        public DevicesController(DbContext db, ILogger<DevicesController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Device>> Get()
        {
            return await _db.Set<Device>().ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var device = await _db.Set<Device>().FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }
            return Ok(device);
        }
        [HttpPost]
        public async Task<ActionResult<Device>> Post(Device device)
        {
            _db.Set<Device>().Add(device);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = device.Id }, device);
        }

        // PUT: api/TodoItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(Guid id, Device device)
        {
            if (id != device.Id)
            {
                return BadRequest();
            }

            _db.Entry(device).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(id))
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
        public async Task<ActionResult<Device>> Delete(Guid id)
        {
            var device = await _db.Set<Device>().FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }
            _db.Set<Device>().Remove(device);
            await _db.SaveChangesAsync();
            return device;
        }

        private bool DeviceExists(Guid id)
        {
            return _db.Set<Device>().Any(g => g.Id == id);
        }
    }
}
