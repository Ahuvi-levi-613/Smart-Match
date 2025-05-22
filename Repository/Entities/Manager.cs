using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{  
        public class Manager
        {
            [Key]
            public int ManagerId { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }

            public virtual List<Job> Jobs { get; set; }
        }
    }
 