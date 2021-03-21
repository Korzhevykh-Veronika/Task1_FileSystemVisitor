using System;
using System.Collections.Generic;
using System.Linq;
using FileSystemVisitor.FileSystemDataProviders;
using FileSystemVisitor.Models;

namespace FileSystemVisitor
{
    public class FileSystemVisitor
    {
        private const string Indentation = "   ";
        private const string PathFoldersSeparator = @"\";

        private readonly Delegates.Filter _filter;

        private IFileSystemDataProvider _dataProvider;

        public event Delegates.ShowMessage OnStart;
        public event Delegates.ShowMessage OnEnd;
        public event Delegates.ShowMessage OnFileFound;
        public event Delegates.ShowMessage OnDirectoryFound;
        public event Delegates.ProcessItem OnFilteredFileFound;
        public event Delegates.ProcessItem OnFilteredDirectoryFound;

        public FileSystemVisitor(IFileSystemDataProvider dataProvider, Delegates.Filter filter)
        {
            _dataProvider = dataProvider;
            _filter = filter;
        }

        public IEnumerable<string> GetDirectoryInfo(string path)
        {
            OnStart?.Invoke();

            foreach (var info in GetDirectoryInfoInternal(path))
            {
                yield return info;
            }

            OnEnd?.Invoke();
        }

        private IEnumerable<string> GetDirectoryInfoInternal(string path)
        {
            var directoryInfo = GetFoldersInfo(path).Concat(GetFilesInfo(path));

            return directoryInfo;
        }

        private IEnumerable<string> GetFoldersInfo(string path)
        {
            var folders = _dataProvider.GetFolders(path);

            foreach (var folder in folders)
            {
                var folderName = GetItemWithOutPath(folder);
                OnDirectoryFound?.Invoke();

                if (!_filter(folderName)) continue;

                var operation = OnFilteredDirectoryFound?.Invoke(folderName);

                if (operation == SearchOperation.ExcludeItem)
                {
                    continue;
                }

                yield return folderName;

                if (operation == SearchOperation.LastItem)
                {
                    break;
                }

                var subItems = GetDirectoryInfoInternal(folder);

                foreach (var subItem in subItems)
                {
                    yield return Indentation + subItem;
                }
            }
        }

        private IEnumerable<string> GetFilesInfo(string path)
        {
            var files = _dataProvider.GetFiles(path);

            foreach (var file in files)
            {
                var fileName = GetItemWithOutPath(file);

                OnFileFound?.Invoke();

                if (!_filter(fileName)) continue;

                var operation = OnFilteredFileFound?.Invoke(fileName);

                if (operation == SearchOperation.ExcludeItem)
                {
                    continue;
                }

                yield return GetItemWithOutPath(file);

                if (operation == SearchOperation.LastItem)
                {
                    break;
                }
            }
        }

        private string GetItemWithOutPath(string path)
        {
            var last = path.LastIndexOf(PathFoldersSeparator, StringComparison.Ordinal);

            return path[(last + 1)..];
        }
    }
}
