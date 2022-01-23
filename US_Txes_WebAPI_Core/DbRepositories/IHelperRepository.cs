using System.Threading.Tasks;

namespace US_Txes_WebAPI_Core.DbRepositories
{
    public interface IHelperRepository
    {
        public Task InitializeData();
    }
}
