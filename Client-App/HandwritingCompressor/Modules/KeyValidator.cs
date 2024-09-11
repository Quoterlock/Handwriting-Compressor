using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace HandwritingsCompressor.Modules
{
    public class KeyValidator
    {
        private string _baseUrl = "https://localhost:7032";
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

            EncryptionKey? result;
            if (!string.IsNullOrEmpty(responseBody))
            {
                result = JsonSerializer.Deserialize<EncryptionKey>(responseBody);
                if (result != null)
                    return result.public_key;
                else 
                    return string.Empty;
            }
            return string.Empty;

        }

        private string Encrypt(string key, string msg)
        {
            // Convert the public key from base64 to byte array
            byte[] publicKeyBytes = Convert.FromBase64String(key);

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    // Import the public key
                    rsa.ImportCspBlob(publicKeyBytes);

                    // Convert the message to a byte array
                    byte[] messageBytes = Encoding.UTF8.GetBytes(msg);

                    // Encrypt the message using the public key
                    byte[] encryptedBytes = rsa.Encrypt(messageBytes, false);

                    // Convert the encrypted bytes to base64 for easier transport
                    return Convert.ToBase64String(encryptedBytes);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error during encryption: " + ex.Message);
                    return string.Empty;
                }
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
    class EncryptionKey
    {
        public string public_key { get; set; }
    }
}
