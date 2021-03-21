using System;
using System.Collections.Generic;
using System.IO;
using FileSystemVisitor.Models;

namespace FileSystemVisitor
{
    public class FileSystemVisitor
    {
        private readonly Delegates.Filter _filter;

        public event Delegates.ShowMessage OnStart;
        public event Delegates.ShowMessage OnEnd;
        public event Delegates.ShowMessage OnFileFinded;
        public event Delegates.ShowMessage OnDirectoryFinded;
        public event Delegates.ProcessItem OnFilteredFileFinded;
        public event Delegates.ProcessItem OnFilteredDirectoryFinded;

        public FileSystemVisitor(Delegates.Filter filter)
        {
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
            var folders = Directory.GetDirectories(path);

            foreach (var folder in folders)
            {
                var folderName = GetItemWithOutPath(folder);
                OnDirectoryFinded?.Invoke();

                if (!_filter(folderName)) continue;
                
                var operation = OnFilteredDirectoryFinded?.Invoke(folderName);

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
                    yield return "  " + subItem;
                }
            }

            var files = Directory.GetFiles(path);

            foreach (var file in files)
            {
                var fileName = GetItemWithOutPath(file);

                OnFileFinded?.Invoke();

                if (!_filter(fileName)) continue;

                var operation = OnFilteredFileFinded?.Invoke(fileName);

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
            var last = path.LastIndexOf(@"\", StringComparison.Ordinal);

            return path.Substring(last + 1);
        }
    }
}
