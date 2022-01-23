using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace US_Txes_WebAPI_Core.DbModels
{
    public class StateDb
    {
        [Key]
        public int StateID { get; set; }
        public string Abbreviation { get; set; }
        public string Name { get; set; }

        public ICollection<ZipCodeDb> ZipCodes { get; set; }

        public ICollection<VehicleFeeDb> VehicleFees { get; set; }
    }
}
