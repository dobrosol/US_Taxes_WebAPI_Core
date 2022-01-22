using System.ComponentModel.DataAnnotations;
using US_Txes_WebAPI_Core.Enumerables;

namespace US_Txes_WebAPI_Core.DbModels
{
    public class VehicleFeeDb
    {
        [Key]
        public int VehicleFeeID { get; set; }
        public int StateID { get; set; }
        public VehicleTypes VehicleType { get; set; }
        public float FeeKoefficient { get; set; }
        public bool IsAvailable { get; set; }
        public string Remarks { get; set; }
    }
}
