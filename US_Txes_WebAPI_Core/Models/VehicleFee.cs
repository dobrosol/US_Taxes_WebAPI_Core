using System.ComponentModel.DataAnnotations;
using US_Txes_WebAPI_Core.Enumerables;
using US_Txes_WebAPI_Core.Validators;

namespace US_Txes_WebAPI_Core.Models
{
    public class VehicleFee
    {
        public int VehicleFeeID { get; set; }
        public int StateID { get; set; }
        [Range(0, 2, ErrorMessage = "Type should be between 0 and 2. Available types: 0 - Passenger, 1 - Truck, 2 - Trailer")]
        public VehicleTypes Type { get; set; }
        [VehicleFeeKoefficientCustomValidator]
        public double? Koefficient { get; set; }
        public bool IsAvailable { get; set; }
        [VehicleFeeKoefficientRemarksValidator]
        public string Remarks { get; set; }

        public State State { get; set; }
    }
}
