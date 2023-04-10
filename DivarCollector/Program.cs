using DivarCollector;
using DivarTools;
using DivarTools.Scraper;
using DivarTools.Session;
using OpenQA.Selenium;
using Spectre.Console;
using System.Net;
using System.Text.RegularExpressions;

AnsiConsole.MarkupLine("\n\n" + @"[red][slowblink]______ _                   _____       _ _           _             
|  _  (_)                 /  __ \     | | |         | |            
| | | |___   ____ _ _ __  | /  \/ ___ | | | ___  ___| |_ ___  _ __ 
| | | | \ \ / / _` | '__| | |    / _ \| | |/ _ \/ __| __/ _ \| '__|
| |/ /| |\ V / (_| | |    | \__/\ (_) | | |  __/ (__| || (_) | |   
|___/ |_| \_/ \__,_|_|     \____/\___/|_|_|\___|\___|\__\___/|_|   
                                                                   
                                                                   [/][/]");

AnsiConsole.MarkupLine("  [bold][Cyan1]Github : https://github.com/miticyber/divar [/][/]\n\n");

string RunTimePath = Environment.CurrentDirectory;
string SessionFolder = Path.Combine(RunTimePath, "Sessions");

if (!Directory.Exists(SessionFolder)) Directory.CreateDirectory(SessionFolder);

var whatsection = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .HighlightStyle(Style.Plain.Foreground(Color.Red1))
        .Title(" Please select one of the [green]options[/]?")
        .PageSize(10)
        .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
        .AddChoices(new[] { "Add Session", "Collect Number", "Exit" }));


if (whatsection == "Login")
{
    AnsiConsole.WriteLine("Welcome, in this section you can create and save your session");

    var number = AnsiConsole.Ask<string>("What's your [green]Phone Number[/]?");

    if (!AnsiConsole.Confirm("Phone Number Is Correct?"))
    {
        AnsiConsole.MarkupLine("Ok Exited..:(");
        Environment.Exit(0);
    }

    bool res = false;
    var divar = new DivarSign().SetNumber(number).SaveSession(Path.Combine(SessionFolder, number + ".txt"));

    try
    {
        res = await divar.Authenticate();
    }
    catch (Exception ex)
    {
        Tools.ErrorLogger(ex.Message);

        Environment.Exit(0);
    }

    if (res)
    {
        try
        {
            var code = AnsiConsole.Ask<string>("\n[green]Code sent to your number[/] please [bold]enter the [red]code[/][/]?");

            var finalres = await divar.confirm(code);

            if (finalres != "Error")
            {
                AnsiConsole.MarkupLine($"\n\nOperation [green]Successful[/]. your session is saved in this [purple]folder[/] : {SessionFolder}");
            }
            else
            {
                AnsiConsole.MarkupLine("\n\n[italic] Operation [red]Failed[/][/]");
            }
        }
        catch (Exception ex)
        {
            Tools.ErrorLogger(ex.Message);
        }

        Environment.Exit(0);
    }

}
else if (whatsection == "Collect Number")
{
    var whatMethod = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title(" Please select one of the following [green]options[/]?")
        .HighlightStyle(Style.Plain.Foreground(Color.Red1))
        .PageSize(10)
        .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
        .AddChoices(new[] { "By Url", "By City", "By Category" }));


    if (whatMethod == "WithUrl")
    {
        AnsiConsole.Markup("\nPlease enter the [red]URL[/] ? : ");
        string? url = Console.ReadLine();
        if (Regex.IsMatch(url, "https?:\\/\\/(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b([-a-zA-Z0-9()!@:%_\\+.~#?&\\/\\/=]*)"))
        {


            var SessionsLocation = Directory.GetFiles(SessionFolder, "*.txt");
            var SessionName = SessionsLocation.Select(x => x.Split($"{SessionFolder}")[1].Replace("/", "").Replace("\\", ""));

            if (SessionsLocation.Length > 0)
            {
                AnsiConsole.WriteLine("\n\n");
                var SessionsSelected = AnsiConsole.Prompt(
                           new MultiSelectionPrompt<string>()
               .Title("[purple3]Which of the following sessions should the operation be performed with?[/]")
               .PageSize(10)
               .HighlightStyle(Style.Plain.Foreground(Color.Red1))
               .MoreChoicesText("[grey](Move up and down to reveal more sessions)[/]")
               .Required()
               .InstructionsText(
                   "[grey](Press [blue]<space>[/] to select a Session, " +
                   "[green]<enter>[/] to accept)[/]")
               .AddChoiceGroup("Select All", SessionName));
                var dnc = new DivarNumberCollector()
                    .SetLogger(Tools.DNCLogger)
                    .SetStatusReporter(Tools.LiveStatusRepoter)
                    .SetSessionName(SessionsSelected)
                    .SetUrl(url);
                SessionReader.Logger = Tools.SessionLogger;


                var sessions = SessionReader.
                    GetAllSession(SessionsLocation
                    .Where(x => x.SessionSplitor(SessionFolder) == SessionsSelected
                        .SingleOrDefault(m => m == x.SessionSplitor(SessionFolder))).Select(x => x)
                        .ToArray());

                var table = new Table().Centered();
                Tools.table = table;
                table.Title(new TableTitle("[slowblink]Status[/]", Style.Plain.Foreground(Color.Aqua)));
                table.BorderStyle(Style.Plain.Foreground(Color.Magenta2));
                table.BorderStyle(Style.Plain.Decoration(Decoration.SlowBlink));
                AnsiConsole.Live(table)
                    .AutoClear(false)
                    .Overflow(VerticalOverflow.Ellipsis)
                    .Cropping(VerticalOverflowCropping.Top)
                    .Start(ctx =>
                    {
                        Tools.ctx = ctx;

                        table.AddColumn("Collected");
                        table.AddColumn("Session");
                        table.AddColumn("Log");
                        ctx.Refresh();
                        table.AddRow("0", "none","none");
                        table.DoubleBorder();
                        ctx.Refresh();
                        dnc.GetNumbers(sessions);
                    });



            }
            else
            {
                AnsiConsole.MarkupLine("[italic][maroon]No sessions were found in the session folder![/][/]");
            }

        }
        else
        {
            AnsiConsole.MarkupLine("\n\n   [red]URL [bold]Invalid[/] ![/]");
        }
    }
    else
    {
        AnsiConsole.MarkupLine(" [italic][green]This section is not complete yet, coming soon...[/][/]");
    }
}
else if (whatsection == "Exit")
{
    AnsiConsole.MarkupLine("[italic][fuchsia] - Good[/] [skyblue1]Bye :) [/][/]");
    Environment.Exit(0);
}
