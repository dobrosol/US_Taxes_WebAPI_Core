using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using US_Txes_WebAPI_Core.DbModels;
using US_Txes_WebAPI_Core.Models;

namespace US_Txes_WebAPI_Core.DbRepositories
{
    public class FeesRepository : IDbEntityRepository<Fee>
    {
        private readonly CustomDbContext _db;
        private readonly IMapper _mapper;
        public FeesRepository(
            CustomDbContext db
            , IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<Fee> Create(Fee entity)
        {
            var FeeDb = _db.Fees.Add(_mapper.Map<Fee, FeeDb>(entity));

            var resultID = await _db.SaveChangesAsync();

            var knowEntity = await _db.Fees.Include(s => s.ZipCode).ThenInclude(zc => zc.State).FirstOrDefaultAsync(s => s.FeeID == FeeDb.Entity.FeeID);

            return _mapper.Map<FeeDb, Fee>(knowEntity);
        }

        public async Task<bool> DeleteByID(int id)
        {
            var FeeDb = await _db.Fees.FirstOrDefaultAsync(s => s.FeeID == id);

            if (FeeDb != null)
            {
                _db.Fees.Remove(FeeDb);

                await _db.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<Fee> FindByID(int id)
        {
            var FeeDb = await _db.Fees.Include(s => s.ZipCode).ThenInclude(zc => zc.State).FirstOrDefaultAsync(s => s.FeeID == id);

            return _mapper.Map<FeeDb, Fee>(FeeDb);
        }

        public async Task<IEnumerable<Fee>> GetAllEntities()
        {
            var FeesDb = await _db.Fees.Include(s => s.ZipCode).ThenInclude(zc => zc.State).ToListAsync();

            return _mapper.Map<IEnumerable<FeeDb>, IEnumerable<Fee>>(FeesDb);
        }

        public async Task<IEnumerable<Fee>> GetAllEntitiesByParentID(int id)
        {
            var feesDb = await _db.Fees.Where(zc => zc.ZipCodeID == id).ToListAsync();

            return _mapper.Map<IEnumerable<FeeDb>, IEnumerable<Fee>>(feesDb);
        }

        public async Task<bool> IsEntityExists(Fee entity)
        {
            var FeeDb = await _db.Fees.FirstOrDefaultAsync(s => s.ZipCodeID == entity.ZipCodeID);

            return FeeDb != null;
        }

        public async Task<Fee> Update(Fee entity)
        {
            var knowEntity = await _db.Fees
                .Include(s => s.ZipCode)
                .ThenInclude(zc => zc.State)
                .FirstOrDefaultAsync(s => s.FeeID == entity.FeeID);

            knowEntity.Value = entity.Value;
            knowEntity.ZipCodeID = entity.ZipCodeID;

            var resultID = await _db.SaveChangesAsync();

            return _mapper.Map<FeeDb, Fee>(knowEntity);
        }
    }
}
