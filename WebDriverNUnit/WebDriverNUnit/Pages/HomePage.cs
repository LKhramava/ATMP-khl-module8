using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverNUnit.WebDriver;

namespace WebDriverNUnit.Pages
{
	public class HomePage : BasePage
	{
		private static readonly By HomeLbl = By.Id("grid");

		public HomePage() : base(HomeLbl, "Home Page")
		{

		}

		private readonly BaseElement loginBE = new BaseElement(By.ClassName("ph-login"));
		private readonly BaseElement loginFrameBE = new BaseElement(By.ClassName("ag-popup__frame__layout__iframe"));

		private readonly BaseElement userNameBE = new BaseElement(By.Name("username"));
		private readonly BaseElement inputLoginSubmitBE = new BaseElement(By.CssSelector(".login-row button[type='submit']"));
		private readonly BaseElement inputPasswordBE = new BaseElement(By.Name("password"));

		private readonly BaseElement dataClickCounterBE = new BaseElement(By.XPath("//a[@data-show-pixel]//div[@data-click-counter]"));

		public YourAccountPage GoToYourAccountPage(string login, string password)
		{
			return Login(login, password);
		}

		public void AssertIsVisible(By locator)
		{
			var label = new BaseElement(locator);
			label.WaitForIsVisible();
		}

		private YourAccountPage Login(string login, string password)
		{
			loginBE.Click();
			Browser.GetDriver().SwitchTo().Frame(this.loginFrameBE.GetElement());

			userNameBE.SendKeys(login);
			inputLoginSubmitBE.Click();

			inputPasswordBE.SendKeys(password);
			inputLoginSubmitBE.Click();

			dataClickCounterBE.Click();

			Browser.GetDriver().SwitchTo().DefaultContent();

			return new YourAccountPage();
		}
	}
}
