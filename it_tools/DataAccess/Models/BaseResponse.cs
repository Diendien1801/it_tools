using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace it_tools.DataAccess.Models
{
    public class BaseResponse<T>
    {
        public bool success { get; set; } 
        public T data { get; set; }  
        public string message { get; set; }
    }
}
