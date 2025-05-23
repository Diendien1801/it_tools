﻿using it_tools.DataAccess.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace it_tools.DataAccess.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _pluginPath;
        private readonly string _baseUrl;

        public AccountRepository(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _pluginPath = config["PluginPath"];
            _baseUrl = config["ApiUrls:Account"];
        }

       

        public User User
        {
            get => default;
            set
            {
            }
        }

        public async Task<(bool success, string message, User data)> GetAccountInfoAsync(string token)
        {
            try
            {
                string url = $"{_baseUrl}"; // URL API không cần token trong đường dẫn
                Console.WriteLine($"[DEBUG] Gọi API: {url}");

                // Tạo request và đặt token vào Header
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"[DEBUG] Response Status: {response.StatusCode}");
                Console.WriteLine($"[DEBUG] Response Content: {responseContent}");

                var result = JsonSerializer.Deserialize<BaseResponse<User>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result != null && result.success)
                {
                    return (true, result.message, result.data);
                }

                return (false, result?.message ?? "Unknown error", null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Exception in GetAccountInfoAsync: {ex.Message}");
                return (false, "Lỗi khi lấy thông tin tài khoản", null);
            }
        }

        public async Task<(bool success, string message, List<Tool> tools)> GetFavoriteToolsAsync(string token)
        {
            try
            {
                string url = $"{_baseUrl}/favourite"; // Không cần truyền token trong URL
                Debug.WriteLine($"[GetFavoriteToolsAsync] Fetching from: {url}");

                // Tạo request và thêm token vào Header
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"[GetFavoriteToolsAsync] Response Status: {response.StatusCode}");
                Debug.WriteLine($"[GetFavoriteToolsAsync] Response Content: {responseContent}");

                var result = JsonSerializer.Deserialize<BaseResponse<List<Tool>>>(responseContent);

                if (result != null && result.success)
                {
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
                    return (true, result.message, result.data);
                }

                Debug.WriteLine($"[GetFavoriteToolsAsync] Lấy tool yêu thích thất bại - Message: {result?.message ?? "Unknown error"}");
                return (false, result?.message ?? "Unknown error", new List<Tool>());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetFavoriteToolsAsync] Lỗi khi fetch tool yêu thích: {ex.Message}");
                return (false, "Lỗi xử lý phản hồi từ server", new List<Tool>());
            }


        }
        public async Task<(bool success, string message)> AddFavoriteToolAsync(string token, string idTool)
        {
            try
            {
                string url = $"{_baseUrl}/favourite/add"; // API endpoint
                Debug.WriteLine($"[AddFavoriteToolAsync] Sending request to: {url}");

                var requestBody = new { idTool }; // Đối tượng chứa idTool
                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"[AddFavoriteToolAsync] Response Status: {response.StatusCode}");
                Debug.WriteLine($"[AddFavoriteToolAsync] Response Content: {responseContent}");

                var result = JsonSerializer.Deserialize<BaseResponse<object>>(responseContent);

                if (result != null && result.success)
                {
                    return (true, result.message);
                }

                return (false, result?.message ?? "Unknown error");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AddFavoriteToolAsync] Lỗi khi thêm tool yêu thích: {ex.Message}");
                return (false, "Lỗi khi thêm tool yêu thích");
            }
        }

        public async Task<(bool success, string message)> RemoveFavoriteToolAsync(string token, string idTool)
        {
            try
            {
                string url = $"{_baseUrl}/favourite/remove"; // API endpoint
                Debug.WriteLine($"[RemoveFavoriteToolAsync] Sending request to: {url}");

                var requestBody = new { idTool }; // Đối tượng chứa idTool
                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"[RemoveFavoriteToolAsync] Response Status: {response.StatusCode}");
                Debug.WriteLine($"[RemoveFavoriteToolAsync] Response Content: {responseContent}");

                var result = JsonSerializer.Deserialize<BaseResponse<object>>(responseContent);

                if (result != null && result.success)
                {
                    return (true, result.message);
                }

                return (false, result?.message ?? "Unknown error");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[RemoveFavoriteToolAsync] Lỗi khi xóa tool yêu thích: {ex.Message}");
                return (false, "Lỗi khi xóa tool yêu thích");
            }
        }

        public async Task<(bool success, string message)> SendUpgradeRequestAsync(string token)
        {
            try
            {
                string url = $"{_baseUrl}/upgrade";
                Debug.WriteLine($"[SendUpgradeRequestAsync] Sending request to: {url}");

                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"[SendUpgradeRequestAsync] Response Status: {response.StatusCode}");
                Debug.WriteLine($"[SendUpgradeRequestAsync] Response Content: {responseContent}");

                var result = JsonSerializer.Deserialize<BaseResponse<object>>(responseContent);

                if (result != null && result.success)
                {
                    return (true, result.message);
                }

                return (false, result?.message ?? "Unknown error");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[SendUpgradeRequestAsync] Lỗi khi gửi yêu cầu nâng cấp: {ex.Message}");
                return (false, "Lỗi khi gửi yêu cầu nâng cấp");
            }
        }
        public async Task<(bool success, string message, List<UpgradeRequest> upgradeRequests)> GetHistoryRequest(string token)
        {
            try
            {
                string url = $"{_baseUrl}/upgradeHistory";
                Debug.WriteLine($"[GetHistoryRequest] Fetching from: {url}");

                // Create request and add token to the header
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"[GetHistoryRequest] Response Status: {response.StatusCode}");
                Debug.WriteLine($"[GetHistoryRequest] Response Content: {responseContent}");

                var result = JsonSerializer.Deserialize<BaseResponse<List<UpgradeRequest>>>(responseContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result != null && result.success)
                {
                    return (true, result.message, result.data);
                }

                Debug.WriteLine($"[GetHistoryRequest] Lấy lịch sử yêu cầu thất bại - Message: {result?.message ?? "Unknown error"}");
                return (false, result?.message ?? "Unknown error", null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetHistoryRequest] Lỗi khi lấy lịch sử yêu cầu: {ex.Message}");
                return (false, "Lỗi khi lấy lịch sử yêu cầu nâng cấp", null);
            }
        }

        
    }
}


