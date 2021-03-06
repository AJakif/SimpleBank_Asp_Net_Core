using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Banker.Models
{
    public class HistoryModel
    {
        [Key]
        public int OId { get; set; }
        public int UserId { get; set; }
        public DateTime DateTime { get; set; }

        public List<HistoryModel> HistoryList { get; set; }

    }
}
