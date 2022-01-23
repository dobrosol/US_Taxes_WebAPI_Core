using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace US_Txes_WebAPI_Core.DbModels
{
    public class FeeDb
    {
        [Key]
        public int FeeID { get; set; }
        [ForeignKey("ZipCode")]
        public int ZipCodeID { get; set; }
        public double Value { get; set; }
        public ZipCodeDb ZipCode { get; set; }
    }
}
