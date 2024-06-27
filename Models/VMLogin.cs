using System.ComponentModel.DataAnnotations;

namespace DRS.Models
{
    public class VMLogin
    {
        [Required(ErrorMessage ="Email is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
