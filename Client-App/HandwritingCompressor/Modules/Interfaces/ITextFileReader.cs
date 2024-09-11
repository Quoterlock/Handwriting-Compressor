namespace HandwritingCompressor.Modules.Interfaces
{
    public interface ITextFileReader
    {
        string Read(string path);
        void Write(string path, string content);
    }
}
