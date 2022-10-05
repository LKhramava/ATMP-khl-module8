using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.ObjectModel;
using System.Drawing;

namespace WebDriverNUnit.WebDriver
{
	internal class BaseElement : IWebElement
	{
		protected string Name;
		protected By Locator;
		protected IWebElement Element;

		public BaseElement(By locator, string name)
		{
			this.Locator = locator;
			this.Name = name == "" ? this.GetText() : name;
		}

		public BaseElement(By locator)
		{
			this.Locator = locator;
		}

		public string GetText()
		{
			this.WaitForIsVisible();
			return this.Element.Text;
		}

		public IWebElement GetElement()
		{
			try
			{
				this.Element = Browser.GetDriver().FindElement(this.Locator);
			}
			catch (Exception)
			{
				throw;
			}
			return this.Element;
		}

		public void WaitForIsVisible()
		{
			new WebDriverWait(Browser.GetDriver(), TimeSpan.FromSeconds(Browser.TimeoutForElement)).Until(ExpectedConditions.ElementIsVisible(this.Locator));
		}

		public void UrlContainsFraction(string fraction)
		{
			new WebDriverWait(Browser.GetDriver(), TimeSpan.FromSeconds(Browser.TimeoutForElement)).Until(ExpectedConditions.UrlContains(fraction));
		}

		public string TagName { get; }
		public string Text { get; }
		public bool Enabled { get; }
		public bool Selected { get; }
		public Point Location { get; }
		public Size Size { get; }
		public bool Displayed { get; }

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public void Click()
		{
			this.WaitForIsVisible();
			Browser.GetDriver().FindElement(this.Locator).Click();
		}

		public void ClickWithActions()
		{
			this.WaitForIsVisible();
			var locator = Browser.GetDriver().FindElement(this.Locator);
			new Actions(Browser.GetDriver()).Click(locator).Build().Perform();
		}

		public void PressEnter()
		{
			this.WaitForIsVisible();
			new Actions(Browser.GetDriver()).KeyDown(Keys.Enter).Build().Perform();
		}

		public void SendKeysWithActions(string keysToSend)
		{
			this.WaitForIsVisible();
			var locator = Browser.GetDriver().FindElement(this.Locator);
			new Actions(Browser.GetDriver()).SendKeys(locator, keysToSend).Build().Perform();
		}

		public void DragAndDropWithActions(IWebElement element)
		{
			this.WaitForIsVisible();
			var locator = Browser.GetDriver().FindElement(this.Locator);

			//doesn't work!!!
			//new Actions(Browser.GetDriver()).DragAndDrop(locator, element).Build().Perform();

			var a = locator;
			var b = element;

			int x = locator.Location.X;
			int y = locator.Location.Y;

			Actions actions = new Actions(Browser.GetDriver());
			actions.MoveToElement(a)
					.Pause(new TimeSpan(0, 0, 0, 1))
					.ClickAndHold(a)
					.Pause(new TimeSpan(0, 0, 0, 1))
					.MoveByOffset(x, y)
					.MoveToElement(b)
					.MoveByOffset(-10, -10)
					.Pause(new TimeSpan(0, 0, 0, 1))
					.Release().Build().Perform();
		}

		public void MoveToElement()
		{
			var locator = Browser.GetDriver().FindElement(this.Locator);
			new Actions(Browser.GetDriver()).MoveToElement(locator).Build().Perform();
		}

		public void JSClick()
		{
			this.WaitForIsVisible();
			IJavaScriptExecutor executor = (IJavaScriptExecutor)Browser.GetDriver();
			executor.ExecuteScript("arguments[0].click();", this.GetElement());

		}

		public void JSHighligh()
		{
			this.WaitForIsVisible();
			IJavaScriptExecutor executor = (IJavaScriptExecutor)Browser.GetDriver();
			executor.ExecuteScript("arguments[0].style.cssText += 'background-color:yellow !important;'", this.GetElement());
		}

		public By GetLocator()
		{
			return this.Locator;
		}

		public IWebElement FindElement(BaseElement baseElement)
		{
			try
			{
				return this.GetElement().FindElement(baseElement.GetLocator());
			}
			catch (Exception)
			{
				throw;
			}
		}

		public IWebElement FindElement(By by)
		{
			try
			{
				return this.GetElement().FindElement(by);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public ReadOnlyCollection<IWebElement> FindElements(By by)
		{
			try
			{
				return this.GetElement().FindElements(by);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public string GetAttribute(string attributeName)
		{
			throw new NotImplementedException();
		}

		public string GetCssValue(string propertyName)
		{
			throw new NotImplementedException();
		}

		public string GetProperty(string propertyName)
		{
			throw new NotImplementedException();
		}

		public void SendKeys(string text)
		{
			this.WaitForIsVisible();
			Browser.GetDriver().FindElement(this.Locator).SendKeys(text);
		}

		public void Submit()
		{
			throw new NotImplementedException();
		}

		public string GetDomAttribute(string attributeName)
		{
			throw new NotImplementedException();
		}

		public string GetDomProperty(string propertyName)
		{
			throw new NotImplementedException();
		}

		public ISearchContext GetShadowRoot()
		{
			throw new NotImplementedException();
		}

		public static ReadOnlyCollection<IWebElement> FindWebElements(By by)
		{
			try
			{
				return Browser.GetDriver().FindElements(by);
			}
			catch (Exception)
			{
				throw;
			}
		}

	}
}
