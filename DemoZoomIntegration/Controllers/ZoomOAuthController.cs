using DemoZoomIntegration.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Text;

namespace DemoZoomIntegration.Controllers
{
    public class ZoomOAuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ZoomOAuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            var clientId = "YOUR_CLIENT_ID";
            var returnUrl = "https://localhost:7288/ZoomOAuth/ReturnUrl";
            return Redirect($"https://zoom.us/oauth/authorize?response_type=code&client_id={clientId}&redirect_uri={Uri.EscapeDataString(returnUrl)}");
        }
        public async Task<IActionResult> ReturnUrl()
        {
            // Get the authorization code from the query string.
            string code = Request.Query["code"];
            // Exchange the authorization code for an access token.
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://zoom.us/oauth/token");
           
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("YOUR_CLIENT_ID:YOUR_CLIENT_SECRET")));
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", "https://localhost:7288/ZoomOAuth/ReturnUrl" },
            });
            var response = await client.SendAsync(request);
            var accessToken = response.Content.ReadAsStringAsync().Result;
            var token= JsonConvert.DeserializeObject<ZoomOAuthToken>(accessToken);

            // Save the access token in the user's session.
            //HttpContext.Session.SetString("AccessToken", accessToken);

            // Redirect the user to the home page.

            RestClient rclient = new RestClient("https://api.zoom.us/v2");
            rclient.AddDefaultHeader("Authorization", $"Bearer {token.access_token}");

            RestRequest rrequest = new RestRequest("/users/me/meetings", Method.Post);
            rrequest.AddJsonBody(new
            {
                topic = "My Meeting",
                type = 1,
                duration = 60
            });



            //{
            //    topic: "Beema Pradhikaran",
            //      type:2,
            //      start_time: "2023-07-17T15:25:10Z",
            //      duration:"3",
            //      settings:{
            //                host_video:true,
            //                participant_video:true,
            //                join_before_host:false,
            //                mute_upon_entry:"true",
            //                watermark: "true",
            //                audio: "voip",
            //                auto_recording: "cloud"
            //         }

            //}


            var rresponse = await rclient.ExecuteAsync(rrequest);

            if (rresponse.IsSuccessful)
            {
                // Meeting created successfully
                var result = rresponse.Content;
                var createdMeeting = JsonConvert.DeserializeObject<ZoomOAuthModel>(result);
                // Process the created meeting details as needed

                return View();
            }
            else
            {
                // Error occurred while creating the meeting
                return View("Error");
            }

        }
        
    }
}
