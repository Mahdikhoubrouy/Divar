using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivarCollector
{
    public static class Tools
    {
        public static LiveDisplayContext ctx { get; set; }
        public static Table table { get; set; }

        public static void SetLiveDisplayContextForStatus(this LiveDisplayContext live)
        {
            ctx = live;
        }
        public static string SessionSplitor(this string data,string SessionFolder)
        {
            return data.Split($"{SessionFolder}")[1].Replace("/", "").Replace("\\", "");
        }
        public static void ErrorLogger(string errorMessage)
        {
            AnsiConsole.Foreground = Color.Red;
            AnsiConsole.Write(" - Error :");
            AnsiConsole.WriteLine(errorMessage);
            AnsiConsole.ResetColors();
        }

        public static void DNCLogger(string text)
        {
            AnsiConsole.Foreground = Color.Purple;
            AnsiConsole.Write(" #-Divar Number Collector Error : ");
            AnsiConsole.WriteLine(text);
            AnsiConsole.ResetColors();
        }

        public static void SessionLogger(string text)
        {
            AnsiConsole.Foreground = Color.Orange1;
            AnsiConsole.Write(" !-Session Error : ");
            AnsiConsole.WriteLine(text);
            AnsiConsole.ResetColors();
        }


        public static void LiveStatusRepoter(string tableName,string data)
        {
            if(tableName == "collected")
            {
                table.UpdateCell(0, 0, data);
                ctx.Refresh();
            }else if(tableName == "log")
            {
                table.UpdateCell(0,2,data);
                ctx.Refresh();
            }
            else if(tableName == "caption")
            {
                table.Caption(data);
                ctx.Refresh();
            }
            else
            {
                table.UpdateCell(0, 1, data);
                ctx.Refresh();
            }
            ctx.UpdateTarget(table);
        }
    }
}
