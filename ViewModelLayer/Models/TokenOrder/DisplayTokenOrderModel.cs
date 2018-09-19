using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelLayer.Models.TokenOrder
{
    public partial class DisplayTokenOrderModel
    {

        public int Amount { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; }

        [DisplayName("Created at")]
        public DateTime TimestampCreated { get; set; }

        [DisplayName("Status changed at")]
        public DateTime TimestampChanged { get; set; }

    }
}
