using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelLayer.Models.SystemParameters
{
    public class DisplaySystemParametersModel : BaseModel
    {
        [Required]
        [DisplayName("Parameter name")]
        public string ParameterName { get; set; }

        [Required]
        [DisplayName("Parameter description")]
        public string ParameterDescription { get; set; }

        [Required]
        [DisplayName("Parameter value")]
        public string ParameterValue { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; }
    }
}
