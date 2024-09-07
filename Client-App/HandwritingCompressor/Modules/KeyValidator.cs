using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace HandwritingsCompressor.Modules
{
    public class KeyValidator
    {
        private string _baseUrl = string.Empty;
        public bool Validate(string productKey)
        {
            try
            {
                string publicKey = GetEncryptionKey();
                var enctyptedKey = Encrypt(publicKey, productKey);
                return VerifyOnServer(enctyptedKey);
            } 
            catch (Exception)
            {
                return false;
            }
        }

        private string GetEncryptionKey()
        {
            HttpClient client = new HttpClient();
            string url = $"{_baseUrl}/get-public-key";

            HttpResponseMessage response = client.GetAsync(url).GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();

            string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            if (!string.IsNullOrEmpty(responseBody))
                return responseBody.ToString();
            return string.Empty;

        }

        private string Encrypt(string key, string msg)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(msg);

            using (RSA rsa = RSA.Create())
            {
                rsa.ImportFromPem(key.ToCharArray());
                byte[] encryptedBytes = rsa.Encrypt(messageBytes, RSAEncryptionPadding.OaepSHA256);
                return Convert.ToBase64String(encryptedBytes);
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
