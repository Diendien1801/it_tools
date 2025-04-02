using it_tools.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ToolLib; // Thêm thư viện Debug

namespace it_tools.DataAccess.Repositories
{
    public class ToolRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5000";
        private readonly string _pluginPath = "C:/Users/dient/source/repos/it_tools/it_tools/";
        public ToolRepository()
        {
            _httpClient = new HttpClient();
        }


        public async Task<List<Tool>> GetAllTools()
        {
            try
            {
                string url = $"{_baseUrl}/api/tool/categories/all";
                Debug.WriteLine($"🔹 Sending request to: {url}");

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                Debug.WriteLine($"🔹 Response Status Code: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"🔹 Response JSON: {json}");

                    var result = JsonSerializer.Deserialize<BaseResponse<List<Tool>>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (result?.success == true && result.data != null)
                    {
                        Debug.WriteLine($"✅ Retrieved {result.data.Count} tools");

                        foreach (var tool in result.data)
                        {
                            if (!string.IsNullOrEmpty(tool.dllPath))
                            {
                                tool.dllPath = Path.Combine(_pluginPath, tool.dllPath);
                                Debug.WriteLine($"🔹 Plugin Path for {tool.name}: {tool.dllPath}");

                                if (File.Exists(tool.dllPath))
                                {
                                    tool.LoadPlugin();
                                }
                                else
                                {
                                    Debug.WriteLine($"❌ Plugin not found at: {tool.dllPath}");
                                }
                            }
                        }

                        return result.data;
                    }
                    else
                    {
                        Debug.WriteLine("❌ API returned failure response or null data");
                        return new List<Tool>();
                    }
                }
                else
                {
                    Debug.WriteLine($"❌ API Error: {response.StatusCode}");
                    return new List<Tool>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ API Request Failed: {ex.Message}");
                return new List<Tool>();
            }
        }
        public async Task<List<Tool>> GetToolsByCategoryAsync(string idToolType)
        {
            try
            {
                string url = $"{_baseUrl}/api/tool/categories/{idToolType}";
                Debug.WriteLine($"🔹 Sending request to: {url}");

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                Debug.WriteLine($"🔹 Response Status Code: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"🔹 Response JSON: {json}");

                    var result = JsonSerializer.Deserialize<BaseResponse<List<Tool>>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (result?.success == true && result.data != null)
                    {
                        Debug.WriteLine($"✅ Retrieved {result.data.Count} tools");

                        foreach (var tool in result.data)
                        {
                            if (!string.IsNullOrEmpty(tool.dllPath))
                            {
                                tool.dllPath = Path.Combine(_pluginPath, tool.dllPath);
                                Debug.WriteLine($"🔹 Plugin Path for {tool.name}: {tool.dllPath}");

                                if (File.Exists(tool.dllPath))
                                {
                                    tool.LoadPlugin();
                                }
                                else
                                {
                                    Debug.WriteLine($"❌ Plugin not found at: {tool.dllPath}");
                                }
                            }
                        }

                        return result.data;
                    }
                    else
                    {
                        Debug.WriteLine("❌ API returned failure response or null data");
                        return new List<Tool>();
                    }
                }
                else
                {
                    Debug.WriteLine($"❌ API Error: {response.StatusCode}");
                    return new List<Tool>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ API Request Failed: {ex.Message}");
                return new List<Tool>();
            }
        }




        public async Task<List<ToolCategory>> GetToolCategoriesAsync()
        {
            try
            {
                string url = $"{_baseUrl}/api/tool/categories";
                Debug.WriteLine($"[DEBUG] Fetching Tool Categories from: {url}");

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[DEBUG] Response JSON: {json}");

                    var result = JsonSerializer.Deserialize<BaseResponse<List<ToolCategory>>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (result?.data != null)
                    {
                        Debug.WriteLine($"[DEBUG] Loaded {result.data.Count} categories.");
                    }
                    else
                    {
                        Debug.WriteLine("[ERROR] Deserialized result is null.");
                    }

                    return result?.data ?? new List<ToolCategory>();
                }
                else
                {
                    Debug.WriteLine($"[ERROR] HTTP Response: {response.StatusCode}, {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Exception: {ex.Message}");
            }

            return new List<ToolCategory>();
        }
        
        //public List<Tool> LoadToolsFromFolder()
        //{
        //    var tools = new List<Tool>();

        //    if (!Directory.Exists(_pluginPath))
        //    {
        //        Directory.CreateDirectory(_pluginPath);
        //    }
        //    foreach (var file in Directory.GetFiles(_pluginPath, "*.dll"))
        //    {
        //        Assembly assembly = Assembly.LoadFrom(file);
        //        var pluginType = assembly.GetTypes().FirstOrDefault(t => typeof(ITool).IsAssignableFrom(t) && !t.IsInterface);

        //        if (pluginType != null)
        //        {
        //            var pluginInstance = (ITool)Activator.CreateInstance(pluginType);
        //            tools.Add(new Tool
        //            {
        //                idTool = Guid.NewGuid().ToString(),
        //                name = pluginInstance.name,
        //                descript = pluginInstance.descript,
        //                iconURL = pluginInstance.iconURL,
        //                idToolType = pluginInstance.idToolType,
        //                access_level = pluginInstance.access_level,
        //                status = pluginInstance.status,
        //                dllPath = file,
        //                LoadedPlugin = pluginInstance
        //            });
        //        }
        //    }

        //    return tools;
        //}

        // Để tạm ở đây
        public void ExecuteTool(Tool tool)
        {
            tool.LoadedPlugin?.Execute();
        }

    }
}
