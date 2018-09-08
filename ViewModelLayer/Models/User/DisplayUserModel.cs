using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelLayer.Models.User
{
    public class DisplayUserModel : BaseUserModel
    {
        
        [DisplayName("Token count")]
        public int TokenCount { get; set; }

    }
}
