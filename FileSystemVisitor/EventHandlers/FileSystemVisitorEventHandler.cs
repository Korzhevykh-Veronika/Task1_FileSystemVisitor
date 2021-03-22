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

        private int _directoryCount;
        private int _fileCount;

        private int _filteredDirectoryCount;
        private int _filteredFileCount;

        private int _excludedDirectoryCount;
        private int _excludedFilesCount;

        public void Initialize(FileSystemVisitor fileSystemVisitor)
        {
            fileSystemVisitor.OnStart += () => ConsoleExtensions.WriteLineLog("Process has been started \n");
            fileSystemVisitor.OnEnd += () => ConsoleExtensions.WriteLineLog("\nProcess has been finished \n");
            fileSystemVisitor.OnDirectoryFound += () => _directoryCount++;
            fileSystemVisitor.OnFileFound += () => _fileCount++;
            fileSystemVisitor.OnFilteredDirectoryFound += name =>
            {
                _filteredDirectoryCount++;

                if (name.Length <= MinFolderNameLength)
                {
                    _excludedDirectoryCount++;
                    return SearchOperation.ExcludeItem;
                }

                return SearchOperation.ContinueSearch;
            };
            fileSystemVisitor.OnFilteredFileFound += name =>
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
