using RestSharp;
using DivarTools.Json;
using DivarTools.Scraper;
using DivarTools.Session.Files;

namespace DivarTools
{
    public class DivarNumberCollector
    {

        #region Properties
        private string ScrapError { get; set; }
        private string ParseError { get; set; }
        private string url { get; set; }
        private string GetNumberError { get; set; }
        private string serializerError { get; set; }
        private Action<string> Logger { get; set; }
        private Action<string, string> StatusReporter { get; set; }
        private List<string> SessionName { get; set; }
        #endregion

        #region Fluent
        public DivarNumberCollector SetUrl(string Url)
        {
            url = Url;
            return this;
        }

        public DivarNumberCollector SetLogger(Action<string> logger)
        {
            Logger = logger;
            return this;
        }

        public DivarNumberCollector SetStatusReporter(Action<string, string> statusreporter)
        {
            StatusReporter = statusreporter;
            return this;
        }

        public DivarNumberCollector SetSessionName(List<string> sessionname)
        {
            SessionName = sessionname;
            return this;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Calling Scraper With Try and Catch
        /// </summary>
        /// <param name="url"></param>
        /// <param name="PageDownCount"></param>
        /// <returns>Source page divar</returns>
        private string CallScraper(string url, int PageDownCount)
        {
            try
            {
                var scraper = new Scrapering(Enums.Drivers.Firefox);
                var srouce = scraper.WebScraper(url, PageDownCount);
                return srouce;
            }
            catch (Exception ex)
            {
                ScrapError = ex.Message;
                return string.Empty;
            }


        }


        /// <summary>
        /// Get a unique code for each post
        /// </summary>
        /// <param name="source"></param>
        /// <returns>A List string => Hash Codes</returns>
        private List<string> GetHashCode(string source)
        {
            try
            {
                var HtmlParser = new HTMLParser();

                return HtmlParser.ParsAndGetPostCode(source);
            }
            catch (Exception ex)
            {
                ParseError = ex.Message;
                return new List<string>();
            }
        }


        /// <summary>
        /// Get number with Hash code
        /// </summary>
        /// <param name="hashCode"></param>
        /// <param name="session"></param>
        /// <returns>DivarApi result</returns>
        private string GetNumber(string hashCode, string session)
        {
            try
            {
                var client = new RestClient();

                var request = new RestRequest($"https://api.divar.ir/v5/posts/{hashCode}/contact/");
                request.AddHeader("cookie", $"token={session}");
                var res = client.Get(request);

                return res.Content!;
            }
            catch (Exception ex)
            {
                GetNumberError = ex.Message;
                return string.Empty;
            }
        }


        /// <summary>
        /// Calling a phone number with a session will go to another session if the session is limited
        /// </summary>
        /// <param name="sessions"></param>
        public void GetNumbers(List<TokenModel> sessions)
        {
            int collected = 0;
            string path = Path.Combine(Environment.CurrentDirectory, "NumbersCollec");
            string FileName = Path.Combine(path, $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}={DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}.txt");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            if (!File.Exists(FileName)) File.Create(FileName).Close();
            bool loopval = true;
            int pageDownNumber = 0;
            int sesssionId = 0;
            StatusReporter.Invoke("caption", "Processing ..");
            while (loopval)
            {
                var src = CallScraper(url, pageDownNumber);

                if (!string.IsNullOrWhiteSpace(ScrapError))
                    StatusReporter.Invoke("log", ScrapError);

                var HashCode = GetHashCode(src);

                if (!string.IsNullOrWhiteSpace(ParseError))
                    StatusReporter.Invoke("log", ParseError);
                bool ifVal = true;
                foreach (var hash in HashCode)
                {
                    if (ifVal)

                    {
                        if (sessions.Count <= sesssionId)
                        {
                            loopval = false;
                            ifVal = false;
                            break;
                        }
                        StatusReporter.Invoke("session", SessionName[sesssionId].Split(".txt")[0]);
                        var numJson = GetNumber(hash, sessions[sesssionId].token);

                        //"Request failed with status code TooManyRequests"
                        if (!string.IsNullOrWhiteSpace(GetNumberError))
                        {
                            StatusReporter.Invoke("log", GetNumberError); 
                            sesssionId++;
                            break;
                        }
                        else
                        {
                            var serialized = Serializer.Jsonserializer<Root>(numJson);

                            if (!string.IsNullOrWhiteSpace(serializerError))
                            {
                                StatusReporter.Invoke("log", serializerError);
                            }
                            else
                            {
                                if(serialized != null)
                                {
                                    if (!serialized.widgets.contact.phone.Contains("پنهان‌شده"))
                                    {
                                        collected++;
                                        StatusReporter.Invoke("collected", collected.ToString());
                                        FileWorker.WriteNumberToFile(FileName, serialized.widgets.contact.phone);
                                    }
                                }
                            }
                        }
                    }
                }
                pageDownNumber++;
            }

            StatusReporter.Invoke("caption", "Opertion End!");
        }

        #endregion

    }
}
