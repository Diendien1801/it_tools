using it_tools.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace it_tools.DataAccess.Repositories
{
    public class AccountRepository
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5000/api/account";
        

        public AccountRepository()
        {
            _httpClient = new HttpClient();
        }

        public async Task<(bool success, string message, User data)> GetAccountInfoAsync(string token)
        {
            try
            {
                string url = $"{BaseUrl}"; // URL API không cần token trong đường dẫn
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
                string url = $"{BaseUrl}/favourite"; // Không cần truyền token trong URL
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
                    Debug.WriteLine($"[GetFavoriteToolsAsync] Số lượng tool yêu thích: {result.data.Count}");
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
                string url = $"{BaseUrl}/favourite/add"; // API endpoint
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
                string url = $"{BaseUrl}/favourite/remove"; // API endpoint
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



    }
}


