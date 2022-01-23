using System.ComponentModel.DataAnnotations;

namespace US_Txes_WebAPI_Core.Models
{
    public class Fee
    {
        public int FeeID { get; set; }
        public int ZipCodeID { get; set; }
        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Value should be greaer than 0")]
        public double Value { get; set; }

        public ZipCode ZipCode { get; set; }
    }
}
