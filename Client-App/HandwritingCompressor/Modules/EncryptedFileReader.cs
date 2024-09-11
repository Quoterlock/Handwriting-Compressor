using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandwritingCompressor.Modules
{
    public class EncryptedFileReader
    {
        public static string ReadFromFile(string path)
        {
            try
            {
                if (File.Exists(path))
                    return File.ReadAllText(path);
                else
                    return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static void WriteToFile(string path, string serializedContent)
        {
            try
            {
                File.WriteAllText(path, serializedContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }
    }
}
