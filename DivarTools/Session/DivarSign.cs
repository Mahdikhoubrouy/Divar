using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivarTools.Session
{
    public class DivarSign
    {
        private RestClient Client { get; set; }
        private string Number { get; set; }
        private string City { get; set; }
        private string sesstionPath { get; set; }
        public DivarSign()
        {
            Client = new RestClient("https://api.divar.ir/v5/auth/{path}");
        }

        public void SaveSessionToFile(string token)
        {
            if (!File.Exists(sesstionPath)) File.Create(sesstionPath).Close();
            File.WriteAllText(sesstionPath, token);
        }

        public string GetSessionString()
        {
            throw new NotImplementedException();
        }

        public DivarSign SaveSession(string path)
        {
            sesstionPath = path;
            return this;
        }

        public DivarSign SetCity(string city)
        {
            return this;
        }

        public DivarSign SetNumber(string number)
        {
            Number = number;
            return this;
        }

        public async Task<string> confirm(string code)
        {
            RestRequest request = new RestRequest();

            request.AddUrlSegment("path", "confirm");
            request.AddBody($"{{\"phone\":\"{Number}\",\"code\":\"{code}\"}}");
            request.AddHeader("accept", "application/json, text/plain, */*");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            var res = await Client.PostAsync(request);

            if (res.StatusCode == System.Net.HttpStatusCode.OK)
                SaveSessionToFile(res.Content!);
            return res.Content!.ToString();


            return "Error";


        }
        public async Task<bool> Authenticate()
        {
            RestRequest request = new RestRequest();

            request.AddUrlSegment("path", "authenticate");
            request.AddBody($"{{\"phone\":\"{Number}\"}}");
            request.AddHeader("accept", "application/json, text/plain, */*");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            var res = await Client.PostAsync(request);

            if (res.StatusCode == System.Net.HttpStatusCode.OK)
                return true;

            return false;
        }

    }
}
