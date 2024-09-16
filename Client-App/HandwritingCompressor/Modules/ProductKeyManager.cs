using HandwritingCompressor.Modules.Interfaces;
using HandwritingsCompressor.Exceptions;
using Microsoft.Win32;
using System.Text.Json;

namespace HandwritingCompressor.Modules
{
    public class ProductKeyManager : IProductKeyManager
    {
        private readonly IKeyValidator _keyValidator;
        private readonly ITextFileReader _fileReader;
        private const string FILE_NAME = "license.bin";

        public ProductKeyManager(IKeyValidator keyValidator, ITextFileReader fileReader)
        {
            _keyValidator = keyValidator;
            _fileReader = fileReader;
        }

        public bool IsActivated()
        {
            bool isActivated = false;
            var key = GetStoredKey();
         
            if (!string.IsNullOrEmpty(key))
                isActivated = _keyValidator.IsValid(key);
            
            return isActivated;
        }

        public void Activate(string key)
        {
            if (!_keyValidator.IsValid(key))
                throw new InvalidProductKeyException(key);
            try
            {
                Store(key);
            } 
            catch (Exception ex)
            {
                throw new Exception(
                    $"Exception occured during product_key saving: {ex.Message}");
            }
        }

        private void Store(string productKey)
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
            _fileReader.Write(FILE_NAME, jsonString);
        }

        private string GetStoredKey()
        {
            try
            {
                var jsonString = _fileReader.Read(FILE_NAME);                
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
