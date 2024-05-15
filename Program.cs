using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BudgetAnalyzer;

public class DatedAmount
{
    public DatedAmount() {}

    public DatedAmount(DateTime date, decimal amount) {
        Date = date.Date;
        Amount = amount;
    }

    public DateTime Date { get; set; }

    public decimal Amount { get; set; }
}

public class Project {
    public string Name { get; set; }

    public string? AutoDailyIncrease { get; set; }

    public List<DatedAmount> Amounts { get; set; }
}

public class Account {
    public string Name { get; set; }

    public List<DatedAmount> Amounts { get; set; }
}

public class FinancialData
{
    public Dictionary<string, List<Project>> Projects { get; set; } = new();

    public Dictionary<string, List<Account>> Accounts { get; set; } = new();
}

class Program
{
    static int Main(string[] args)
    {
        var rootCommand = new RootCommand("Console application to analyze family financial data.");

        var commands = CreateCommands();
        foreach (var command in commands)
            rootCommand.AddCommand(command);

        // Parse the command line arguments
        return rootCommand.InvokeAsync(args).Result;
    }

    private static IEnumerable<Command> CreateCommands()
    {
        var projectedDateOption = new Option<DateTime>("--projected-date", getDefaultValue: () => DateTime.Today);
        var dataPathOption = new Option<string>("--data-path", getDefaultValue: () => Path.Combine(Directory.GetCurrentDirectory(), $"{AppDomain.CurrentDomain.FriendlyName}.json"));
        var nameOption = new Option<string>("--name");
        var dateOption = new Option<DateTime>("--date", getDefaultValue: () => DateTime.Today);
        var amountOption = new Option<decimal>("--amount");

        // TODO add account
        // TODO add account amount
        // TODO regole per aumentare purpose in modo automatico (aumento mensile e massimo)

        #region Projection commands

        var projectionCommand = new Command("projection", "Gets the projection for every balance at the given date")
            {
                dataPathOption,
                projectedDateOption,
            };
        projectionCommand.SetHandler(HandleGetProjectionCommand,
            dataPathOption, projectedDateOption);

        #endregion

        #region Project commands

        var addProjectCommand = new Command("project-add", "Adds a new financial project.")
        {
            dataPathOption,
            nameOption
        };
        addProjectCommand.SetHandler(HandleAddProjectCommand, dataPathOption, nameOption);

        var listProjectCommand = new Command("project-list", "Lists the projects") {
            dataPathOption
        };
        listProjectCommand.SetHandler(HandleListProjectCommand, dataPathOption);

        var readProjectCommand = new Command("project-read", "Reads an existing project")
        {
            dataPathOption,
            nameOption
        };
        readProjectCommand.SetHandler(HandleReadProjectCommand, dataPathOption, nameOption);

        var addProjectAmountCommand = new Command("project-add-amount", "Adds a new amount to an existing project.")
        {
            dataPathOption,
            nameOption,
            dateOption,
            amountOption,
        };
        addProjectAmountCommand.SetHandler(HandleAddProjectAmountCommand, dataPathOption, nameOption, dateOption, amountOption);

        #endregion

        #region Balance commands

        #endregion

        return new Command[] {
            projectionCommand,
            addProjectCommand,
            listProjectCommand,
            readProjectCommand,
            addProjectAmountCommand
        };
    }

    private static void HandleGetProjectionCommand(String dataPath, DateTime projectedDate)
    {
        Console.WriteLine($"Using data path: {dataPath}");
        Console.WriteLine($"and date: {projectedDate}");

        var financialData = ReadFinancialData(dataPath);

        var bestProjectedAmounts = new Dictionary<string, decimal>();
        var bestAccountBalances = new Dictionary<string, decimal>();

        foreach (var pair in financialData.Projects)
        {
            var bestProjectedAmount = pair.Value
                .Where(p => p.Date <= projectedDate)
                .OrderByDescending(p => p.Date)
                .FirstOrDefault();

            if (bestProjectedAmount != null)
            {
                bestProjectedAmounts[pair.Key] = bestProjectedAmount.Amount;
            }
        }

        foreach (var pair in financialData.Accounts)
        {
            var bestAccountBalance = pair.Value
                .Where(a => a.Date <= projectedDate)
                .OrderByDescending(a => a.Date)
                .FirstOrDefault();

            if (bestAccountBalance != null)
            {
                bestAccountBalances[pair.Key] = bestAccountBalance.Amount;
            }
        }

        // Display results
        Console.WriteLine("Best Projected Amounts:");
        foreach (var item in bestProjectedAmounts)
        {
            Console.WriteLine($"{item.Key}: {item.Value}");
        }

        Console.WriteLine("Best Account Balances:");
        foreach (var item in bestAccountBalances)
        {
            Console.WriteLine($"{item.Key}: {item.Value}");
        }

    }

    private static void HandleAddProjectCommand(string dataPath, string name)
    {
        Console.WriteLine($"Using data path: {dataPath}");
        Console.WriteLine($"and project name: {name}");

        var financialData = ReadFinancialData(dataPath);
        if (financialData.Projects.ContainsKey(name))
            return;

        financialData.Projects[name] = new List<DatedAmount>();

        SaveFinancialData(dataPath, financialData);
    }

    private static void HandleListProjectCommand(string dataPath)
    {
        Console.WriteLine($"Using data path: {dataPath}");

        var financialData = ReadFinancialData(dataPath);
        if (financialData.Projects.Keys.Count == 0)
        {
            Console.WriteLine("No purpose");
            return;
        }

        foreach (var purposeKey in financialData.Projects.Keys)
            Console.WriteLine(purposeKey);
    }

    private static void HandleReadProjectCommand(string dataPath, string name)
    {

        Console.WriteLine($"Using data path: {dataPath}");
        Console.WriteLine($"and project name: {name}");

        var financialData = ReadFinancialData(dataPath);
        if (!financialData.Projects.ContainsKey(name))
        {
            Console.WriteLine("No project with the given name");
            return;
        }

        var amounts = financialData.Projects[name];
        if (amounts.Count == 0)
        {
            Console.WriteLine("No amounts in the project");
            return;
        }
        foreach (var amount in amounts)
        {
            Console.WriteLine($"{amount.Date} {amount.Amount}");
        }
    }

    private static void HandleAddProjectAmountCommand(string dataPath, string name, DateTime date, decimal amount)
    {
        var financialData = ReadFinancialData(dataPath);
        if (!financialData.Projects.ContainsKey(name))
        {
            Console.WriteLine("No project with the given name");
            return;
        }

        var project = financialData.Projects[name];
        project.Add(new DatedAmount(date, amount));

        project.Sort((i1, i2) => i2.Date.CompareTo(i1.Date));

        SaveFinancialData(dataPath, financialData);
    }

    private static FinancialData ReadFinancialData(string dataPath)
    {
        if (!File.Exists(dataPath))
            return new FinancialData();
        var jsonData = File.ReadAllText(dataPath);
        var financialData = JsonConvert.DeserializeObject<FinancialData>(jsonData);
        return financialData;
    }

    private static void SaveFinancialData(string dataPath, FinancialData financialData)
    {
        if (File.Exists(dataPath))
        {
            // crea backup
        }

        var jsonData = JsonConvert.SerializeObject(financialData);
        File.WriteAllText(dataPath, jsonData);
    }
}
