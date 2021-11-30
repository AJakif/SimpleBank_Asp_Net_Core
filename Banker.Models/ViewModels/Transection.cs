using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Banker.Models.ViewModels
{
    public class Transection
    {
        [Key]
        public int OId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Use Characters only")]
        public string Name { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Required]

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        public string Remark { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
