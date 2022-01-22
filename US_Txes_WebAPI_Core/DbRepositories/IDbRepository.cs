using System.Collections.Generic;
using System.Threading.Tasks;

namespace US_Txes_WebAPI_Core.DbRepositories
{
    public interface IDbRepository<T>
    {
        public Task<IEnumerable<T>> GetAllEntities();

        public Task<IEnumerable<T>> GetAllEntitiesByParentID(int id);

        public Task<T> FindByID(int id);

        public Task<T> Create(T entity);

        public Task<T> Update(T entity);

        public Task<bool> DeleteByID(int id);

        public Task<bool> IsEntityExists(T entity);
    }
}
