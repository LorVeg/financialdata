namespace FinancialData.Commands.Database;

// ReSharper disable once ClassNeverInstantiated.Global
internal class ClearDatabaseCommand(string dataPath) : AbstractCommand
{
  public override async Task ExecuteAsync()
  {
    BackupFinancialData(dataPath);
    
    if (File.Exists(dataPath))
      File.Delete(dataPath);

    await Task.CompletedTask;
  }
}