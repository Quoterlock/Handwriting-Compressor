namespace ProductKeyManagerApp.Modules.Interfaces
{
    public interface IProductKeyManager
    {
        bool VerifyKey(string key);
        string GenerateKey();
    }
}
