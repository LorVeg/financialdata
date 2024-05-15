namespace FinancialData.Commands.Database;

// ReSharper disable once ClassNeverInstantiated.Global
internal class ClearDatabaseCommand : AbstractCommand
{
  public static void Execute(
    string dataPath)
  {
    BackupFinancialData(dataPath);
    
    if (File.Exists(dataPath))
      File.Delete(dataPath);
  }
}