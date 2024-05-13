namespace Toolkits.Core.Tests;

[TestClass]
public class FolderTests
{
    [Test]
    public void Combine_ShouldReturnCombinedPath()
    {
        // Arrange
        var folder = new Folder("C:\\Folder1");
        var path1 = "Subfolder1";
        var path2 = "Subfolder2";
        var expectedPath = "C:\\Folder1\\Subfolder1\\Subfolder2";

        // Act
        var combinedFolder = folder.Combine(path1, path2);

        // Assert
        Assert.AreEqual(expectedPath, combinedFolder.ToString());
    }

    [Test]
    public void CombinePaths_ShouldReturnCombinedPath()
    {
        // Arrange
        var path1 = "C:\\Folder1";
        var path2 = "Subfolder1";
        var path3 = "Subfolder2";
        var expectedPath = "C:\\Folder1\\Subfolder1\\Subfolder2";

        // Act
        var combinedFolder = Folder.CombinePaths(path1, path2, path3);

        // Assert
        Assert.AreEqual(expectedPath, combinedFolder.ToString());
    }

    [Test]
    public void TryCreateFolder_ShouldCreateDirectoryIfNotExists()
    {
        // Arrange
        var folderPath = "C:\\NewFolder";

        // Act
        var folder = new Folder(folderPath);
        folder.TryCreateFolder();

        // Assert
        Assert.AreEqual(true, Directory.Exists(folderPath));

        Directory.Delete(folder);
    }
}
