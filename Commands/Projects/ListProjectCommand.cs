namespace FinancialData.Commands.Projects;

internal class ListProjectCommand(string dataPath) : AbstractCommand
{
  public override async Task ExecuteAsync(
    )
  {
    Console.WriteLine($"Using data path: {dataPath}");

    var financialData = ReadFinancialData(dataPath);
    if (financialData.Projects.Keys.Count == 0)
    {
      Console.WriteLine("No projects found");
      return;
    }

    var projects = financialData.Projects.Values.ToList();
    projects.Sort(
      (
        p1,
        p2) => string.Compare(
        p1.Name,
        p2.Name,
        StringComparison.Ordinal));

    foreach (var project in projects)
      Console.WriteLine(project.Name);

    await Task.CompletedTask;
  }
}