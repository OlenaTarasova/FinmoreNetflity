
using Microsoft.Playwright;

using NUnit.Framework;

using System.Net;

using System.Text.Json;

using FinmoreNetflity.Utils;
 
namespace ApiPositiveTests.Tests

{

    [TestFixture]

    public class PostsCrudIDTests

    {

        private IPlaywright _playwright;

        private IAPIRequestContext _api;

        private int _postId;
 
        private const string BaseUrl = "https://dev.emeli.in.ua";

        private const string Username = "admin";   

        private const string Password = "Engineer_123"; 
 
        [OneTimeSetUp]

        public async Task Setup()

        {

            _playwright = await Playwright.CreateAsync();
 
            _api = await _playwright.APIRequest.NewContextAsync(new()

            {

                BaseURL = BaseUrl,

                ExtraHTTPHeaders = new Dictionary<string, string>

                {

                    {

                        "Authorization",

                        AuthHelper.GetBasicAuthHeader(Username, Password)

                    }

                }

            });

        }
        [Test, Order(1)] //Create ID post, id is required
        public async Task Create_Post_Should_Return_ID()

        {
            int id = 22924; 

            var response = await _api.PostAsync(

                "/wp-json/wp/v2/posts/{id}",

                new() { DataObject = new { title = "New Post for ID test"} }

            );

           Assert.That(response.Status, Is.EqualTo(200));

    var json = JsonDocument.Parse(await response.TextAsync());
    Assert.That(json.RootElement.GetProperty("id").GetInt32(), Is.EqualTo(id));

        }   
    

 [OneTimeTearDown]

        public async Task TearDown()

        {

            await _api.DisposeAsync();

            _playwright.Dispose();

        }
    }
}
