using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using HandwritingCompressor.Modules.Interfaces;

namespace HandwritingCompressor.Modules
{
    public class EncryptionKeysStorage : IKeysStorage
    {
        public string Retreive(string name)
        {
            var path = GetPath(name);
            if (!File.Exists(path))
                return string.Empty;

            var bytes = File.ReadAllBytes(path);
            var decryptedKey = ProtectedData.Unprotect(bytes, null, DataProtectionScope.CurrentUser);
            return System.Text.Encoding.UTF8.GetString(decryptedKey);
        }

        public void Store(string name, string key)
        {
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
            byte[] encryptedKey = ProtectedData.Protect(keyBytes, null, DataProtectionScope.CurrentUser);
            
            var path = GetPath(name);
            File.WriteAllBytes(path, encryptedKey);
        }

        private string GetPath(string name) =>
            $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Microsoft\\Crypto\\Keys\\{name}.bin";
    }
}
