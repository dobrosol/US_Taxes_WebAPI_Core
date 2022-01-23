using US_Txes_WebAPI_Core.Enumerables;

namespace US_Txes_WebAPI_Core.Models
{
    public class Tax
    {
        public string StateAbbreviation { get; set; }
        public int ZipCode { get; set; }

        public VehicleTypes VehicleType { get; set; }

        public string Vehicle { get; set; }

        public double? Payment { get; set; }

        public string Remarks { get; set; }
    }
}
