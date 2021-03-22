namespace FileSystemVisitor.FileSystemDataProviders
{
    public interface IFileSystemDataProvider
    {
        string[] GetFolders(string path);
        string[] GetFiles(string path);
    }
}
