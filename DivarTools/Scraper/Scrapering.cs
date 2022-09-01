using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivarTools.Scraper
{
    public class Scrapering
    {
        private IWebDriver WebDriver { get; set; }

        private IWebDriver CreateWebDriver(Enums.Drivers driver)
        {
            return driver switch
            {
                Enums.Drivers.Chrome => CustomDrivers.ChromeDriver(),
                Enums.Drivers.Firefox => CustomDrivers.FireFoxDriver(),
                _ => CustomDrivers.FireFoxDriver(),
            };

        }

        public Scrapering(Enums.Drivers driver)
        {
            WebDriver = CreateWebDriver(driver);
        }

        public string WebScraper(string url, int PageDownCount)
        {
            WebDriver.Navigate().GoToUrl(url);
            Thread.Sleep(2000);
            var elementHtml = WebDriver.FindElement(By.TagName("body"));

            for (int i = 0; i < PageDownCount; i++)
            {
                elementHtml.SendKeys(Keys.PageDown);
                Thread.Sleep(500);
            }
            Thread.Sleep(1000);
            var src = WebDriver.PageSource;

            WebDriver.Quit();

            return src;
        }

    }
}
