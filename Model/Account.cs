namespace FinancialData.Model;

internal class Account
{
  public string Name { get; set; }

  public List<DatedAmount> Amounts { get; set; }
}