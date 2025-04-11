using it_tools.DataAccess.Models;
using Microsoft.Extensions.Configuration;
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
    class AuthRepository : IAuthRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public AuthRepository(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _baseUrl = config["ApiUrls:Auth"];
        }

        public async Task<(bool success, string message)> RegisterAsync(string username, string password)
        {
            try
            {
                var userData = new { username, password };
                string jsonData = JsonSerializer.Serialize(userData);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                Console.WriteLine($"[DEBUG] Sending request to {_baseUrl}/register with data: {jsonData}");

                HttpResponseMessage response = await _httpClient.PostAsync($"{_baseUrl}/register", content);
                string responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"[DEBUG] Response Status: {response.StatusCode}");
                Console.WriteLine($"[DEBUG] Response Content: {responseContent}");

                var result = JsonSerializer.Deserialize<BaseResponse<object>>(responseContent);

                if (result != null && result.success)
                {
                    Console.WriteLine($"[DEBUG] Register Success: {result.message}");
                    return (true, result.message);
                }

                Console.WriteLine($"[DEBUG] Register Failed: {result?.message ?? "Unknown error"}");
                return (false, result?.message ?? "Unknown error");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Exception in RegisterAsync: {ex.Message}");
                return (false, "An error occurred during registration");
            }
        }
        public async Task<(bool success, string message, string token)> LoginAsync(string username, string password)
        {
            Debug.WriteLine($"[LoginAsync] Bắt đầu đăng nhập với Username: {username}");

            var loginData = new { username, password };
            string jsonData = JsonSerializer.Serialize(loginData);
            Debug.WriteLine($"[LoginAsync] Dữ liệu gửi đi: {jsonData}");

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync($"{_baseUrl}/login", content);
            Debug.WriteLine($"[LoginAsync] Response Status: {response.StatusCode}");

            string responseContent = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"[LoginAsync] Response Content: {responseContent}");

            try
            {
                var result = JsonSerializer.Deserialize<BaseResponse<String>>(responseContent);

                if (result != null && result.success && result.data != null)
                {
                    string token = result.data;
                    

                    Debug.WriteLine($"[LoginAsync] Đăng nhập thành công - Message: {result.message}");
                    Debug.WriteLine($"[LoginAsync] Token: {token}");
                    

                    return (true, result.message, token);
                }

                Debug.WriteLine($"[LoginAsync] Đăng nhập thất bại - Message: {result?.message ?? "Unknown error"}");
                return (false, result?.message ?? "Unknown error", null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[LoginAsync] Lỗi khi parse JSON: {ex.Message}");
                return (false, "Lỗi xử lý phản hồi từ server", null);
            }
        }

        public Task<bool> LogoutAsync(string token)
        {
            throw new NotImplementedException();
        }
    }


}

