using Microsoft.Playwright;
using System.Threading.Tasks;
using NUnit.Framework;
using FinmoreNetflity.Pages;


namespace FinmoreNetflity.Tests

{
   
    [TestFixture]
    public class LogInTest
    {
        protected IPlaywright? _playwright;
        protected IBrowser? _browser;
        protected IPage? _page;
        private BasePage? _basepage;
        private LogInPage? _loginPage;
        [SetUp]

        public async Task Setup()
        {

            _playwright = await Playwright.CreateAsync();

            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions

            {

                Headless = false,

                SlowMo = 200

            });
            _page = await _browser.NewPageAsync();

           _basepage = new BasePage(_page);

            _loginPage = new LogInPage(_page);
            await _basepage.NavigateAsync();

           

        }

        [TearDown]

        public async Task Teardown()

        {

            await _browser!.CloseAsync();

            _playwright!.Dispose();

        }

        [Test]

        [Category("UI")]

        [Description("Перевірка реєстрації нового користувача")]
        public async Task RegisterNewUserTest()
        {
            await _loginPage!.NavigateToRegisterPageAsync();
            await _loginPage.RegisterAsync(Constants.BasicName, Constants.BasicEmail, Constants.BasicPassword, Constants.BasicConfirmPassword);
        // await _loginPage.LogInAsync(Constants.BasicEmail, Constants.BasicPassword);
             }
}
    }
