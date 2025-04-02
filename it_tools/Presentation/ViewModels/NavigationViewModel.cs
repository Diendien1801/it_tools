using it_tools.DataAccess.Models;
using it_tools.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace it_tools.Presentation.ViewModels
{
    public class NavigationViewModel
    {
        private readonly ToolRepository _toolRepository;
        public ObservableCollection<ToolCategory> ToolCategories { get; set; }

        public NavigationViewModel()
        {
            _toolRepository = new ToolRepository();
            ToolCategories = new ObservableCollection<ToolCategory>();
            _ = LoadToolCategoriesAsync();
            
        }

        public async Task LoadToolCategoriesAsync()
        {
            var categories = await _toolRepository.GetToolCategoriesAsync();
            ToolCategories.Clear();
            ToolCategories.Add(new ToolCategory
            {
                idToolType = "0",
                name = "All"

            });
            foreach (var category in categories)
            {
                ToolCategories.Add(category);
            }
        }
    }
}
