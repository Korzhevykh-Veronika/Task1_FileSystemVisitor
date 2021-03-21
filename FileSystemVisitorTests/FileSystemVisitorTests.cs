using FileSystemVisitor.FileSystemDataProviders;
using FileSystemVisitor.Models;
using Moq;
using System.Linq;
using Xunit;

namespace FileSystemVisitorTests
{
    public class FileSystemVisitorTests
    {
        [Fact]
        public void GetDirectoryInfo_FoldersWithSubFolders_SubFoldersVisited()
        {
            // Arrange
            var path = @"C:\ProgramFiles";
            var countOfFileSystemItems = 4;

            var dataProviderMock = new Mock<IFileSystemDataProvider>();

            dataProviderMock
                .Setup(provider => provider.GetFolders(path))
                .Returns(new[] { path + "\\folder1" });

            dataProviderMock
                .Setup(provider => provider.GetFolders(path + "\\folder1"))
                .Returns(new string[] { });

            dataProviderMock
                .Setup(provider => provider.GetFiles(path))
                .Returns(new[] { path + "\\file1.txt", path + "\\file2.txt" });

            dataProviderMock
                .Setup(provider => provider.GetFiles(path + "\\folder1"))
                .Returns(new[] { path + "\\folder1" + "\\file3.txt" });

            var visitor = new FileSystemVisitor.FileSystemVisitor(dataProviderMock.Object, (string name) => true);

            // Act
            var result = visitor.GetDirectoryInfo(path).ToList();

            // Assert
            Assert.Equal(countOfFileSystemItems, result.Count);
        }

        [Fact]
        public void GetDirectoryInfo_ItemWithInvalidName_ItemFilteredOut()
        {
            // Arrange
            var path = @"C:\ProgramFiles";
            var countOfFilteredItems = 1;

            var dataProviderMock = new Mock<IFileSystemDataProvider>();

            dataProviderMock
                .Setup(provider => provider.GetFolders(path))
                .Returns(new string[] { });

            dataProviderMock
                .Setup(provider => provider.GetFiles(path))
                .Returns(new[] { path + "\\file1.txt", path + "\\file2.txt" });

            bool filter(string name) => name != "file2.txt";

            var visitor = new FileSystemVisitor.FileSystemVisitor(dataProviderMock.Object, filter);

            // Act
            var result = visitor.GetDirectoryInfo(path).ToList();

            // Assert
            Assert.Equal(countOfFilteredItems, result.Count);
        }

        [Fact]
        public void GetDirectoryInfo_DefaultItem_NameWithoudPathReturned()
        {
            // Arrange
            var path = @"C:\ProgramFiles";

            var dataProviderMock = new Mock<IFileSystemDataProvider>();

            dataProviderMock
                .Setup(provider => provider.GetFolders(path))
                .Returns(new string[] { });

            dataProviderMock
                .Setup(provider => provider.GetFiles(path))
                .Returns(new[] { path + "\\file1.txt" });


            var visitor = new FileSystemVisitor.FileSystemVisitor(dataProviderMock.Object, (string name) => true);

            // Act
            var result = visitor.GetDirectoryInfo(path).ToList();

            // Assert
            Assert.Equal("file1.txt", result.Single());
        }

        [Fact]
        public void GetDirectoryInfo_DefaultItems_OnStartCalledOnce()
        {
            // Arrange
            var path = @"C:\ProgramFiles";
            var countOnStartCalled = 0;

            var dataProviderMock = new Mock<IFileSystemDataProvider>();

            dataProviderMock
                .Setup(provider => provider.GetFolders(path))
                .Returns(new string[] { });

            dataProviderMock
                .Setup(provider => provider.GetFiles(path))
                .Returns(new[] { path + "\\file1.txt" });

            var visitor = new FileSystemVisitor.FileSystemVisitor(dataProviderMock.Object, (string name) => true);

            visitor.OnStart += () => { countOnStartCalled++; };

            // Act
            var result = visitor.GetDirectoryInfo(path).ToList();

            // Assert
            Assert.Equal(1, countOnStartCalled);
        }

        [Fact]
        public void GetDirectoryInfo_DefaultItems_OnEndCalledOnce()
        {
            // Arrange
            var path = @"C:\ProgramFiles";
            var countOnEndCalled = 0;

            var dataProviderMock = new Mock<IFileSystemDataProvider>();

            dataProviderMock
                .Setup(provider => provider.GetFolders(path))
                .Returns(new string[] { });

            dataProviderMock
                .Setup(provider => provider.GetFiles(path))
                .Returns(new[] { path + "\\file1.txt" });

            var visitor = new FileSystemVisitor.FileSystemVisitor(dataProviderMock.Object, (string name) => true);

            visitor.OnEnd += () => { countOnEndCalled++; };

            // Act
            var result = visitor.GetDirectoryInfo(path).ToList();

            // Assert
            Assert.Equal(1, countOnEndCalled);
        }

        [Fact]
        public void GetDirectoryInfo_FolderWithSubFolder_OnDirectoryFoundCalled2Times()
        {
            // Arrange
            var path = @"C:\ProgramFiles";
            var countOnDirectoryFoundCalled = 0;

            var dataProviderMock = new Mock<IFileSystemDataProvider>();

            dataProviderMock
                .Setup(provider => provider.GetFolders(path))
                .Returns(new[] { path + "\\folder1" });

            dataProviderMock
                .Setup(provider => provider.GetFolders(path + "\\folder1"))
                .Returns(new string[] { path + "\\folder1" + "\\folder2" });

            dataProviderMock
                .Setup(provider => provider.GetFolders(path + "\\folder1" + "\\folder2"))
                .Returns(new string[] { });

            dataProviderMock
                .Setup(provider => provider.GetFiles(path))
                .Returns(new[] { path + "\\file1.txt", path + "\\file2.txt" });

            dataProviderMock
                .Setup(provider => provider.GetFiles(path + "\\folder1"))
                .Returns(new[] { path + "\\folder1" + "\\file3.txt" });

            var visitor = new FileSystemVisitor.FileSystemVisitor(dataProviderMock.Object, (string name) => true);

            visitor.OnDirectoryFound += () => { countOnDirectoryFoundCalled++; };

            // Act
            var result = visitor.GetDirectoryInfo(path).ToList();

            // Assert
            Assert.Equal(2, countOnDirectoryFoundCalled);
        }

        [Fact]
        public void GetDirectoryInfo_3FilesFound_OnFileFoundCalled3Times()
        {
            // Arrange
            var path = @"C:\ProgramFiles";
            var countOnFileFoundCalled = 0;

            var dataProviderMock = new Mock<IFileSystemDataProvider>();

            dataProviderMock
                .Setup(provider => provider.GetFolders(path))
                .Returns(new[] { path + "\\folder1" });

            dataProviderMock
                .Setup(provider => provider.GetFolders(path + "\\folder1"))
                .Returns(new string[] { });

            dataProviderMock
                .Setup(provider => provider.GetFiles(path))
                .Returns(new[] { path + "\\file1.txt", path + "\\file2.txt" });

            dataProviderMock
                .Setup(provider => provider.GetFiles(path + "\\folder1"))
                .Returns(new[] { path + "\\folder1" + "\\file3.txt" });

            var visitor = new FileSystemVisitor.FileSystemVisitor(dataProviderMock.Object, (string name) => true);

            visitor.OnFileFound += () => { countOnFileFoundCalled++; };

            // Act
            var result = visitor.GetDirectoryInfo(path).ToList();

            // Assert
            Assert.Equal(3, countOnFileFoundCalled);
        }

        [Fact]
        public void GetDirectoryInfo_1FilteredFile_OnFilteredFileFoundCalledOnce()
        {
            // Arrange
            var path = @"C:\ProgramFiles";
            var countOfFilteredItems = 0;

            var dataProviderMock = new Mock<IFileSystemDataProvider>();

            dataProviderMock
                .Setup(provider => provider.GetFolders(path))
                .Returns(new string[] { });

            dataProviderMock
                .Setup(provider => provider.GetFiles(path))
                .Returns(new[] { path + "\\file1.txt", path + "\\file2.txt" });

            bool filter(string name) => name != "file2.txt";

            var visitor = new FileSystemVisitor.FileSystemVisitor(dataProviderMock.Object, filter);
            visitor.OnFilteredFileFound += (string name) =>
            {
                countOfFilteredItems++;
                return SearchOperation.ContinueSearch;
            };

            // Act
            var result = visitor.GetDirectoryInfo(path).ToList();

            // Assert
            Assert.Equal(1, countOfFilteredItems);
        }

        [Fact]
        public void GetDirectoryInfo_1FilteredFolder_OnFilteredDirectoryFoundCalledOnce()
        {
            // Arrange
            var path = @"C:\ProgramFiles";
            var countOfFilteredItems = 0;

            var dataProviderMock = new Mock<IFileSystemDataProvider>();

            dataProviderMock
                .Setup(provider => provider.GetFolders(path))
                .Returns(new[] { path + "\\folder1", path + "\\folder2" });

            dataProviderMock
                .Setup(provider => provider.GetFolders(path + "\\folder1"))
                .Returns(new string[] { });

            dataProviderMock
                .Setup(provider => provider.GetFolders(path + "\\folder2"))
                .Returns(new string[] { });

            bool filter(string name) => name != "folder2";

            var visitor = new FileSystemVisitor.FileSystemVisitor(dataProviderMock.Object, filter);
            visitor.OnFilteredDirectoryFound += (string name) =>
            {
                countOfFilteredItems++;
                return SearchOperation.ContinueSearch;
            };

            // Act
            var result = visitor.GetDirectoryInfo(path).ToList();

            // Assert
            Assert.Equal(1, countOfFilteredItems);
        }

        [Fact]
        public void GetDirectoryInfo_SkipFile_FileIsNotAddedToResult()
        {
            // Arrange
            var path = @"C:\ProgramFiles";

            var dataProviderMock = new Mock<IFileSystemDataProvider>();

            dataProviderMock
                .Setup(provider => provider.GetFolders(path))
                .Returns(new string[] { });

            dataProviderMock
                .Setup(provider => provider.GetFiles(path))
                .Returns(new[] { path + "\\file1.txt", path + "\\file2.txt", path + "\\file3.txt" });

            var visitor = new FileSystemVisitor.FileSystemVisitor(dataProviderMock.Object, (string name) => true);
            visitor.OnFilteredFileFound += (string name) =>
            {
                if (name == "file2.txt")
                {
                    return SearchOperation.ExcludeItem;
                }

                return SearchOperation.ContinueSearch;
            };

            // Act
            var result = visitor.GetDirectoryInfo(path).ToList();

            // Assert
            Assert.DoesNotContain("file2.txt", result);
        }

        [Fact]
        public void GetDirectoryInfo_SkipFolder_FolderIsNotAddedToResult()
        {
            // Arrange
            var path = @"C:\ProgramFiles";

            var dataProviderMock = new Mock<IFileSystemDataProvider>();

            dataProviderMock
               .Setup(provider => provider.GetFolders(path))
               .Returns(new[] { path + "\\folder1", path + "\\folder2" });

            dataProviderMock
                .Setup(provider => provider.GetFolders(path + "\\folder1"))
                .Returns(new string[] { });

            dataProviderMock
                .Setup(provider => provider.GetFolders(path + "\\folder2"))
                .Returns(new string[] { });

            var visitor = new FileSystemVisitor.FileSystemVisitor(dataProviderMock.Object, (string name) => true);
            visitor.OnFilteredDirectoryFound += (string name) =>
            {
                if (name == "folder1")
                {
                    return SearchOperation.ExcludeItem;
                }

                return SearchOperation.ContinueSearch;
            };

            // Act
            var result = visitor.GetDirectoryInfo(path).ToList();

            // Assert
            Assert.DoesNotContain("folder1", result);
        }

        [Fact]
        public void GetDirectoryInfo_CancelAfterFirstFile_1FileAdded()
        {
            // Arrange
            var path = @"C:\ProgramFiles";

            var dataProviderMock = new Mock<IFileSystemDataProvider>();

            dataProviderMock
                .Setup(provider => provider.GetFolders(path))
                .Returns(new string[] { });

            dataProviderMock
                .Setup(provider => provider.GetFiles(path))
                .Returns(new[] { path + "\\file1.txt", path + "\\file2.txt", path + "\\file3.txt" });

            var visitor = new FileSystemVisitor.FileSystemVisitor(dataProviderMock.Object, (string name) => true);
            visitor.OnFilteredFileFound += (string name) =>
            {
                if (name == "file1.txt")
                {
                    return SearchOperation.LastItem;
                }

                return SearchOperation.ContinueSearch;
            };

            // Act
            var result = visitor.GetDirectoryInfo(path).ToList();

            // Assert
            Assert.Equal("file1.txt", result.Single());
        }

        [Fact]
        public void GetDirectoryInfo_CancelAfterFirstFolder_1FolderAdded()
        {
            // Arrange
            var path = @"C:\ProgramFiles";

            var dataProviderMock = new Mock<IFileSystemDataProvider>();

            dataProviderMock
               .Setup(provider => provider.GetFolders(path))
               .Returns(new[] { path + "\\folder1", path + "\\folder2" });

            dataProviderMock
                .Setup(provider => provider.GetFolders(path + "\\folder1"))
                .Returns(new string[] { });

            dataProviderMock
                .Setup(provider => provider.GetFolders(path + "\\folder2"))
                .Returns(new string[] { });

            var visitor = new FileSystemVisitor.FileSystemVisitor(dataProviderMock.Object, (string name) => true);
            visitor.OnFilteredDirectoryFound += (string name) =>
            {
                if (name == "folder1")
                {
                    return SearchOperation.LastItem;
                }

                return SearchOperation.ContinueSearch;
            };

            // Act
            var result = visitor.GetDirectoryInfo(path).ToList();

            // Assert
            Assert.Equal("folder1", result.Single());
        }
    }
}
