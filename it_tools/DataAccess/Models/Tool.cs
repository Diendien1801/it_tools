using it_tools.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolLib;

namespace it_tools.DataAccess.Models
{
    public class Tool
    {
        public string idTool { get; set; }
        public string name { get; set; }
        public string descript { get; set; }
        public string status { get; set; }
        public string access_level { get; set; }
        public string iconURL { get; set; }
        public string idToolType { get; set; }
        public string? dllPath { get; set; }

        public ITool? LoadedPlugin { get; set; }
        public void LoadPlugin()
        {
            if (!string.IsNullOrEmpty(dllPath))
            {
                LoadedPlugin = ToolService.LoadToolFromDll(dllPath);
            }
        }

    }
}
