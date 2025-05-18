using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class ManagerDto
    {
        public int ManagerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

      //  public List<JobDto> Jobs { get; set; }

    }
}
