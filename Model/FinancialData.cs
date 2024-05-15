namespace FinancialData.Model;

internal class FinancialData
{
  public Dictionary<string, Project> Projects { get; set; } = new();

  public Dictionary<string, Account> Accounts { get; set; } = new();
}