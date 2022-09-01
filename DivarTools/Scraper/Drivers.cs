using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace DivarTools.Scraper
{
    internal static class CustomDrivers
    {
        public static FirefoxDriver FireFoxDriver()
        {
            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();

            service.HideCommandPromptWindow = true;

            var Options = new FirefoxOptions();
            Options.AddArgument("--window-position=-32000,-32000");
            Options.AddArgument("--headless");

            return new FirefoxDriver(service, Options);
        }

        public static ChromeDriver ChromeDriver()
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();

            service.HideCommandPromptWindow = true;

            var Options = new ChromeOptions();
            Options.AddArgument("--window-position=-32000,-32000");
            Options.AddArgument("--headless");

            return new ChromeDriver(service, Options);
        }

    }
}
