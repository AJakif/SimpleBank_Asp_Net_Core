using System;
using System.Collections.Generic;
using System.Text;

namespace Banker.Models.ViewModels
{
    public class CollectData
    {
        public List<Transection> Transections { get; set; }

        public UserViewModel User { get; set; }
        public Transection Transection { get; set; }
    }
}
