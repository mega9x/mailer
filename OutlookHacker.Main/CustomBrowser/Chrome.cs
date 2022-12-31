using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using SeleniumUndetectedChromeDriver;

namespace OutlookHacker.Crawler.CustomBrowser
{
    public class Chrome
    {
        public ChromeDriver Driver { get; private set; }
        public Chrome(string proxy)
        {
            var chromeOption = new ChromeOptions();
            chromeOption.AddArgument("--incognito");
            chromeOption.AddArgument("user-agent=Mozilla/6.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Safari/537.36");
            chromeOption.AddArgument("--private");
            chromeOption.AddArgument("--proxy-server=" + "http://" + proxy);
            Driver = UndetectedChromeDriver.Create(chromeOption);
            Driver.Manage().Cookies.DeleteAllCookies();
        }

        public Chrome()
        {
            var chromeOption = new ChromeOptions();
            chromeOption.AddArgument("--incognito");
            chromeOption.AddArgument("user-agent=Mozilla/6.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Safari/537.36");
            chromeOption.AddArgument("--private");
            Driver = UndetectedChromeDriver.Create(chromeOption);
            Driver.Manage().Cookies.DeleteAllCookies();
        }
    }
}