using System;
using FileSystemVisitor.Models;
using FileSystemVisitor.Utils;

namespace FileSystemVisitor.StartupDataProviders
{
    public class ConsoleStartupDataProvider
    {
        public StartupData ProvideStartupData()
        {
            var data = new StartupData();

            Console.Write("Input the path to the folder: ");
            string path = Console.ReadLine();
            var validator = new InputValidator();

            var isPathValid = validator.IsPathValid(path);

            if (isPathValid)
            {
                ConsoleExtensions.WriteLineSuccess("Validation passed");
            }
            else
            {
                ConsoleExtensions.WriteLineFail("Incorrect path to the folder!");
                return null;
            }

            data.Path = path;

            Console.Write("Input the file extension you would like to exclude (.txt, .png): ");
            string fileExtension = Console.ReadLine();
            var isExtensionValid = validator.IsExtensionValid(fileExtension);

            if (isExtensionValid)
            {
                ConsoleExtensions.WriteLineSuccess("Validation passed");
            }
            else
            {
                ConsoleExtensions.WriteLineFail("Incorrect extension file!");
                return null;
            }

            data.ExtensionToExclude = fileExtension;

            return data;
        }
    }
}
