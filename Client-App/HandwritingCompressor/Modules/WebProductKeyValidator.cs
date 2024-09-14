using System.Net.Http;
using System.Text.Json;
using HandwritingCompressor.Modules.Interfaces;

namespace HandwritingsCompressor.Modules
{
    public class WebProductKeyValidator : IKeyValidator
    {
        // TODO : Move to config file
        private string _baseUrl = "https://localhost:7032";
        public bool IsValid(string productKey)
        {
            try
            {
                if (string.IsNullOrEmpty(productKey))
                    return false;
                return VerifyOnServer(productKey);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool VerifyOnServer(string enctryptedKey)
        {
            HttpClient client = new HttpClient();
            try
            {
                string url = $"{_baseUrl}/verify?key={enctryptedKey}";

                HttpResponseMessage response = client.GetAsync(url).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();

                string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                if (!string.IsNullOrEmpty(responseBody))
                    return JsonSerializer.Deserialize<ValidationResult>(responseBody).result;

                return false;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
            return false;
        }

    }

    class ValidationResult
    {
        public bool result { get; set; }
    }
}
