namespace FinancialData.Model;

internal class Project
{
  public Project()
  {}

  public Project(
    string name)
  {
    Name = name;
  }

  public string Name { get; set; } = "";

  public string? AutoDailyIncrease { get; set; }

  public List<DatedAmount> Amounts { get; set; } = new();
}