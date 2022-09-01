using DivarTools.Json;
using DivarTools.Session.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivarTools.Session
{
    public static class SessionReader
    {
        public static Action<string> Logger { get; set; }
        private static string sessionError { get; set; }
        private static List<string> ReadSession(string[] path)
        {
            var sessions = new List<string>();
            try
            {
                foreach (var p in path)
                {
                    sessions.Add(FileWorker.ReadTokenFromFile(p));
                }

                return sessions;
            }
            catch (Exception ex)
            {
                sessionError = ex.Message;
                return sessions;
            }
        }

        public static List<TokenModel> GetAllSession(string[] path)
        {
            List<TokenModel> sessions = new List<TokenModel>();

            var GetSession = ReadSession(path);

            if (!string.IsNullOrWhiteSpace(sessionError))
                Logger.Invoke(sessionError);

            foreach(var session in GetSession)
            {
                sessions.Add(Serializer.Jsonserializer<TokenModel>(session));
            }

            return sessions;

        }
    }
}
