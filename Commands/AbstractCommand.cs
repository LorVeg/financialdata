using System.Runtime.ConstrainedExecution;
using Newtonsoft.Json;

namespace FinancialData.Commands;

internal class AbstractCommand
{
  protected static Model.FinancialData ReadFinancialData(
    string dataPath)
  {
    if (!File.Exists(dataPath))
      return new Model.FinancialData();
    var jsonData = File.ReadAllText(dataPath);
    var financialData =
      JsonConvert.DeserializeObject<Model.FinancialData>(jsonData);
    if (financialData == null)
      throw new ApplicationException(
        "Could not read financial data from the given file");
    
    return financialData;
  }

  protected static void BackupFinancialData(
    string dataPath)
  {
    if (!File.Exists(dataPath))
      return;

    var directoryPath = Path.GetDirectoryName(dataPath);
    var fileName = Path.GetFileNameWithoutExtension(dataPath);
    var fileExtension = Path.GetExtension(dataPath);

    var fileNameAndExtensions = $"{fileName}-{DateTime.Now:MM-dd-yyyy HH:mm:ss}{fileExtension}";
    var backupFilePath = directoryPath == null ?  fileNameAndExtensions : Path.Combine(
      directoryPath,fileNameAndExtensions);
    File.Copy(dataPath, backupFilePath);
  }

  protected static void SaveFinancialData(
    string dataPath,
    Model.FinancialData financialData)
  {
    BackupFinancialData(dataPath);

    financialData.Sort();

    var jsonData = JsonConvert.SerializeObject(financialData);
    File.WriteAllText(
      dataPath,
      jsonData);
  }
}