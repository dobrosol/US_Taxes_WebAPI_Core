using System.Collections.Generic;
using System.Threading.Tasks;
using US_Txes_WebAPI_Core.Enumerables;
using US_Txes_WebAPI_Core.Models;

namespace US_Txes_WebAPI_Core.DbRepositories
{
    public interface ITaxesRepository
    {
        public Task<IEnumerable<Tax>> CalculateAllTaxes();

        public Task<Tax> CalculateTax(string stateAbbreviation, int zipCode, VehicleTypes vehicleType);
    }
}
