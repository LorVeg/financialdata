namespace FinancialData.Commands.Projection;

internal class ProjectionCommand(string dataPath,
  DateTime projectedDate) : AbstractCommand
{
  public override async Task ExecuteAsync()
  {
    Console.WriteLine($"Using data path: {dataPath}");
    Console.WriteLine($"and date: {projectedDate}");

    var financialData = ReadFinancialData(dataPath);

    var bestProjectedAmounts = new Dictionary<string, decimal>();
    var bestAccountBalances = new Dictionary<string, decimal>();

    /*foreach (var pair in financialData.Projects)
    {
      var bestProjectedAmount = pair.Value
        .Where(p => p.Date <= projectedDate)
        .OrderByDescending(p => p.Date)
        .FirstOrDefault();

      if (bestProjectedAmount != null)
        bestProjectedAmounts[pair.Key] = bestProjectedAmount.Amount;
    }

    foreach (var pair in financialData.Accounts)
    {
      var bestAccountBalance = pair.Value
        .Where(a => a.Date <= projectedDate)
        .OrderByDescending(a => a.Date)
        .FirstOrDefault();

      if (bestAccountBalance != null)
        bestAccountBalances[pair.Key] = bestAccountBalance.Amount;
    }

    // Display results
    Console.WriteLine("Best Projected Amounts:");
    foreach (var item in bestProjectedAmounts)
      Console.WriteLine($"{item.Key}: {item.Value}");

    Console.WriteLine("Best Account Balances:");
    foreach (var item in bestAccountBalances)
      Console.WriteLine($"{item.Key}: {item.Value}");*/

    await Task.CompletedTask;
  }
}