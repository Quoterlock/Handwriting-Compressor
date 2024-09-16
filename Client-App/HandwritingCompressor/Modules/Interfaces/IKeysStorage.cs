namespace HandwritingCompressor.Modules.Interfaces
{
    public interface IKeysStorage
    {
        void Store(string name, string key);
        string Retreive(string name);
    }
}
