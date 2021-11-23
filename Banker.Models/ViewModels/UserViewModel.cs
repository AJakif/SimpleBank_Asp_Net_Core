using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Banker.Models.ViewModels
{
    public class UserViewModel
    {
        [Key]
        public int OId { get; set; }
        [Required]
        public string Name { get; set; } 
        public string Address { get; set; }
        public string Gender { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public double Balance { get; set; }
    }
}
