using Microsoft.Win32;
using System.Text.Json;

namespace HandwritingCompressor.Modules
{
    public class ProductKeyManager
    {
        public static void Save(string productKey)
        {
            var deviceIdObj = Registry.GetValue(
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography",
                "MachineGuid", string.Empty) 
                ?? throw new Exception("DeviceId is null");

            var keyItem = new ProductKeyItem
            {
                DeviceId = deviceIdObj.ToString() ?? string.Empty,
                ProductKey = productKey
            };

            var jsonString = JsonSerializer.Serialize(keyItem);
            EncryptedFileReader.WriteToFile("productKey.json", jsonString);
        }

        public static string Get()
        {
            try
            {
                var jsonString = EncryptedFileReader.ReadFromFile("productKey.json");
                var item = JsonSerializer.Deserialize<ProductKeyItem>(jsonString);

                if (item == null)
                    return string.Empty;

                var currentDeviceId = GetDeviceId();

                if (currentDeviceId != item.DeviceId) //file is invalid
                    return string.Empty;

                return item.ProductKey;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static string GetDeviceId()
        {
            var deviceIdObj = Registry.GetValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography",
                    "MachineGuid", string.Empty);
            if (deviceIdObj == null)
                return string.Empty;
            return deviceIdObj.ToString() ?? string.Empty;
        }
    }

    class ProductKeyItem
    {
        public string DeviceId { get; set; }
        public string ProductKey { get; set; }
    }
}
