using System.IO;

namespace FileSystemVisitor.FileSystemDataProviders
{
    public class FileSystemDataProvider : IFileSystemDataProvider
    {
        public string[] GetFiles(string path)
        {
            return Directory.GetFiles(path);
        }

        public string[] GetFolders(string path)
        {
            return Directory.GetDirectories(path);
        }
    }
}
