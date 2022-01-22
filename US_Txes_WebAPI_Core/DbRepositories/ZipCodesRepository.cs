using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using US_Txes_WebAPI_Core.DbModels;
using US_Txes_WebAPI_Core.Models;

namespace US_Txes_WebAPI_Core.DbRepositories
{
    public class ZipCodesRepository : IDbRepository<ZipCode>
    {
        private readonly CustomDbContext _db;
        private readonly IMapper _mapper;
        public ZipCodesRepository(
            CustomDbContext db
            , IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<ZipCode> Create(ZipCode entity)
        {
            var zipCodeDb = _db.ZipCodes.Add(_mapper.Map<ZipCode, ZipCodeDb>(entity));

            var resultID = await _db.SaveChangesAsync();

            var knowEntity = await _db.ZipCodes.Include(zc => zc.State).FirstOrDefaultAsync(s => s.ZipCodeID == zipCodeDb.Entity.ZipCodeID);

            return _mapper.Map<ZipCodeDb, ZipCode>(knowEntity);
        }

        public async Task<bool> DeleteByID(int id)
        {
            var zipCodeDb = await _db.ZipCodes.FirstOrDefaultAsync(s => s.ZipCodeID == id);

            if (zipCodeDb != null)
            {
                _db.ZipCodes.Remove(zipCodeDb);

                await _db.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<ZipCode> FindByID(int id)
        {
            var zipCodeDb = await _db.ZipCodes.Include(zc => zc.State).FirstOrDefaultAsync(s => s.ZipCodeID == id);

            return _mapper.Map<ZipCodeDb, ZipCode>(zipCodeDb);
        }

        public async Task<IEnumerable<ZipCode>> GetAllEntities()
        {
            var zipCodeDb = await _db.ZipCodes.Include(zc => zc.State).ToListAsync();

            return _mapper.Map<IEnumerable<ZipCodeDb>, IEnumerable<ZipCode>>(zipCodeDb);
        }

        public async Task<IEnumerable<ZipCode>> GetAllEntitiesByParentID(int id)
        {
            var zipCodeDb = await _db.ZipCodes.Where(zc => zc.StateID == id).ToListAsync();

            return _mapper.Map<IEnumerable<ZipCodeDb>, IEnumerable<ZipCode>>(zipCodeDb);
        }

        public async Task<bool> IsEntityExists(ZipCode entity)
        {
            var ZipCodeDb = await _db.ZipCodes.FirstOrDefaultAsync(s => s.Value == entity.Value);

            return ZipCodeDb != null;
        }

        public async Task<ZipCode> Update(ZipCode entity)
        {
            var knowEntity = await _db.ZipCodes.Include(zc => zc.State).FirstOrDefaultAsync(s => s.ZipCodeID == entity.ZipCodeID);

            knowEntity.Value = entity.Value;
            knowEntity.StateID = entity.StateID;

            var resultID = await _db.SaveChangesAsync();

            return _mapper.Map<ZipCodeDb, ZipCode>(knowEntity);
        }
    }
}
