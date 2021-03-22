using FileSystemVisitor.Models;

namespace FileSystemVisitor.StartupDataProviders
{
    public class DefaultStartupDataProvider
    {
        private const string DefaultPath = @"C:\Users\Veronika_Korzhevykh\Pictures";
        private const string DefaultExtensionToExclude = @".txt";

        public StartupData ProvideStartupData()
        {
            var data = new StartupData
            {
                Path = DefaultPath,
                ExtensionToExclude = DefaultExtensionToExclude
            };

            return data;
        }
    }
}
