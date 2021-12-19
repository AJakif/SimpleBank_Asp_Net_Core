using System;
using System.Collections.Generic;
using System.Text;

namespace Banker.Models
{
    public class CollectDataModel
    {
        public List<TransactionModel> Transections { get; set; }

        public UserModel User { get; set; }
        public TransactionModel Transection { get; set; }
    }
}
