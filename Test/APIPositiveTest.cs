using Microsoft.Playwright;

using NUnit.Framework;

using System.Net;

using System.Text.Json;

using FinmoreNetflity.Utils;
 
namespace ApiPositiveTests.Tests

{

    [TestFixture]

    public class PostsCrudTests

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
 
        // ================== CREATE ==================

        [Test, Order(1)]// має запускатись першим

        public async Task Create_Post()

        {

            var response = await _api.PostAsync("/wp-json/wp/v2/posts", new()

            {

                DataObject = new //наше Body

                {

                    title = "Playwright C# API Post",

                    content = "Created via Playwright API test",

                    status = "publish"

                }

            });
 
            Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.Created));
 
            var json = JsonDocument.Parse(await response.TextAsync());

            _postId = json.RootElement.GetProperty("id").GetInt32(); // сохраняємо ID створеного поста для подальших тестів
 
            Assert.That(_postId, Is.GreaterThan(0));

        }
 
        // ================== READ ==================

        [Test, Order(2)]

        public async Task Get_Post()

        {

            var response = await _api.GetAsync($"/wp-json/wp/v2/posts/{_postId}");
 
            Assert.That(response.Status, Is.EqualTo(200));
 
            var body = await response.TextAsync();

            Assert.That(body, Does.Contain("Playwright C# API Post"));

        }
 
        // ================== UPDATE ==================

        [Test, Order(3)]

        public async Task Update_Post()

        {

            var response = await _api.PostAsync($"/wp-json/wp/v2/posts/{_postId}", new()

            {

                DataObject = new

                {

                    title = "UPDATED Playwright Post"

                }

            });
 
            Assert.That(response.Status, Is.EqualTo(200));
 
            var json = JsonDocument.Parse(await response.TextAsync());

            var title = json.RootElement

                .GetProperty("title")

                .GetProperty("rendered")

                .GetString();
 
            Assert.That(title, Is.EqualTo("UPDATED Playwright Post"));

        }
 
        // ================== DELETE ==================

        [Test, Order(4)]

        public async Task Delete_Post()

        {

            var response = await _api.DeleteAsync($"/wp-json/wp/v2/posts/{_postId}?force=true");
 
            Assert.That(response.Status, Is.EqualTo(200));

        }
   [Test, Order(5)]
           public async Task Get_Post_retest()

        {

            var response = await _api.GetAsync($"/wp-json/wp/v2/posts/{_postId}");
 
            Assert.That(response.Status, Is.EqualTo(404));
 
           
        }

        [Test, Order(6)]
           public async Task Create_Scheduled_Post()

        {
            var response = await _api.PostAsync("/wp-json/wp/v2/posts", new()

            {

                DataObject = new //наше Body

                {

                    title = "Scheduled Playwright C# API Post",

                    content = "Created via Playwright API test",

                    status = "future",

                    date = DateTime.UtcNow.AddHours(1).ToString("yyyy-MM-ddTHH:mm:ss")

                }

            });
 
            Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.Created));
 
            var json = JsonDocument.Parse(await response.TextAsync());

            _postId = json.RootElement.GetProperty("id").GetInt32(); // сохраняємо ID створеного поста для подальших тестів
 
            Assert.That(_postId, Is.GreaterThan(0));
        }

        [Test, Order(7)]
 public async Task Get_Scheduled_Post()

        {

            var response = await _api.GetAsync($"/wp-json/wp/v2/posts/{_postId}");
 
           Assert.That(response.Status, Is.EqualTo(200));
 
            var body = await response.TextAsync();

            Assert.That(body, Does.Contain("Scheduled Playwright C# API Post"));

        }

        [Test, Order(8)]
        
           public async Task Delete_Scheduled_Post()

        {

            var response = await _api.DeleteAsync($"/wp-json/wp/v2/posts/{_postId}?force=true");
 
            Assert.That(response.Status, Is.EqualTo(200));

        }
            [Test, Order(9)]
              public async Task Get_Scheduled_Post_retest()
        {
            var response = await _api.GetAsync($"/wp-json/wp/v2/posts/{_postId}");
 
            Assert.That(response.Status, Is.EqualTo(404));
        }

        [Test, Order(10)] // Missing required field generates validation error
        public async Task Create_Post_Without_Title_Should_Fail()
        {
            var response = await _api.PostAsync("/wp-json/wp/v2/posts", new()
            {
                DataObject = new
                {
                    content = "This post has no title"
                    
                }
            });

            Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.Created)); // WordPress allows posts without titles, so it returns 201 Created

             var json = JsonDocument.Parse(await response.TextAsync());

            _postId = json.RootElement.GetProperty("id").GetInt32(); // сохраняємо ID створеного поста для подальших тестів
 
            Assert.That(_postId, Is.GreaterThan(0));
        }

        [Test, Order(11)] // Get the post created without title ( test 6)
        public async Task Get_Post_Without_Title()
        {
            var response = await _api.GetAsync($"/wp-json/wp/v2/posts/{_postId}");
 
            Assert.That(response.Status, Is.EqualTo(200));
 
            var body = await response.TextAsync();

            Assert.That(body, Does.Contain("This post has no title"));
        }

        [Test, Order(12)] // Clean up by deleting the post created without title
        public async Task Delete_Post_Without_Title()
        {
            var response = await _api.DeleteAsync($"/wp-json/wp/v2/posts/{_postId}?force=true");
 
            Assert.That(response.Status, Is.EqualTo(200));
        }

        [Test, Order(13)] // Test that the post is deleted
        public async Task Get_Deleted_Post()
        {
            var response = await _api.GetAsync($"/wp-json/wp/v2/posts/{_postId}");
 
            Assert.That(response.Status, Is.EqualTo(404));
        }

        [Test, Order(14)] // Using a non-existent post status returns a validation error.( test )
        public async Task Create_Post_With_Invalid_Status_Should_Fail()
        {
            var response = await _api.PostAsync("/wp-json/wp/v2/posts", new()
            {
                DataObject = new
                {
                    title = "Invalid Status Post",
                    content = "This post has an invalid status",
                    status = "invalid_status"
                }
            });

            Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.BadRequest)); // Expecting 400 Bad Request for invalid status
        }
        [Test, Order(15)] // Create Post With Valid Fields but Invalid Author ID
        public async Task Create_Post_With_Invalid_Author_Should_Fail()
        {
            var response = await _api.PostAsync("/wp-json/wp/v2/posts", new()
            {
                DataObject = new
                {
                    title = "Invalid Author Post",
                    content = "This post has an invalid author ID",
                    author = 99999 // Assuming this author ID does not exist
                }
            });

            Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.BadRequest)); // Expecting 400 Bad Request for invalid author ID
        }
        [OneTimeTearDown]

        public async Task TearDown()

        {

            await _api.DisposeAsync();

            _playwright.Dispose();

        }

    
//          негативні тести
//   [Test , Order(5)]
        
           
// public async Task Create_Post_Without_Auth_Should_401()
// {
//     var context = await _playwright.APIRequest.NewContextAsync();
 
//     var response = await context.PostAsync(
//         "https://dev.emeli.in.ua/wp-json/wp/v2/posts",
//         new() { DataObject = new { title = "hack" } }
//     );
 
//     Assert.That(response.Status, Is.EqualTo(401));
// }
    }

}

 