using it_tools.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace it_tools.DataAccess.Repositories
{
    public interface IToolRepository
    {

        Task<List<Tool>> GetAllTools();
        Task<List<Tool>> GetToolsByCategoryAsync(string idToolType);
        Task<List<ToolCategory>> GetToolCategoriesAsync();
        
    }
}
