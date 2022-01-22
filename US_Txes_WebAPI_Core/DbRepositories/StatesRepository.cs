using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using US_Txes_WebAPI_Core.DbModels;
using US_Txes_WebAPI_Core.Models;

namespace US_Txes_WebAPI_Core.DbRepositories
{
    public class StatesRepository : IDbRepository<State>
    {
        private readonly CustomDbContext _db;
        private readonly IMapper _mapper;
        public StatesRepository(
            CustomDbContext db
            , IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<State> Create(State entity)
        {
            var stateDb = _db.States.Add(_mapper.Map<State, StateDb>(entity));

            var resultID = await _db.SaveChangesAsync();

            var knowEntity = await _db.States.Include(s => s.ZipCodes).FirstOrDefaultAsync(s => s.StateID == stateDb.Entity.StateID);

            return _mapper.Map<StateDb, State>(knowEntity);
        }

        public async Task<bool> DeleteByID(int id)
        {
            var stateDb = await _db.States.FirstOrDefaultAsync(s => s.StateID == id);

            if (stateDb != null)
            {
                _db.States.Remove(stateDb);

                await _db.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<State> FindByID(int id)
        {
            var stateDb = await _db.States.Include(s => s.ZipCodes).FirstOrDefaultAsync(s => s.StateID == id);

            return _mapper.Map<StateDb, State>(stateDb);
        }

        public async Task<IEnumerable<State>> GetAllEntities()
        {
            var statesDb = await _db.States.Include(s => s.ZipCodes).ToListAsync();

            return _mapper.Map<IEnumerable<StateDb>, IEnumerable<State>>(statesDb);
        }

        public Task<IEnumerable<State>> GetAllEntitiesByParentID(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> IsEntityExists(State entity)
        {
            var stateDb = await _db.States.FirstOrDefaultAsync(s => s.Name.ToLower() == entity.Name.ToLower() || s.Abbreviation.ToLower() == entity.Abbreviation.ToLower());

            return stateDb != null;
        }

        public async Task<State> Update(State entity)
        {
            var knowEntity = await _db.States.Include(s => s.ZipCodes).FirstOrDefaultAsync(s => s.StateID == entity.StateID);

            knowEntity.Name = entity.Name;
            knowEntity.Abbreviation = entity.Abbreviation;

            var resultID = await _db.SaveChangesAsync();

            return _mapper.Map<StateDb, State>(knowEntity);
        }
    }
}
