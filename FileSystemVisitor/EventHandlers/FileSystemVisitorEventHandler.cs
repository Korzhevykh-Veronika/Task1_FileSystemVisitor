using System;
using FileSystemVisitor.Models;
using FileSystemVisitor.Utils;

namespace FileSystemVisitor.EventHandlers
{
    public class FileSystemVisitorEventHandler
    {
        private const int MinFolderNameLength = 4;
        private const int MaxFilesToDisplayCount = 5;
        private const string FileToExcludeNameStart = "f";

        private int _directoryCount = 0;
        private int _fileCount = 0;

        private int _filteredDirectoryCount = 0;
        private int _filteredFileCount = 0;

        private int _excludedDirectoryCount = 0;
        private int _excludedFilesCount = 0;

        public void Initialize(FileSystemVisitor fileSystemVisitor)
        {
            fileSystemVisitor.OnStart += () => ConsoleExtensions.WriteLineLog("Process has been started \n");
            fileSystemVisitor.OnEnd += () => ConsoleExtensions.WriteLineLog("\nProcess has been finished \n");
            fileSystemVisitor.OnDirectoryFinded += () => _directoryCount++;
            fileSystemVisitor.OnFileFinded += () => _fileCount++;
            fileSystemVisitor.OnFilteredDirectoryFinded += (name) =>
            {
                _filteredDirectoryCount++;

                if (name.Length <= MinFolderNameLength)
                {
                    _excludedDirectoryCount++;
                    return SearchOperation.ExcludeItem;
                }

                return SearchOperation.ContinueSearch;
            };
            fileSystemVisitor.OnFilteredFileFinded += (name) =>
            {
                _filteredFileCount++;

                if (_filteredFileCount >= MaxFilesToDisplayCount)
                {
                    return SearchOperation.LastItem;
                }

                if (name.StartsWith(FileToExcludeNameStart))
                {
                    _excludedFilesCount++;
                    return SearchOperation.ExcludeItem;
                }

                return SearchOperation.ContinueSearch;
            };
        }

        public void ShowSummary()
        {
            ConsoleExtensions.WriteLineSummary("Directories found: " + _directoryCount);
            ConsoleExtensions.WriteLineSummary("Files found: " + _fileCount);
            ConsoleExtensions.WriteLineSummary("Filtered directories found: " + _filteredDirectoryCount);
            ConsoleExtensions.WriteLineSummary("Filtered files found: " + _filteredFileCount);

            Console.WriteLine("");

            ConsoleExtensions.WriteLineSummary("Excluded directories: " + _excludedDirectoryCount);
            ConsoleExtensions.WriteLineSummary("Excluded files: " + _excludedFilesCount);
        }
    }
}
