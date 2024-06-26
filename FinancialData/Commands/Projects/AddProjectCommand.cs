using FinancialData.Model;

namespace FinancialData.Commands.Projects;

internal class AddProjectCommand(string dataPath,
  string name) : AbstractCommand
{
  public override async Task ExecuteAsync()
  {
    Console.WriteLine($"Using data path: {dataPath}");
    Console.WriteLine($"and project name: {name}");

    var projectKey = name.ToLowerInvariant();

    var financialData = ReadFinancialData(dataPath);
    if (financialData.Projects.ContainsKey(projectKey))
      return;

    var project = new Project(name);
    financialData.Projects[name] = project;

    SaveFinancialData(
      dataPath,
      financialData);

    await Task.CompletedTask;
  }
}