namespace FinancialData.Model;

internal class Account
{
  public string Name { get; set; } = "";

  public List<DatedAmount> Amounts { get; set; } = [];

  public void Sort()
  {
    Amounts.Sort((a1, a2) => a2.Date.CompareTo(a1.Date));
  }
}