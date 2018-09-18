using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Classes
{
    [Table("SystemParameters")]
    public class SystemParameter
    {

        public Guid Id { get; set; }

        public string ParameterName { get; set; }

        public string ParameterDescription { get; set; }

        public string ParameterValue { get; set; }

        public bool IsActive { get; set; }
    }
}
