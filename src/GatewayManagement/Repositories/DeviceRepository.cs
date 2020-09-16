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
    public class DeviceRepository : BaseRepository<Device>
    {
        public DeviceRepository(DbContext db) : base(db)
        {
        }

        public override async Task<IEnumerable<Device>> FindAll()
        {
            return await _db.Set<Device>().ToListAsync();
        }

        public override async Task<Device> FindById(int id)
        {
            return await _db.Set<Device>().FindAsync(id);
        }

        public async Task<Device> FindByUID(int uid)
        {
            return await _db.Set<Device>().SingleOrDefaultAsync(g => g.UID == uid);
        }

        public override async Task<Result> Create(Device device)
        {
            try
            {
                if (device == null)
                {
                    return new Result { Status = false, Detail = "Not device recived." };
                }
                if (await ExistsUID(device.UID))
                {
                    return new Result { Status = false, Detail = "Already exists a Device with this UID." };
                }
                var gateway = await _db.Set<Gateway>().Include(g => g.Devices).SingleOrDefaultAsync(g => g.Id == device.GatewayId);
                if (gateway.Devices.Count == 10)
                {
                    return new Result{Status=false, Detail="A Gateway can't have more than 10 Devices."};
                }
                _db.Set<Device>().Add(device);
                await _db.SaveChangesAsync();
                return new Result { Status = true, Entity = device };
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

        public override async Task<Result> Update(Device device)
        {
            _db.Entry(device).State = EntityState.Modified;
            var existSerial = await ExistsUID(device.Id, device.UID);
            if (existSerial)
            {
                return new Result{Status=false, Detail="Already exists a Device with this UID."};
            }
            try
            {
                var gateway = await _db.Set<Gateway>().Include(g => g.Devices).SingleOrDefaultAsync(g => g.Id == device.GatewayId);
                if (gateway.Devices.Count >= 10)
                {
                    return new Result{Status=false, Detail="A Gateway can't have more than 10 Devices."};
                }
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new Result { Status = false, Detail = ex.Message };
            }
            return new Result { Status = true, Entity=device };
        }

        public override async Task<Result> Delete(int id)
        {
            var device = await _db.Set<Device>().FindAsync(id);
            if (device == null)
            {
                return new Result { Status = false, Detail = "Device not Found." };
            }
            _db.Set<Device>().Remove(device);
            await _db.SaveChangesAsync();
            return new Result { Status = true };
        }

        public async Task<bool> ExistsUID(int uid)
        {
            return await _db.Set<Device>().AnyAsync(g => g.UID == uid);
        }

        public async Task<bool> ExistsUID(int id, int uid)
        {
            var exist = await ExistsUID(uid);
            if (exist)
            {
                var device = await FindByUID(uid);
                if (device.Id != id)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        protected override bool Exists(int id)
        {
            return _db.Set<Device>().Any(g => g.Id == id);
        }
    }
}
