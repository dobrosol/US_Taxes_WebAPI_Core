using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using US_Txes_WebAPI_Core.DbModels;
using US_Txes_WebAPI_Core.Models;

namespace US_Txes_WebAPI_Core.DbRepositories
{
    public class VehicleFeesRepository : IDbEntityRepository<VehicleFee>
    {
        private readonly CustomDbContext _db;
        private readonly IMapper _mapper;
        public VehicleFeesRepository(
            CustomDbContext db
            , IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<VehicleFee> Create(VehicleFee entity)
        {
            if (!entity.IsAvailable)
            {
                entity.Koefficient = null;
            }
            else
            {
                entity.Remarks = null;
            }

            var VehicleFeeDb = _db.VehicleFees.Add(_mapper.Map<VehicleFee, VehicleFeeDb>(entity));

            var resultID = await _db.SaveChangesAsync();

            var knowEntity = await _db.VehicleFees.Include(s => s.State).FirstOrDefaultAsync(s => s.VehicleFeeID == VehicleFeeDb.Entity.VehicleFeeID);

            return _mapper.Map<VehicleFeeDb, VehicleFee>(knowEntity);
        }

        public async Task<bool> DeleteByID(int id)
        {
            var VehicleFeeDb = await _db.VehicleFees.FirstOrDefaultAsync(s => s.VehicleFeeID == id);

            if (VehicleFeeDb != null)
            {
                _db.VehicleFees.Remove(VehicleFeeDb);

                await _db.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<VehicleFee> FindByID(int id)
        {
            var VehicleFeeDb = await _db.VehicleFees.Include(s => s.State).FirstOrDefaultAsync(s => s.VehicleFeeID == id);

            return _mapper.Map<VehicleFeeDb, VehicleFee>(VehicleFeeDb);
        }

        public async Task<IEnumerable<VehicleFee>> GetAllEntities()
        {
            var VehicleFeesDb = await _db.VehicleFees.Include(s => s.State).ToListAsync();

            return _mapper.Map<IEnumerable<VehicleFeeDb>, IEnumerable<VehicleFee>>(VehicleFeesDb);
        }

        public async Task<IEnumerable<VehicleFee>> GetAllEntitiesByParentID(int id)
        {
            var vehicleFeesDb = await _db.VehicleFees.Where(zc => zc.StateID == id).ToListAsync();

            return _mapper.Map<IEnumerable<VehicleFeeDb>, IEnumerable<VehicleFee>>(vehicleFeesDb);
        }

        public async Task<bool> IsEntityExists(VehicleFee entity)
        {
            var VehicleFeeDb = await _db.VehicleFees.FirstOrDefaultAsync(s => s.StateID == entity.StateID && s.Type == entity.Type);

            return VehicleFeeDb != null;
        }

        public async Task<VehicleFee> Update(VehicleFee entity)
        {
            var knowEntity = await _db.VehicleFees.Include(s => s.State).FirstOrDefaultAsync(s => s.VehicleFeeID == entity.VehicleFeeID);

            knowEntity.Koefficient = !entity.IsAvailable ? null : entity.Koefficient;
            knowEntity.Type = entity.Type;
            knowEntity.IsAvailable = entity.IsAvailable;
            knowEntity.Remarks = !entity.IsAvailable ? entity.Remarks : null;

            var resultID = await _db.SaveChangesAsync();

            return _mapper.Map<VehicleFeeDb, VehicleFee>(knowEntity);
        }
    }
}
