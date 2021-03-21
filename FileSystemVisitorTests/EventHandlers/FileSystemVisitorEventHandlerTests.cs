using FileSystemVisitor;
using FileSystemVisitor.EventHandlers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace FileSystemVisitorTests
{
    public class FileSystemVisitorEventHandlerTests
    {
        [Fact]
        public void Initialize_DefaultFileSystemVisitor_AllEventsHaveHandlers()
        {
            //Arrange
            var fileSystemVisitor = new FileSystemVisitor.FileSystemVisitor(null, null);
            var eventHandler = new FileSystemVisitorEventHandler();

            // Act
            eventHandler.Initialize(fileSystemVisitor);

            //Assert
            VerifyDelegateAttachedTo(fileSystemVisitor, "OnStart");
            VerifyDelegateAttachedTo(fileSystemVisitor, "OnEnd");
            VerifyDelegateAttachedTo(fileSystemVisitor, "OnDirectoryFound");
            VerifyDelegateAttachedTo(fileSystemVisitor, "OnFileFound");
            VerifyDelegateAttachedTo(fileSystemVisitor, "OnFilteredDirectoryFound");
            VerifyDelegateAttachedTo(fileSystemVisitor, "OnFilteredFileFound");

        }

        private void VerifyDelegateAttachedTo(object objectWithEvent, string eventName)
        {
            var allBindings = BindingFlags.IgnoreCase | BindingFlags.Public |
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

            var type = objectWithEvent.GetType();
            var fieldInfo = type.GetField(eventName, allBindings);

            var value = fieldInfo.GetValue(objectWithEvent);

            var handler = value as Delegate;

            Assert.NotNull(handler);
        }
    }
}
