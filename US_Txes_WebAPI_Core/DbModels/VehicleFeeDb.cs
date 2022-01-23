using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using US_Txes_WebAPI_Core.Enumerables;

namespace US_Txes_WebAPI_Core.DbModels
{
    public class VehicleFeeDb
    {
        [Key]
        public int VehicleFeeID { get; set; }
        [ForeignKey("State")]
        public int StateID { get; set; }
        public VehicleTypes Type { get; set; }
        public double? Koefficient { get; set; }
        public bool IsAvailable { get; set; }
        public string Remarks { get; set; }

        public StateDb State { get; set; }
    }
}
