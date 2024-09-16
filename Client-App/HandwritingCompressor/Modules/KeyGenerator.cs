using System.Security.Cryptography;

namespace HandwritingCompressor.Modules
{
    public static class KeyGenerator
    {
        public static string Generate(int bytesLength)
        {
            return RandomNumberGenerator.GetString(GetSymbolsSet(), bytesLength);
        }

        private static string GetSymbolsSet()
            =>"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
    }
}
