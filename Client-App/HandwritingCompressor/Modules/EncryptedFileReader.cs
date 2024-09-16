using HandwritingCompressor.Modules.Interfaces;
using System.IO;
using System.Security.Cryptography;

namespace HandwritingCompressor.Modules
{
    public class EncryptedFileReader : ITextFileReader
    {
        private readonly string _key = string.Empty;
        public EncryptedFileReader(IKeysStorage keys) 
        {
            string name = "handwriting-app-data-encryption-key";
            while (string.IsNullOrEmpty(_key))
            {
                _key = keys.Retreive(name);
                if (string.IsNullOrEmpty(_key))
                    keys.Store(name, KeyGenerator.Generate(32));
            }
        }

        public string Read(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    var content = File.ReadAllText(path);
                    return SymmetricEncrypter.Decrypt(content, _key);
                }
            }
            catch (CryptographicException ex)
            {
                File.Delete(path);
            }
            catch (Exception) { }
            return string.Empty;
        }

        public void Write(string path, string content)
        {
            try
            {
                var enctyptedContent = SymmetricEncrypter.Encrypt(content, _key);
                File.WriteAllText(path, enctyptedContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }
    }
}
