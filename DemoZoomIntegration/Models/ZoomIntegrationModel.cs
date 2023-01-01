namespace DemoZoomIntegration.Models
{
    public class ZoomIntegrationModel
    {
        public string Host { get; set; }
        public string Join { get; set; }
        public string Code { get; set; }
        public string MeetingId { get; set; }
        public string EncryptedPwd { get; set; }
    }
    public class ZoomResponse
    {
        public string start_url { get; set;}
        public string join_url { get; set;}
    }
    public class TokenResponse
    {

    }
}
