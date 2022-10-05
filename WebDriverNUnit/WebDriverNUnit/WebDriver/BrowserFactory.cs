using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;

namespace WebDriverNUnit.WebDriver
{
	public class BrowserFactory
	{
		public enum BrowserType
		{
			Chrome,
			Firefox
		}

		public static IWebDriver GetDriver(BrowserType type, int timeOutSec)
		{
			IWebDriver driver = null;
			switch (type)
			{
				case BrowserType.Chrome:
					{
						var service = ChromeDriverService.CreateDefaultService();
						ChromeOptions options = new ChromeOptions();
						options.AddArgument("disable-infobars");
						driver = new ChromeDriver(service, options, TimeSpan.FromSeconds(timeOutSec));
						break;
					}
				case BrowserType.Firefox:
					{
						var service = FirefoxDriverService.CreateDefaultService();
						FirefoxOptions options = new FirefoxOptions();
						driver = new FirefoxDriver(service, options, TimeSpan.FromSeconds(timeOutSec));
						break;
					}
			}
			return driver;
		}
	}
}
