using ProductKeyManagerApp.Modules.Interfaces;

namespace ProductKeyManagerApp.Modules
{
    public class ProductKeyManager : IProductKeyManager
    {
        private List<string> _keys = [];
        public string GenerateKey()
        {
            var key = Guid.NewGuid().ToString();
            _keys.Add(key);
            return key;
        }

        public bool VerifyKey(string key)
        {
            return _keys.Contains(key);
        }
    }
}
