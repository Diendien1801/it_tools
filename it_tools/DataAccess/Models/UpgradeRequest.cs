using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace it_tools.DataAccess.Models
{
    public class UpgradeRequest
    {
        public string IdRequest { get; set; }
        public string IdUser { get; set; }
        public string Username { get; set; }
        public string Level { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? HandledAt { get; set; }
    }


}
