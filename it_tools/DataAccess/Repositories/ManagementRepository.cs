using it_tools.BusinessLogic.Services;
using it_tools.DataAccess.Models;
using it_tools.Presentation.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ToolLib;

namespace it_tools.DataAccess.Repositories
{
    internal class ManagementRepository : IManagementRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ManagementRepository(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _baseUrl = config["ApiUrls:Management"];
        }

        public async Task<(bool success, string message, List<UpgradeRequest> data)> GetAllRequestAsync(string token)
        {
            try
            {
                string url = $"{_baseUrl}/requests";
                Debug.WriteLine($"[DEBUG] Calling API: {url}");

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"[DEBUG] Response Status: {response.StatusCode}");
                Debug.WriteLine($"[DEBUG] Response Content: {responseContent}");

                var result = JsonSerializer.Deserialize<BaseResponse<List<UpgradeRequest>>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result != null && result.success)
                {
                    return (true, result.message, result.data);
                }

                return (false, result?.message ?? "Unknown error", null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Exception in GetAllRequestAsync: {ex.Message}");
                return (false, "Error retrieving request list", null);
            }
        }


        public async Task<(bool success, string message)> AcceptUpgradeRequest(string token, string idRequest)
        {
            try
            {
                string url = $"{_baseUrl}/requests/approve";
                Debug.WriteLine($"[DEBUG] Calling API: {url}");

                var requestBody = new
                {
                    idRequest = idRequest
                };

                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                request.Content = jsonContent;

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"[DEBUG] Response Status: {response.StatusCode}");
                Debug.WriteLine($"[DEBUG] Response Content: {responseContent}");

                var result = JsonSerializer.Deserialize<BaseResponse<object>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result != null && result.success)
                {
                    return (true, result.message);
                }

                return (false, result?.message ?? "Unknown error");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Exception in AcceptUpgradeRequest: {ex.Message}");
                return (false, "Error accepting request");
            }
        }
        public async Task<(bool success, string message)> RejectUpgradeRequest(string token, string idRequest)
        {
            try
            {
                string url = $"{_baseUrl}/requests/reject";
                Debug.WriteLine($"[DEBUG] Calling API: {url}");

                var requestBody = new
                {
                    idRequest = idRequest
                };

                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                request.Content = jsonContent;

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"[DEBUG] Response Status: {response.StatusCode}");
                Debug.WriteLine($"[DEBUG] Response Content: {responseContent}");

                var result = JsonSerializer.Deserialize<BaseResponse<object>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result != null && result.success)
                {
                    return (true, result.message);
                }

                return (false, result?.message ?? "Unknown error");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Exception in RejectUpgradeRequest: {ex.Message}");
                return (false, "Error rejecting request");
            }
        }

        public async Task<(bool success, string message, List<Tool> data)> GetAllToolAsync(string token)
        {
            try
            {
                string url = $"{_baseUrl}/tools";
                Debug.WriteLine($"[DEBUG] Calling API: {url}");

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"[DEBUG] Response Status: {response.StatusCode}");
                Debug.WriteLine($"[DEBUG] Response Content: {responseContent}");

                var result = JsonSerializer.Deserialize<BaseResponse<List<Tool>>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result != null && result.success)
                {
                    return (true, result.message, result.data);
                }

                return (false, result?.message ?? "Unknown error", null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Exception in GetAllToolAsync: {ex.Message}");
                return (false, "Error retrieving tool list", null);
            }
        }

        public async Task<(bool success, string message)> DisableTool(string token, string idTool)
        {
            try
            {
                string url = $"{_baseUrl}/tool/disable";
                Debug.WriteLine($"[DEBUG] Calling API: {url}");

                var body = new { idTool = idTool };
                var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"[DEBUG] Response Status: {response.StatusCode}");
                Debug.WriteLine($"[DEBUG] Response Content: {responseContent}");

                var result = JsonSerializer.Deserialize<BaseResponse<object>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result != null && result.success)
                {
                    return (true, result.message);
                }

                return (false, result?.message ?? "Unknown error");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Exception in DisableTool: {ex.Message}");
                return (false, "Lỗi khi disable tool");
            }
        }
        public async Task<(bool success, string message)> EnableTool(string token, string idTool)
        {
            try
            {
                string url = $"{_baseUrl}/tool/active";
                Debug.WriteLine($"[DEBUG] Calling API: {url}");

                var body = new { idTool = idTool };
                var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"[DEBUG] Response Status: {response.StatusCode}");
                Debug.WriteLine($"[DEBUG] Response Content: {responseContent}");

                var result = JsonSerializer.Deserialize<BaseResponse<object>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result != null && result.success)
                {
                    return (true, result.message);
                }

                return (false, result?.message ?? "Unknown error");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Exception in EnableTool: {ex.Message}");
                return (false, "Lỗi khi enable tool");
            }
        }

        public async Task<(bool success, string message)> UpdateAccessLevel(string token, string idTool, string accessLevel)
        {
            try
            {
                string url = $"{_baseUrl}/tool/update"; // API endpoint phía server phải trùng khớp
                Debug.WriteLine($"[DEBUG] Calling API: {url}");

                var body = new { idTool = idTool, accessLevel = accessLevel };
                var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"[DEBUG] Response Status: {response.StatusCode}");
                Debug.WriteLine($"[DEBUG] Response Content: {responseContent}");

                var result = JsonSerializer.Deserialize<BaseResponse<object>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result != null && result.success)
                {
                    return (true, result.message);
                }

                return (false, result?.message ?? "Unknown error");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Exception in UpdateAccessLevel: {ex.Message}");
                return (false, "Lỗi khi cập nhật quyền truy cập tool");
            }
        }

        public async Task<(bool success, string message)> DeleteToolAsync(string token, string idTool)
        {
            try
            {
                string url = $"{_baseUrl}/tool/delete"; 
                Debug.WriteLine($"[DEBUG] Calling API: {url}");

                var body = new { idTool = idTool };
                var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"[DEBUG] Response Status: {response.StatusCode}");
                Debug.WriteLine($"[DEBUG] Response Content: {responseContent}");

                var result = JsonSerializer.Deserialize<BaseResponse<object>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result != null && result.success)
                {
                    return (true, result.message);
                }

                return (false, result?.message ?? "Unknown error");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Exception in DeleteToolAsync: {ex.Message}");
                return (false, "Lỗi khi xóa tool");
            }
        }


        public async Task<(bool success, string message)> AddToolAsync(
    string token,
    string name,
    string descript,
    string iconURL,
    string access_level,
    string dllPath,
    string idToolType
)
        {
            try
            {
                string url = $"{_baseUrl}/tool/add"; // khớp với route backend
                Debug.WriteLine($"[DEBUG] Calling API: {url}");

                var requestBody = new
                {
                    name = name,
                    descript = descript,
                    iconURL = iconURL,
                    access_level = access_level,
                    dllPath = dllPath,
                    idToolType = idToolType
                };

                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = jsonContent
                };
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"[DEBUG] Response Status: {response.StatusCode}");
                Debug.WriteLine($"[DEBUG] Response Content: {responseContent}");

                var result = JsonSerializer.Deserialize<BaseResponse<object>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result != null && result.success)
                {
                    return (true, result.message);
                }

                return (false, result?.message ?? "Unknown error");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Exception in AddToolAsync: {ex.Message}");
                return (false, "Lỗi khi thêm công cụ");
            }
        }

        public async Task<(bool success, string message)> ReCoverToolAsync(string token, string idTool)
        {
            try
            {
                string url = $"{_baseUrl}/tool/recover"; // Đường dẫn API khôi phục tool
                Debug.WriteLine($"[DEBUG] Calling API: {url}");

                var body = new { idTool = idTool };
                var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"[DEBUG] Response Status: {response.StatusCode}");
                Debug.WriteLine($"[DEBUG] Response Content: {responseContent}");

                var result = JsonSerializer.Deserialize<BaseResponse<object>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result != null && result.success)
                {
                    return (true, result.message);
                }

                return (false, result?.message ?? "Unknown error");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Exception in RestoreToolAsync: {ex.Message}");
                return (false, "Lỗi khi khôi phục tool");
            }
        }



    }
}