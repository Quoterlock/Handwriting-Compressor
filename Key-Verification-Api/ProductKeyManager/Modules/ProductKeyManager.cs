using Konscious.Security.Cryptography;
using ProductKeyManagerApp.Modules.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace ProductKeyManagerApp.Modules
{
    public class ProductKeyManager : IProductKeyManager
    {
        private readonly List<HashSaltPair> _productKeys = [];
        public string GenerateKey()
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var key = RandomNumberGenerator.GetString(chars, 32);
            for (int i = 0; i < 7; i++)
                key = key.Insert((i+1) * 4 + i, "-");
            StoreKey(key);
            return key;
        }

        private void StoreKey(string key)
        {
            var salt = GenerateSalt();
            var hashed = HashPassword(key, salt);
            _productKeys.Add(new HashSaltPair 
            { 
                Hash = hashed, 
                Salt = salt 
            });
        }

        public bool VerifyKey(string key)
        {
            foreach(var pair in _productKeys)
            {
                var hash = HashPassword(key, pair.Salt);
                if(CryptographicOperations.FixedTimeEquals(hash, pair.Hash))
                    return true;
            }
            return false;
        }

        public static byte[] GenerateSalt(int length = 16)
        {
            var salt = new byte[length];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetBytes(salt);
            }
            return salt;
        }

        public static byte[] HashPassword(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 4; // Кількість потоків
            argon2.MemorySize = 128 * 1024; // 128 MB оперативної пам'яті
            argon2.Iterations = 3; // Кількість ітерацій

            return argon2.GetBytes(32); // Генерація 32-байтового ключа
        }
    }

    class HashSaltPair
    {
        public byte[] Hash { get; set; } = [];
        public byte[] Salt { get; set; } = [];
    }
}
