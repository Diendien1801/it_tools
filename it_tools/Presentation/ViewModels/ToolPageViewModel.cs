using it_tools.DataAccess.Models;
using it_tools.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace it_tools.Presentation.ViewModels
{
    public class ToolPageViewModel
    {
        private readonly ToolRepository _toolRepository;
        public ObservableCollection<Tool> Tools { get; set; }


        public ToolPageViewModel()
        {
            _toolRepository = new ToolRepository();
            Tools = new ObservableCollection<Tool>();
           


        }
        public async Task LoadTools(string idToolType)
        {
            var tools = await _toolRepository.GetToolsByCategoryAsync(idToolType);
            Tools.Clear();
            foreach (var tool in tools)
            {
                Tools.Add(tool);
            }
        }
        
    }
}
