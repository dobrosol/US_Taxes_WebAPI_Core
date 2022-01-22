using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace US_Txes_WebAPI_Core.Models
{
    public class State
    {
        public int StateID { get; set; }
        [Required(ErrorMessage = "Abbreviation is empty")]
        [StringLength(2, ErrorMessage = "Abbreviation length should equal 2")]
        public string Abbreviation { get; set; }
        [Required(ErrorMessage = "Name is empty")]
        public string Name { get; set; }

        public IEnumerable<ZipCode> ZipCodes { get; set; }
    }
}
