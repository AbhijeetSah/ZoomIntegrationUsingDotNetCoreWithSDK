using DemoZoomIntegration.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;

namespace DemoZoomIntegration.Controllers
{
    public class ZoomIntegrationController : Controller
    {
        

        public IActionResult Index()
        {
            ZoomIntegrationModel zoomIntegrationModel = new ZoomIntegrationModel();

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            var apiSecret = "fsbimmWxDLu3kX0SXeDm8ZD8fuVI6W99X2kC";
            byte[] symmetricKey = Encoding.ASCII.GetBytes(apiSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "JwajRFXySE2sK9J9SNmL5g",
                Expires = now.AddSeconds(300),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);


            var client = new RestClient("https://api.zoom.us/v2/users/wewelah824@cosaxu.com/meetings");
            var request = new RestRequest();

            //var client = new RestClient("https://api.zoom.us/v2/users/wewelah824@cosaxu.com/meetings");
            //var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            var time= DateTime.UtcNow.ToString();
            request.AddJsonBody(new { topic = "Meeting with Beema Pradhikaran", duration = "49", start_time = time, type = "2" }); //"2022-12-08T05:00:00"
            request.AddHeader("authorization", String.Format("Bearer {0}", tokenString));
            
            var response = client.Post(request);

            
            HttpStatusCode statusCode = response.StatusCode;
            int numericStatusCode = (int)statusCode;
            var jObject = JObject.Parse(response.Content);

            zoomIntegrationModel.Host = (string)jObject["start_url"];
            zoomIntegrationModel.Join = (string)jObject["join_url"];
            zoomIntegrationModel.Code = Convert.ToString(numericStatusCode);
            zoomIntegrationModel.MeetingId = (string)jObject["id"];
            zoomIntegrationModel.EncryptedPwd = (string)jObject["encrypted_password"];

            return View(zoomIntegrationModel);
        }

        [HttpGet]
        public IActionResult JoinMeeting(string MeetingId= null ,string EncryptedPwd = null)
        {
            //ViewBag.JoinUrl = url;
            //Console.WriteLine("Zoom copyright!");
            //Console.WriteLine("generate websdk token!");
            string apiKey = "MaDOA4PkoxzybFiwRzuoQtTIGIoLtrVymwR5"; //apikey
            string apiSecret = "ALU531tHKTxiQk4Ne6auXWip3vFXdg2wpO4p";  //apiSecret
            string meetingNumber = MeetingId;
            String ts = (ToTimestamp(DateTime.UtcNow.ToUniversalTime()) - 30000).ToString();
            string role = "0";
            //string token = GenerateToken(apiKey, apiSecret, meetingNumber, ts, role);
            //Console.WriteLine(token);
            var token = GenerateSignature(apiKey, apiSecret, meetingNumber, role).ToString();
            var model = new MeetingModel
            {
                Signature = token,
                SdkKey = "MaDOA4PkoxzybFiwRzuoQtTIGIoLtrVymwR5",
                MeetingNumber = meetingNumber,
                UserName = "SDK TESTER Client",
                PassWord = EncryptedPwd,
                RegistrantToken = "",
                UserEmail = ""
            };
            return View(model);
        }



        public object GenerateSignature(string apikey,string apisecret, string MeetingNumber,  string role)
        {
            //meeting initiation Time in millisecond 
            var iat = (ToTimestamp(DateTime.UtcNow) / 1000) - 30;
            //meeting expiry time
            var exp = iat + 60 * 60 * 2;

            //apisecret encryption with  SecurityAlgorithms.HmacSha256
            var Securitykey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(apisecret));
            var credential = new Microsoft.IdentityModel.Tokens.SigningCredentials(Securitykey, SecurityAlgorithms.HmacSha256);

            //adding header with  SecurityAlgorithms.HmacSha256 and jwttype
            var header = new JwtHeader(credential);

            //adding jwtpayload
            var payload = new JwtPayload {
                {"sdkKey",apikey },
                {"mn",MeetingNumber },
                {"role",role },
                {"iat",iat },
                {"exp",exp },
                {"appKey",apikey },
                {"tokenExp",iat+60*60*2 },

            };
            
            //generating jwt securitytoken
            var secToken = new JwtSecurityToken(header, payload);
            //initializing jwt security token handler
            var handler = new JwtSecurityTokenHandler();
            //generating token string
            var tokenString = handler.WriteToken(secToken);

            //showing token value and arguments
            var token = handler.ReadJwtToken(tokenString);
            var tokestring = token.Payload.First().Value;
            //returning token string
            return tokenString.TrimEnd('=');

        }

        public long ToTimestamp(DateTime value) {
            long epoch = (value.Ticks - 621355968000000000) / 10000;            
            return epoch;
        }

        //public string GenerateToken(string apiKey, string apiSecret, string meetingNumber, string ts, string role)
        //{
        //    string message = String.Format("{0}{1}{2}{3}", apiKey, meetingNumber, ts, role);
        //    apiSecret = apiSecret ?? "";
        //    var encoding = new System.Text.ASCIIEncoding();
        //    byte[] keyByte = encoding.GetBytes(apiSecret);
        //    byte[] messageBytesTest = encoding.GetBytes(message);
        //    string msgHashPreHmac = System.Convert.ToBase64String(messageBytesTest);
        //    byte[] messageBytes = encoding.GetBytes(msgHashPreHmac);
        //    using (var hmacsha256 = new HMACSHA256(keyByte))
        //    {
        //        byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
        //        string msgHash = System.Convert.ToBase64String(hashmessage);
        //        string token = String.Format("{0}.{1}.{2}.{3}.{4}", apiKey, meetingNumber, ts, role, msgHash);
        //        var tokenBytes = System.Text.Encoding.UTF8.GetBytes(token);
        //        return System.Convert.ToBase64String(tokenBytes).TrimEnd('=');
        //    }
        //}

        [HttpGet]
        public IActionResult HostMeeting(string MeetingId = null, string EncryptedPwd = null)
        {
            string apiKey = "MaDOA4PkoxzybFiwRzuoQtTIGIoLtrVymwR5"; //apikey //ZOOM_SDK_KEY
            string apiSecret = "ALU531tHKTxiQk4Ne6auXWip3vFXdg2wpO4p";  //apiSecret //ZOOM_SDK_SECRET


            //generate meeting time //remains unused
            String ts = (ToTimestamp(DateTime.UtcNow) - 30000).ToString();


            string meetingNumber = MeetingId;
            string role = "1";



            //string token = GenerateToken(apiKey, apiSecret, meetingNumber, ts, role);
            //Console.WriteLine(token);
            


            var token = GenerateSignature(apiKey, apiSecret, meetingNumber, role).ToString();

            var model = new MeetingModel
            {
                Signature = token,
                SdkKey = "MaDOA4PkoxzybFiwRzuoQtTIGIoLtrVymwR5",
                MeetingNumber = meetingNumber,
                UserName = "SDK TESTER HOST",
                PassWord = EncryptedPwd,
                RegistrantToken = "",
                UserEmail = ""
            };
            return View(model);
            
        }

        //[HttpGet]
        //public IActionResult HostMeeting(string url)
        //{
        //    ViewBag.HostUrl = url;
        //    return View();
        //}
    }
}
