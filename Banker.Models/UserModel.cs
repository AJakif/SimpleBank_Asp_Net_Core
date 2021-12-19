using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Banker.Models
{
    public class UserModel
    {
        [Key]
        public int OId { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Use Characters only")]
        public string Name { get; set; } 
        public string Address { get; set; }
        public string Gender { get; set; }

        public string Role { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public decimal Balance { get; set; }
    }
}
