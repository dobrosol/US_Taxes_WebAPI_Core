using System.ComponentModel.DataAnnotations;

namespace US_Txes_WebAPI_Core.Models
{
    public class ZipCode
    {
        public int ZipCodeID { get; set; }
        [Required(ErrorMessage = "Value is empty")]
        [Range(1, 99999, ErrorMessage = "Value should be between {0} and {1}")]
        public int Value { get; set; }
        [Required(ErrorMessage = "StateID is empty")]
        public int StateID { get; set; }

        public State State { get; set; }
    }
}
