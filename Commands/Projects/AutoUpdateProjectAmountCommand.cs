using FinancialData.Model;

namespace FinancialData.Commands.Projects;

internal class AutoUpdateProjectAmountCommand : AbstractCommand
{
  public static void Execute(
    string dataPath,
    string name,
    DateTime date)
  {
    Console.WriteLine($"Using data path: {dataPath},");
    Console.WriteLine($"project name: {name}");
    Console.WriteLine($"and date: {date}");
    
    var financialData = ReadFinancialData(dataPath);
#pragma warning disable CA1854
    if (!financialData.Projects.ContainsKey(name))
#pragma warning restore CA1854
    {
      Console.WriteLine("No project with the given name");
      return;
    }

    var project = financialData.Projects[name];
    if (project.AutoDailyIncrease == null)
    {
      Console.WriteLine("Project has no autoupdate rule");
      return;
    }

    if (project.Amounts.Count == 0)
    {
      Console.WriteLine("Project has no amount to start from");
      return;
    }

    // Gets the last amount,
    // assuming they are already ordered by descending date
    var lastDatedAmount = project.Amounts.First();
    
    // Nothing to do if there's already an amount
    // for the requested date (or newer)
    if (lastDatedAmount.Date >= date)
      return;
    
    while (lastDatedAmount.Date < date)
    {
      var nextAmount = 0;
      lastDatedAmount = new DatedAmount(
        lastDatedAmount.Date.AddDays(1),
        nextAmount);
    }
    
    project.Amounts.Add(lastDatedAmount);
    
    SaveFinancialData(
      dataPath,
      financialData);
  }
}