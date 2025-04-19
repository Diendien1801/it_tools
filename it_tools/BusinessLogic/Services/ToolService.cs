using it_tools.DataAccess.Models;
using it_tools.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ToolLib;

namespace it_tools.BusinessLogic.Services
{
    public class ToolService : IToolService
    {
        private readonly IToolRepository _repo;

        public ToolService(IToolRepository repo)
        {
            _repo = repo;
        }

        public IToolRepository IToolRepository
        {
            get => default;
            set
            {
            }
        }

        public async Task<List<Tool>> GetAllTools()
        {
            var tools = await _repo.GetAllTools();
            return tools;
        }

        public async Task<List<Tool>> GetToolsByCategoryAsync(string idToolType)
        {
            var tools = await _repo.GetToolsByCategoryAsync(idToolType);
            return tools;
        }

        public async Task<List<ToolCategory>> GetToolCategoriesAsync()
        {
            var categories = await _repo.GetToolCategoriesAsync();
            return categories;
        }

    }
}
