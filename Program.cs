using System.CommandLine;
using FinancialData.Commands.Database;
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

    var commands = new List<Command>();

    CreateDatabaseCommands(
      commands,
      dataPathOption);
    CreateProjectionCommands(
      commands,
      dataPathOption,
      projectedDateOption
    );
    CreateProjectCommands(
      commands,
      dataPathOption,
      nameOption,
      dateOption,
      amountOption);

    return commands;
  }

  private static void CreateDatabaseCommands(
    ICollection<Command> commands,
    Option<string> dataPathOption)
  {
    var clearDatabaseCommand = new Command(
      "database-clear",
      "Clears the database, all data will be lost")
    {
      dataPathOption
    };
    clearDatabaseCommand.SetHandler(
      ClearDatabaseCommand.Execute,
      dataPathOption);
    commands.Add(clearDatabaseCommand);
  }

  private static void CreateProjectCommands(
    ICollection<Command> commands,
    Option<string> dataPathOption,
    Option<string> nameOption,
    Option<DateTime> dateOption,
    Option<decimal> amountOption)
  {
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
    commands.Add(addProjectCommand);

    var listProjectCommand = new Command(
      "project-list",
      "Lists the projects")
    {
      dataPathOption
    };
    listProjectCommand.SetHandler(
      ListProjectCommand.Execute,
      dataPathOption);
    commands.Add(listProjectCommand);

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
    commands.Add(readProjectCommand);

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
    commands.Add(addProjectAmountCommand);
    
    var autoUpdateProjectAmountCommand = new Command(
      "project-auto-update-amount",
      "Update the amount for a project up to the given date")
    {
      dataPathOption,
      nameOption,
      dateOption
    };
    autoUpdateProjectAmountCommand.SetHandler(
      AutoUpdateProjectAmountCommand.Execute,
      dataPathOption,
      nameOption,
      dateOption);
    commands.Add(autoUpdateProjectAmountCommand);
  }

  private static void CreateProjectionCommands(
    ICollection<Command> commands,
    Option<string> dataPathOption,
    Option<DateTime> projectedDateOption
  )
  {
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
    commands.Add(projectionCommand);
  }
}