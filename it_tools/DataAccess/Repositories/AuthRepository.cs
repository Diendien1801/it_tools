using it_tools.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace it_tools.DataAccess.Repositories
{
    class AuthRepository
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5000/api/auth"; // Cập nhật đúng URL backend

        public AuthRepository()
        {
            _httpClient = new HttpClient();
        }

        public async Task<(bool success, string message)> RegisterAsync(string username, string password)
        {
            try
            {
                var userData = new { username, password };
                string jsonData = JsonSerializer.Serialize(userData);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                Console.WriteLine($"[DEBUG] Sending request to {BaseUrl}/register with data: {jsonData}");

                HttpResponseMessage response = await _httpClient.PostAsync($"{BaseUrl}/register", content);
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
            var loginData = new { username, password };
            string jsonData = JsonSerializer.Serialize(loginData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync($"{BaseUrl}/login", content);
            string responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<BaseResponse<string>>(responseContent);

            if (result.success)
            {
                return (true, result.message, result.data); // Token ở data
            }

            return (false, result.message, null);
        }

    } 


}

