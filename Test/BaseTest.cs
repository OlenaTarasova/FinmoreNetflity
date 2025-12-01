using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;
using FinmoreNetflity.Pages;


namespace FinmoreNetflity.Tests
{
    public class BaseTest
    {

        protected IPlaywright? _playwright;
        protected IBrowser? _browser;
        protected IPage? _page;
        protected const string BaseUrl = "https://finmore.netlify.app/";

        // public BaseTest(IPage page) : base(page)
       

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

            await _page.GotoAsync(BaseUrl, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });
        }

 [TearDown]

        public async Task Teardown()

        {

            await _browser!.CloseAsync();

            _playwright!.Dispose();

        }
 
    }
}