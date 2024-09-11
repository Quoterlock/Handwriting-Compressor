namespace HandwritingCompressor.Modules.Interfaces
{
    public interface IProductKeyManager
    {
        bool IsActivated();
        void Activate(string key);
    }
}
