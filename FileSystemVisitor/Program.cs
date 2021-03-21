using FileSystemVisitor.EventHandlers;
using FileSystemVisitor.FileSystemDataProviders;
using System;

namespace FileSystemVisitor
{
    static class Program
    {
        static void Main(string[] args)
        {
            var startupDataProvider = new DefaultStartupDataProvider();
            var data = startupDataProvider.ProvideStartupData();

            if (data == null)
            {
                return;
            }

            bool ItemFilter(string name) => !name.EndsWith(data.ExtensionToExclude);
            var fileSystemVisitor = new FileSystemVisitor(new FileSystemDataProvider(), ItemFilter);
            var eventHandler = new FileSystemVisitorEventHandler();

            eventHandler.Initialize(fileSystemVisitor);

            var files = fileSystemVisitor.GetDirectoryInfo(data.Path);

            foreach (var file in files)
            {
                Console.WriteLine(file);
            }

            eventHandler.ShowSummary();
        }
    }
}
