using Microsoft.Playwright;
using System.Threading.Tasks;
using NUnit.Framework;

namespace FinmoreNetflity.Pages
{
    public class HomePage : BasePage
    {
        public HomePage(IPage page) : base(page) { }


        public async Task OpenAsync() => await NavigateAsync();

       private const string ExpectedTitle = "Повнофункціональний фінансовий менеджер";
        private const string ExpectedUrl = "https://finmore.netlify.app/";
       
          public async Task VerifyHomePageAsync()
        {
            // Перевірка URL
            var currentUrl = Page.Url;
            Assert.That(currentUrl, Is.EqualTo(ExpectedUrl), $"Incorrect URL. Expected: {ExpectedUrl}, Actual: {currentUrl}");

            // Перевірка Title
            var pageTitle = await Page.TitleAsync();
            Assert.That(pageTitle, Is.EqualTo(ExpectedTitle), $"Incorrect page title. Expected: {ExpectedTitle}, Actual: {pageTitle}");
        }

       
    }
}