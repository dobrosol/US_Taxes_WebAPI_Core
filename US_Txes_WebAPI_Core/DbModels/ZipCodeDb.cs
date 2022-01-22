using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace US_Txes_WebAPI_Core.DbModels
{
    public class ZipCodeDb
    {
        [Key]
        public int ZipCodeID { get; set; }
        public int Value { get; set; }

        [ForeignKey("State")]
        public int StateID { get; set; }
        public StateDb State { get; set; }
    }
}
