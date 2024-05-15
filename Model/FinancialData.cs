namespace FinancialData.Model;

internal class FinancialData
{
  public Dictionary<string, Project> Projects { get; set; } = new();

  public Dictionary<string, Account> Accounts { get; set; } = new();

  public string GetKey(
    string name)
  {
    return name.ToLowerInvariant();
  }
  
  public void Sort()
  {
    var projects = new List<Project>(Projects.Values);
    Projects.Clear();
    projects.Sort((p1, p2) => string.Compare(p1.Name, p2.Name, StringComparison.Ordinal));
    foreach (var project in projects)
    {
      project.Sort();
      Projects[GetKey(project.Name)] = project;
    }

    var accounts = new List<Account>(Accounts.Values);
    Accounts.Clear();
    accounts.Sort((a1, a2) => string.Compare(a1.Name, a2.Name, StringComparison.Ordinal));
    foreach (var account in accounts)
    {
      account.Sort();
      Accounts[GetKey(account.Name)] = account;
    }
  }
}