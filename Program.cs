using System.CommandLine;
using FinancialData.Commands.Projection;
using FinancialData.Commands.Projects;

namespace FinancialData;

public static class Program
{
  public static int Main(
    string[] args)
  {
    var rootCommand = new RootCommand(
      "Console application to analyze family financial data.");

    var commands = CreateCommands();
    foreach (var command in commands)
      rootCommand.AddCommand(command);

    // Parse the command line arguments
    return rootCommand.InvokeAsync(args).Result;
  }

  private static IEnumerable<Command> CreateCommands()
  {
    var projectedDateOption = new Option<DateTime>(
      "--projected-date",
      () => DateTime.Today);
    var dataPathOption = new Option<string>(
      "--data-path",
      () => Path.Combine(
        Directory.GetCurrentDirectory(),
        $"{AppDomain.CurrentDomain.FriendlyName}.json"));
    var nameOption = new Option<string>("--name");
    var dateOption = new Option<DateTime>(
      "--date",
      () => DateTime.Today);
    var amountOption = new Option<decimal>("--amount");

    // TODO add account
    // TODO add account amount
    // TODO regole per aumentare purpose in modo automatico (aumento mensile e massimo)

    #region Projection commands

    var projectionCommand = new Command(
      "projection",
      "Gets the projection for every balance at the given date")
    {
      dataPathOption,
      projectedDateOption
    };
    projectionCommand.SetHandler(
      ProjectionCommand.Execute,
      dataPathOption,
      projectedDateOption);

    #endregion

    #region Project commands

    var addProjectCommand = new Command(
      "project-add",
      "Adds a new financial project.")
    {
      dataPathOption,
      nameOption
    };
    addProjectCommand.SetHandler(
      AddProjectCommand.Execute,
      dataPathOption,
      nameOption);

    var listProjectCommand = new Command(
      "project-list",
      "Lists the projects")
    {
      dataPathOption
    };
    listProjectCommand.SetHandler(
      ListProjectCommand.Execute,
      dataPathOption);

    var readProjectCommand = new Command(
      "project-read",
      "Reads an existing project")
    {
      dataPathOption,
      nameOption
    };
    readProjectCommand.SetHandler(
      ReadProjectCommand.Execute,
      dataPathOption,
      nameOption);

    var addProjectAmountCommand = new Command(
      "project-add-amount",
      "Adds a new amount to an existing project.")
    {
      dataPathOption,
      nameOption,
      dateOption,
      amountOption
    };
    addProjectAmountCommand.SetHandler(
      AddProjectAmountCommand.Execute,
      dataPathOption,
      nameOption,
      dateOption,
      amountOption);

    #endregion

    #region Balance commands

    #endregion

    return new[]
    {
      // Projection
      projectionCommand,
      // Projects
      addProjectCommand,
      listProjectCommand,
      readProjectCommand,
      addProjectAmountCommand
      // Accounts
    };
  }
}