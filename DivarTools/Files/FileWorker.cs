using DivarTools.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivarTools.Session.Files
{
    internal static class FileWorker
    {
        public static string ReadTokenFromFile(string path)
        {
            return File.ReadAllText(path);
        }

        public static void WriteNumberToFile(string path,string number)
        {
            File.AppendAllText(path, number+"\n");
        }
    }
}
