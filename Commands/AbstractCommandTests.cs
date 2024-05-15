using NUnit.Framework;

namespace FinancialData.Commands;

[TestFixture]
internal class AbstractCommandTests
{
    private string _tempFilePath = "";

    [SetUp]
    public void Setup()
    {
        // Create a temporary file for testing
        _tempFilePath = Path.Combine(
            Path.GetTempPath(),
            "testfile.txt");
        File.WriteAllText(
            _tempFilePath,
            "Test content");
    }

    [TearDown]
    public void Cleanup()
    {
        // Clean up the temporary files after each test
        if (File.Exists(_tempFilePath))
        {
            File.Delete(_tempFilePath);
        }

        var directoryPath = Path.GetDirectoryName(_tempFilePath);
        var fileName = Path.GetFileNameWithoutExtension(_tempFilePath);
        var fileExtension = Path.GetExtension(_tempFilePath);

        var backupFiles = Directory.GetFiles(
            directoryPath,
            $"{fileName}-*{fileExtension}");
        foreach (var file in backupFiles)
        {
            File.Delete(file);
        }
    }

    [Test]
    public void BackupFinancialData_BackupFileCreated()
    {
        // Act
        AbstractCommand.BackupFinancialData(_tempFilePath);

        // Assert
        var directoryPath = Path.GetDirectoryName(_tempFilePath);
        var fileName = Path.GetFileNameWithoutExtension(_tempFilePath);
        var fileExtension = Path.GetExtension(_tempFilePath);

        var backupFiles = Directory.GetFiles(
            directoryPath,
            $"{fileName}-*{fileExtension}");
        Assert.That(
            1 == backupFiles.Length,
            "Backup file was not created correctly");

        var backupFilePath = backupFiles[0];
        Assert.That(
            File.Exists(backupFilePath),
            "Backup file does not exist");

        var originalContent = File.ReadAllText(_tempFilePath);
        var backupContent = File.ReadAllText(backupFilePath);
        Assert.That(originalContent == backupContent,
            "Backup file content does not match the original");
    }

    [Test]
    public void BackupFinancialData_FileDoesNotExist_NoBackupCreated()
    {
        // Arrange
        var nonExistentFilePath = Path.Combine(
            Path.GetTempPath(),
            "nonexistentfile.txt");

        // Act
        AbstractCommand.BackupFinancialData(nonExistentFilePath);

        // Assert
        var directoryPath = Path.GetDirectoryName(nonExistentFilePath);
        var fileName = Path.GetFileNameWithoutExtension(nonExistentFilePath);
        var fileExtension = Path.GetExtension(nonExistentFilePath);

        var backupFiles = Directory.GetFiles(
            directoryPath,
            $"{fileName}-*{fileExtension}");
        Assert.That(
            0 == backupFiles.Length,
            "Backup file should not be created for non-existent file");
    }
}