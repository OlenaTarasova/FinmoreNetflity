using Microsoft.Playwright;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using FinmoreNetflity.Pages;
 
namespace FinmoreNetflity.Tests
{
    [TestFixture]
    public class LogInTest
    {
        private IPlaywright? _playwright;
        private IBrowser? _browser;
        private IPage? _page;
        private BasePage? _basepage;
        private LogInPage? _loginPage;
 
        [SetUp]
        public async Task Setup()
        {
            _playwright = await Playwright.CreateAsync();
 
          
            bool isCI = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CI"));
 
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = isCI,
                // SlowMo тільки локально, у CI — максимально швидко
                SlowMo = isCI ? 0 : 200,
                Args = isCI
                    ? new[]
                    {
                        "--no-sandbox",
                        "--disable-setuid-sandbox",
                        "--disable-dev-shm-usage",
                        "--disable-web-security",
                        "--disable-features=IsolateOrigins,site-per-process"
                    }
                    : null
            });
 
            _page = await _browser.NewPageAsync();
 
            _basepage = new BasePage(_page);
            _loginPage = new LogInPage(_page);
 
            await _basepage.NavigateAsync();
        }
 
        [TearDown]
        public async Task Teardown()
        {
            if (_browser != null)
            {
                await _browser.CloseAsync();
            }
 
            _playwright?.Dispose();
        }
 
        [Test]
        [Category("UI")]
        [Description("Перевірка реєстрації нового користувача")]
        public async Task RegisterNewUserTest()
        {
            await _loginPage!.NavigateToRegisterPageAsync();
            await _loginPage.RegisterAsync(
                Constants.BasicName,
                Constants.BasicEmail,
                Constants.BasicPassword,
                Constants.BasicConfirmPassword);
 
        
 //           await _loginPage.LogInAsync(Constants.BasicEmail, Constants.BasicPassword);
        }
    }
}