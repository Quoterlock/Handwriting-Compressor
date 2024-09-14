namespace HandwritingCompressor.Modules.Interfaces
{
    public interface IKeyStorage
    {
        void Store(string name, string key);
        string Retreive(string name);
    }
}
