using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Banker.Models
{
    public class AuditModel
    {

        [Required]
        public int OId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string TransId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public String Date { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Source { get; set; }

        [Required]
        public string TransactionType { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string LogType { get; set; }

        public List<AuditModel> AuditList { get; set; }
    }
}
