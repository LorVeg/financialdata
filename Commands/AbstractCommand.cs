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
    return financialData;
  }

  protected static void SaveFinancialData(
    string dataPath,
    Model.FinancialData financialData)
  {
    if (File.Exists(dataPath))
    {
      // crea backup
    }
    
    // TODO ordina i dati prima di salvarli

    var jsonData = JsonConvert.SerializeObject(financialData);
    File.WriteAllText(
      dataPath,
      jsonData);
  }
}