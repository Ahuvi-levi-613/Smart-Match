using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class Requirements
    {
        [Key]
        public int RequirementId { get; set; }
        public string Description { get; set; }
    }

}
