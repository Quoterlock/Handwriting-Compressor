using HandwritingCompressor.Modules.Interfaces;
using System.IO;

namespace HandwritingCompressor.Modules
{
    public class EncryptedFileReader : ITextFileReader
    {
        // AES 256 key
        private const string ENCRYPTION_KEY = "cT+zGf8QXUklvskijxwMzirYsRhRFu1h6eXiIj24z64=";

        public string Read(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    var content = File.ReadAllText(path);
                    return SymmetricEncrypter.Decrypt(content, ENCRYPTION_KEY);
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public void Write(string path, string content)
        {
            try
            {
                var enctyptedContent = SymmetricEncrypter.Encrypt(content, ENCRYPTION_KEY);
                File.WriteAllText(path, enctyptedContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }
    }
}
