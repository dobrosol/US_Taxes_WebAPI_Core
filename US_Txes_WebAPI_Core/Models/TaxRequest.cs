using System.ComponentModel.DataAnnotations;
using US_Txes_WebAPI_Core.Enumerables;

namespace US_Txes_WebAPI_Core.Models
{
    public class TaxRequest
    {
        [Required(ErrorMessage = "Abbreviation is empty")]
        [StringLength(2, ErrorMessage = "StateAbbreviation length should equal 2")]
        public string StateAbbreviation { get; set; }
        [Range(1, 99999, ErrorMessage = "ZipCode should be between {1} and {2}")]
        public int ZipCode { get; set; }
        [Range(0, 2, ErrorMessage = "VehicleType should be between 0 and 2. Available types: 0 - Passenger, 1 - Truck, 2 - Trailer")]
        public VehicleTypes VehicleType { get; set; }
    }
}
