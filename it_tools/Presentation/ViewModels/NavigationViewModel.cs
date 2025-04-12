using it_tools.BusinessLogic.Services;
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
        private readonly IToolService _toolService;
        public ObservableCollection<ToolCategory> ToolCategories { get; set; }
        
        public bool IsUser { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
        public bool IsGuest => !IsUser;
        public NavigationViewModel( IToolService toolService)
        {
            _toolService = toolService;
            ToolCategories = new ObservableCollection<ToolCategory>();
            _ = LoadToolCategoriesAsync();
            
        }

        public async Task LoadToolCategoriesAsync()
        {
            var categories = await _toolService.GetToolCategoriesAsync();
            ToolCategories.Clear();
            ToolCategories.Add(new ToolCategory
            {
                idToolType = "0",
                name = "All"

            });
            ToolCategories.Add(new ToolCategory
            {
                idToolType = "-1",
                name = "Favourite"

            });
            foreach (var category in categories)
            {
                ToolCategories.Add(category);
            }
        }
    }
}
