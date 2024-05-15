using FinancialData.Model;

namespace FinancialData.Commands.Projects;

internal class AddProjectAmountCommand(string dataPath,
  string name,
  DateTime date,
  decimal amount) : AbstractCommand
{
  public override async Task ExecuteAsync()
  {
    Console.WriteLine($"Using data path: {dataPath},");
    Console.WriteLine($"project name: {name},");
    Console.WriteLine($"date: {date}");
    Console.WriteLine($"and amount: {amount}");
    
    var financialData = ReadFinancialData(dataPath);
#pragma warning disable CA1854
    if (!financialData.Projects.ContainsKey(name))
#pragma warning restore CA1854
    {
      Console.WriteLine("No project with the given name");
      return;
    }

    var project = financialData.Projects[name];
    project.Amounts.Add(
      new DatedAmount(
        date,
        amount));

    SaveFinancialData(
      dataPath,
      financialData);

    await Task.CompletedTask;
  }
}