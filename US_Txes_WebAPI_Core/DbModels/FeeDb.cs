using System.ComponentModel.DataAnnotations;

namespace US_Txes_WebAPI_Core.DbModels
{
    public class FeeDb
    {
        [Key]
        public int FeeID { get; set; }
        public int ZipCodeID { get; set; }
        public float Value { get; set; }
    }
}
