using OpenQA.Selenium;
using System;
using System.Threading;
using WebDriverNUnit.WebDriver;

namespace WebDriverNUnit.Pages
{
	public class YourAccountPage : BasePage
	{
		private static readonly By AccountLbl = By.Id("app-canvas");

		public YourAccountPage() : base(AccountLbl, "Account Page")
		{
		}

		private readonly BaseElement sideBarContentBE = new BaseElement(By.Id("sideBarContent"));

		private readonly BaseElement newEmailBE = new BaseElement(By.XPath("//a[contains(@href,'compose')]"));
		private readonly BaseElement newEmailWindowBE = new BaseElement(By.CssSelector(".compose-app__compose"));
		private readonly BaseElement newEmailWindowCloseBE = new BaseElement(By.XPath("//button[@data-promo-id='extend']/following-sibling::button"));

		private readonly BaseElement toBE = new BaseElement(By.XPath("//div[contains(@data-type,'to')]//input"));
		private readonly BaseElement subjectBE = new BaseElement(By.XPath("//input[contains(@name,'Subject')]"));
		private readonly BaseElement bodyBE = new BaseElement(By.XPath("(//div[@contenteditable='true']//div)[1]"));

		private readonly BaseElement saveDraftBE = new BaseElement(By.XPath("//button[@data-test-id='save']"));

		private readonly BaseElement mainContainerBE = new BaseElement(AccountLbl);

		private By lettersBy = By.XPath("//div[contains(@class, 'letter-list')]//a[contains(@class, 'js-letter-list-item')]");
		private By letterCorrespondentBy = By.XPath("//span[contains(@class, 'll-crpt')]");
		private By letterSubjectBy = By.XPath("//span[contains(@class, 'llc__subject')]//span");
		private By letterSnippetBy = By.XPath("//span[contains(@class, 'llc__snippet')]//span");

		private readonly BaseElement draftBE = new BaseElement(By.XPath("//div[@id='sideBarContent']//a[contains(@href,'drafts')]"));

		private readonly BaseElement sentBE = new BaseElement(By.XPath("//div[@id='sideBarContent']//a[contains(@href,'sent')]"));

		private readonly BaseElement composeAppPopupBE = new BaseElement(By.XPath("//div[contains(@class, 'compose-app_popup')]"));
		private readonly BaseElement composeAppPopupSendBE = new BaseElement(By.XPath("//div[contains(@class, 'compose-app_popup')]//button[@data-test-id='send']"));
		private readonly BaseElement sentMessageCloseBE = new BaseElement(By.CssSelector(".layer__controls"));

		private readonly BaseElement accoutContainerBE = new BaseElement(By.XPath("//div[contains(@class, 'ph-project__account')]"));
		private readonly BaseElement exitBE = new BaseElement(By.XPath("//div[contains(@class, 'ph-accounts')]//div[contains(@class, 'ph-item')]//div[contains(@class, 'ph-icon')]"));

		public bool SaveDraftEmail(string letterEmail, string letterSubject, string letterBody)
		{
			SaveDraftEmailInternal(letterEmail, letterSubject, letterBody);

			//	Verify, that the mail presents in ‘Drafts’ folder
			//	Verify the draft content (addressee, subject and body – should be the same as in 3).
			sideBarContentBE.Click();
			
			//var draftSubjectByStr = "//span[contains(@class, 'll-sj__normal') and contains(text(), '" + letterSubject + "')]";
			//var draftSubjectBy = By.XPath(draftSubjectByStr);
			//IsElementVisible(draftSubjectBy);
			Thread.Sleep(1000);

			var letterInDraft = FindLetterInList(lettersBy, letterEmail, letterSubject, letterBody);
			return letterInDraft != null;
		}

		public bool SendDraftEmail(string letterEmail, string letterSubject, string letterBody)
		{
			return SendDraftEmailInternal(letterEmail, letterSubject, letterBody);
		}
		public void Logout()
		{
			accoutContainerBE.Click();
			exitBE.Click();
		}

		private void SaveDraftEmailInternal(string letterEmail, string letterSubject, string letterBody)
		{
			newEmailBE.Click();
			newEmailWindowBE.WaitForIsVisible();

			toBE.SendKeys(letterEmail);
			subjectBE.SendKeys(letterSubject);
			bodyBE.SendKeys(letterBody);

			//	Save the mail as a draft.
			saveDraftBE.Click();

			var newEmailWindowClose = newEmailWindowBE.FindElement(newEmailWindowCloseBE);
			newEmailWindowClose.Click();
		}

		private bool SendDraftEmailInternal(string letterEmail, string letterSubject, string letterBody)
		{
			//	Verify, that the mail presents in ‘Drafts’ folder
			//	Verify the draft content (addressee, subject and body – should be the same as in 3).
			draftBE.Click();
			Thread.Sleep(1000);

			var letterInDraft = FindLetterInList(lettersBy, letterEmail, letterSubject, letterBody);
			if (letterInDraft != null)
			{
				letterInDraft.Click();
				composeAppPopupBE.WaitForIsVisible();

				//	Send the mail
				composeAppPopupSendBE.Click();
				sentMessageCloseBE.Click();

				var checkLetterInDraftResult = CheckLetterInDraft(letterEmail, letterSubject, letterBody);
				var checkLetterInSentResult = CheckLetterInSent(letterEmail, letterSubject, letterBody);
				return checkLetterInDraftResult == null && checkLetterInSentResult != null;
			}
			return false;
		}

		private IWebElement? CheckLetterInDraft(string letterEmail, string letterSubject, string letterBody)
		{
			//	Verify, that the mail disappeared from ‘Drafts’ folder
			sideBarContentBE.Click();
			draftBE.Click();

			Thread.Sleep(1000);
			var notExistLetterInDraft = FindLetterInList(lettersBy, letterEmail, letterSubject, letterBody);
			return notExistLetterInDraft;
		}

		private IWebElement? CheckLetterInSent(string letterEmail, string letterSubject, string letterBody)
		{
			//	Verify, that the mail is in ‘Sent’ folder.
			sideBarContentBE.Click();
			sentBE.Click();

			Thread.Sleep(100);
			var letterInSent = FindLetterInList(lettersBy, letterEmail, letterSubject, letterBody);
			return letterInSent;
		}

		private IWebElement FindLetterInList(By letters, string letterEmail, string letterSubject, string letterBody)
		{
			var lettersList = Browser.GetDriver().FindElements(letters);

			foreach (var letter in lettersList)
			{
				var email = letter.FindElement(letterCorrespondentBy).GetAttribute("title");
				var subject = letter.FindElement(letterSubjectBy).Text;
				var data = letter.FindElement(letterSnippetBy).Text;

				if (string.Equals(email, letterEmail, StringComparison.OrdinalIgnoreCase) &&
					subject.Contains(letterSubject, StringComparison.OrdinalIgnoreCase) &&
					data.Contains(letterBody, StringComparison.OrdinalIgnoreCase))
				{
					return letter;
				}
			}
			return null;
		}
	}
}
