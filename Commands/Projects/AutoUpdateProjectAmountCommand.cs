using FinancialData.Model;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Nito.AsyncEx.Synchronous;

namespace FinancialData.Commands.Projects;

internal class AutoUpdateProjectAmountCommand(string dataPath,
  string name,
  DateTime date) : AbstractCommand
{
  public override async Task  ExecuteAsync(
    )
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
      var nextAmountTask = EvaluateExpressionAsync(project.AutoDailyIncrease, lastDatedAmount.Amount);
      var nextAmount = nextAmountTask.WaitAndUnwrapException();
      
      lastDatedAmount = new DatedAmount(
        lastDatedAmount.Date.AddDays(1),
        nextAmount);
    }
    
    project.Amounts.Add(lastDatedAmount);
    
    SaveFinancialData(
      dataPath,
      financialData);

    await Task.CompletedTask;
  }
  
  private static async Task<decimal> EvaluateExpressionAsync(string expressionBody, decimal amount)
  {
    var result = await CSharpScript.EvaluateAsync<decimal>(
      expressionBody,
      globals: new ScriptGlobals { amount = amount }
    );
    return result;
  }
}

internal class ScriptGlobals
{
  // ReSharper disable once InconsistentNaming
  public decimal amount;
}