namespace FinancialData.Model;

internal class DatedAmount
{
  public DatedAmount()
  {
  }

  public DatedAmount(
    DateTime date,
    decimal amount)
  {
    Date = date.Date;
    Amount = amount;
  }

  public DateTime Date { get; set; }

  public decimal Amount { get; set; }
}