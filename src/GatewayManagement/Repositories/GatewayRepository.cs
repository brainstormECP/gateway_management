using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GatewayManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GatewayManagement.Repositories
{
    public class GatewayRepository : BaseRepository<Gateway>
    {
        public GatewayRepository(DbContext db) : base(db)
        {
        }

        public override async Task<IEnumerable<Gateway>> FindAll()
        {
            return await _db.Set<Gateway>().Include(g => g.Devices).ToListAsync();
        }

        public override async Task<Gateway> FindById(int id)
        {
            return await _db.Set<Gateway>().Include(g => g.Devices).SingleOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Gateway> FindBySerialNumber(string serialNumber)
        {
            return await _db.Set<Gateway>().Include(g => g.Devices).SingleOrDefaultAsync(g => g.SerialNumber == serialNumber);
        }

        public override async Task<Result> Create(Gateway gateway)
        {
            try
            {
                if (gateway == null)
                {
                    return new Result { Status = false, Detail = "Not gateway recived." };
                }
                if (await SerialNumberExists(gateway.SerialNumber))
                {
                    return new Result { Status = false, Detail = "Already exists a Gateway with this serial number." };
                }
                if (gateway.Devices.Count > 10)
                {
                    return new Result { Status = false, Detail = "A Gateway can't have more than 10 Devices." };
                }
                _db.Set<Gateway>().Add(gateway);
                await _db.SaveChangesAsync();
                return new Result { Status = true, Entity = gateway };
            }
            catch (DbUpdateException ex)
            {
                return new Result { Status = false, Detail = ex.Message };
            }
            catch (Exception ex)
            {
                return new Result { Status = false, Detail = ex.Message };
            }
        }

        public override async Task<Result> Update(Gateway gateway)
        {
            _db.Entry(gateway).State = EntityState.Modified;
            var existSerial = await SerialNumberExists(gateway.Id, gateway.SerialNumber);
            if (existSerial)
            {
                return new Result{Status=false, Detail="A gateway with this serial number exist."};
            }
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new Result { Status = false, Detail = ex.Message };
            }
            return new Result { Status = true, Entity=gateway };
        }

        public override async Task<Result> Delete(int id)
        {
            var gateway = await _db.Set<Gateway>().FindAsync(id);
            if (gateway == null)
            {
                return new Result { Status = false, Detail = "Gateway not Found." };
            }
            _db.Set<Gateway>().Remove(gateway);
            await _db.SaveChangesAsync();
            return new Result { Status = true };
        }

        public async Task<bool> SerialNumberExists(string serialNumber)
        {
            return await _db.Set<Gateway>().AnyAsync(g => g.SerialNumber == serialNumber);
        }

        public async Task<bool> SerialNumberExists(int id, string serialNumber)
        {
            var exist = await SerialNumberExists(serialNumber);
            if (exist)
            {
                var gateway = await FindBySerialNumber(serialNumber);
                if (gateway.Id != id)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        protected override bool Exists(int id)
        {
            return _db.Set<Gateway>().Any(g => g.Id == id);
        }
    }
}
