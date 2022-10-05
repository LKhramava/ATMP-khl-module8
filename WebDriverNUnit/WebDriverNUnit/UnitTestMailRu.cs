using NUnit.Framework;
using System;
using WebDriverNUnit.Pages;
using WebDriverNUnit.WebDriver;

namespace WebDriverNUnit
{
	public class TestsForMailRu : BaseTest
	{
		[Test]
		[TestCase("lizakhramova", "070461040485")]
		public void TestLogin(string login, string password)
		{
			var homePage = new HomePage();
			homePage.GoToYourAccountPage(login, password);
		}

		[Test]
		[TestCase("lizakhramova", "070461040485", "lizakhramova@mail.ru", "TestAT", "Hello! My name is Liza! How are you? See you later. Bye.")]
		public void SaveDraftEmail(string login, string password, string letterEmail, string letterSubject, string letterBody)
		{
			var homePage = new HomePage();
			var accountPage = homePage.GoToYourAccountPage(login, password);

			letterSubject = letterSubject + DateTime.Now.TimeOfDay.Ticks.ToString();

			var letterInDraft = accountPage.SaveDraftEmail(letterEmail, letterSubject, letterBody);
			Assert.NotNull(letterInDraft);
		}

		[Test]
		[TestCase("lizakhramova", "070461040485", "lizakhramova@mail.ru", "TestAT", "Hello! My name is Liza! How are you? See you later. Bye.")]
		public void SendDraftEmail(string login, string password, string letterEmail, string letterSubject, string letterBody)
		{
			var homePage = new HomePage();
			var accountPage = homePage.GoToYourAccountPage(login, password);

			letterSubject = letterSubject + DateTime.Now.TimeOfDay.Ticks.ToString();

			accountPage.SaveDraftEmail(letterEmail, letterSubject, letterBody);
			var sendDraftEmailResult = accountPage.SendDraftEmail(letterEmail, letterSubject, letterBody);

			accountPage.Logout();

			Assert.IsTrue(sendDraftEmailResult);
		}
	}
}