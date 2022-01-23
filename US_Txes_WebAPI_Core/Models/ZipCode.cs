using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace US_Txes_WebAPI_Core.Models
{
    public class ZipCode
    {
        public int ZipCodeID { get; set; }
        [Range(1, 99999, ErrorMessage = "Value should be between {1} and {2}")]
        public int Value { get; set; }
        public int StateID { get; set; }

        public State State { get; set; }

        public Fee Fee { get; set; }
    }
}
