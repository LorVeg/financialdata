namespace FinancialData.Commands.Projects;

internal class ReadProjectCommand(string dataPath,
  string name) : AbstractCommand
{
  public override async Task ExecuteAsync(
    )
  {
    Console.WriteLine($"Using data path: {dataPath}");
    Console.WriteLine($"and project name: {name}");

    var financialData = ReadFinancialData(dataPath);
#pragma warning disable CA1854
    if (!financialData.Projects.ContainsKey(name))
#pragma warning restore CA1854
    {
      Console.WriteLine("No project with the given name");
      return;
    }

    var project = financialData.Projects[name];
    Console.WriteLine($"Project name: {project.Name}");
    if (project.Amounts.Count == 0)
    {
      Console.WriteLine("No amounts in the project");
      return;
    }

    foreach (var amount in project.Amounts)
      Console.WriteLine($"{amount.Date} {amount.Amount}");

    await Task.CompletedTask;
  }
}