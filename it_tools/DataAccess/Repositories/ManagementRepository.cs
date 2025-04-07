using it_tools.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace it_tools.DataAccess.Repositories
{
    internal class ManagementRepository
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5000/api/management";

        public ManagementRepository()
        {
            _httpClient = new HttpClient();
        }

        public async Task<(bool success, string message, List<UpgradeRequest> data)> GetAllRequestAsync(string token)
        {
            try
            {
                string url = $"{BaseUrl}/requests";
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
                string url = $"{BaseUrl}/requests/approve";
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
                string url = $"{BaseUrl}/requests/reject";
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
                string url = $"{BaseUrl}/tools";
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
                string url = $"{BaseUrl}/tool/disable";
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
                string url = $"{BaseUrl}/tool/active";
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
                string url = $"{BaseUrl}/tool/update"; // API endpoint phía server phải trùng khớp
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





    }
}