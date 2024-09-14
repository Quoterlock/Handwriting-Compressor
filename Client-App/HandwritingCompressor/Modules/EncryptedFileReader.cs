using HandwritingCompressor.Modules.Interfaces;
using System.IO;

namespace HandwritingCompressor.Modules
{
    public class EncryptedFileReader : ITextFileReader
    {
        private readonly string _key;
        public EncryptedFileReader(IKeyStorage keys) 
        {
            _key = keys.Retreive("handwriting_app_kek"); 
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
