using FinancialData.Model;

namespace FinancialData.Commands.Projects;

internal class AddProjectAmountCommand : AbstractCommand
{
  public static void Execute(
    string dataPath,
    string name,
    DateTime date,
    decimal amount)
  {
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
  }
}