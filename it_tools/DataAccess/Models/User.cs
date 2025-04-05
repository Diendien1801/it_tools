using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace it_tools.DataAccess.Models
{
    public class User
    {
        public string idUser { get; set; }
        public string username { get; set; }
        public string role { get; set; }
        public string createAt { get; set; }
        public string level { get; set; }
    }
}
