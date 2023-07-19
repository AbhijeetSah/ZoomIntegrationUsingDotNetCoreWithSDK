namespace DemoZoomIntegration.Models
{
    public class ZoomOAuthModel
    {
    }

    public class ZoomUserData
    {
        public string? uuid { get; set; }
        public string? id { get; set; }
        public string? host_id { get; set; }
        public string? host_email { get; set; }
        public string? topic { get; set; }
        public string? type { get; set; }
        public string? status { get; set; }
        public string? start_time { get; set; }
        public int? duration { get; set; }
        public string? timezone { get; set; }
        public string? created_at { get; set; }
        public string? start_url { get; set; }
        public string? join_url { get; set; }
        public string? password { get; set; }
        public string? h323_password { get; set; }
        public string? pstn_password { get; set; }
        public string? encrypted_password { get; set; }
        public SettingsModel? settings { get; set; }
        public bool? pre_schedule { get; set; }

    }

    public class SettingsModel
    {
        public bool? host_video { get; set; }
        public bool? participant_video { get; set; }
        public bool? cn_meeting { get; set; }
        public bool? in_meeting { get; set; }
        public bool? join_before_host { get; set; }
        public int? jbh_time { get; set; }
        public bool? mute_upon_entry { get; set; }
        public bool? watermark { get; set; }
        public bool? use_pmi { get; set; }
        public int? approval_type { get; set; }
        public bool? audio { get; set; }
        public bool? auto_recording { get; set; }
        public bool? enforce_login { get; set; }
        public string? enforce_login_domains { get; set; }
        public string? alternative_hosts { get; set; }
        public bool? alternative_host_update_polls { get; set; }
        public bool? close_registration { get; set; }
        public bool? show_share_button { get; set; }
        public bool? allow_multiple_devices { get; set; }
        public bool? registrants_confirmation_email { get; set; }
        public bool? waiting_room { get; set; }
        public bool? request_permission_to_unmute_participants { get; set; }
        public bool? meeting_authentication { get; set; }
        public string? encryption_type { get; set; }
        public bool? alternative_hosts_email_notification { get; set; }
        public bool? show_join_info { get; set; }
        public bool? device_testing { get; set; }
        public bool? focus_mode { get; set; }
        public bool? enable_dedicated_group_chat { get; set; }
        public bool? private_meeting { get; set; }
        public bool? email_notification { get; set; }
        public bool? host_save_video_order { get; set; }
        public bool? email_in_attendee_report { get; set; }
        //"approved_or_denied_countries_or_regions": {
        //    "enable": false
        //},
        //"question_and_answer": {
        //    "enable": false
        //},
        //"breakout_room": {
        //    "enable": false
        //},
        //    "sign_language_interpretation": {
        //    "enable": false
        //},
    }

    public class ZoomOAuthToken
    {
        public string? access_token { get; set; }
        public string? token_type { get; set; }
        public string? refresh_token { get; set; }
        public int? expires_in { get; set; }

    }

}
