using Microsoft.Playwright;

using NUnit.Framework;

 
namespace APINegativeTests.Tests

{

    [TestFixture]

    public class APINegativeTest

    {

        private IPlaywright _playwright;

        private IAPIRequestContext _api;
 
        private const string BaseUrl = "https://dev.emeli.in.ua";

        // private const string Username = "admin";   

        // private const string Password = "Engineer_123"; 
 
        [OneTimeSetUp]

        public async Task Setup()

        {

            _playwright = await Playwright.CreateAsync();
 
            _api = await _playwright.APIRequest.NewContextAsync(new()

            {

                BaseURL = BaseUrl,

       

            });
        }

            [Test , Order(1)] //without auth

        public async Task Create_Post_Without_Auth_Should_401()

{

            var context = await _playwright.APIRequest.NewContextAsync();
    var response = await context.PostAsync(

        "https://dev.emeli.in.ua/wp-json/wp/v2/posts",

        new() { DataObject = new { title = "hack" } }

    );
 
    Assert.That(response.Status, Is.EqualTo(401));

}
[Test , Order(2)] //invalid endpoint

        public async Task Create_Post_Invalid_Endpoint_Should_404()
{

            var response = await _api.PostAsync( "https://dev.emeli.in.ua/posts",
        new() { DataObject = new { title = "hack" } }
    );
    Assert.That(response.Status, Is.EqualTo(404));
}
[Test, Order(3)] //get post without auth
        public async Task Get_Post_Without_Auth_Should()
{   
            var context = await _playwright.APIRequest.NewContextAsync();
    var response = await context.GetAsync(
        "https://dev.emeli.in.ua/wp-json/wp/v2/posts/22924"
    );
 
    Assert.That(response.Status, Is.EqualTo(200));  
}


  [OneTimeTearDown]

        public async Task TearDown()

        {

            await _api.DisposeAsync();

            _playwright.Dispose();

        }

        }
        
    }
