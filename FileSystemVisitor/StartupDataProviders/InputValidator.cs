using System.IO;
using System.Text.RegularExpressions;

namespace FileSystemVisitor.StartupDataProviders
{
    public class InputValidator
    {
        private const string ValidPathPattern = @"^[a-zA-Z]:(\\[a-zA-Z0-9_а-яА-я]+)*$";

        public bool IsPathValid(string path)
        {
            return Regex.IsMatch(path, ValidPathPattern);
        }

        public bool IsExtensionValid(string fileExtension)
        {
            string extension = Path.GetExtension(fileExtension);

            return (!string.IsNullOrEmpty(extension) || fileExtension == string.Empty);
        }
    }
}
