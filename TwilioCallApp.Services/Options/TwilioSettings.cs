namespace TwilioCallApp.Services.Options
{
    public static class TwilioSettings
    {
        /// <summary>
        /// The primary Twilio account SID, displayed prominently on your twilio.com/console dashboard.
        /// </summary>
        public static string AccountSid { get; set; }

        /// <summary>
        /// Signing Key SID, also known as the API SID or API Key.
        /// </summary>
        public static string ApiKey { get; set; }

        /// <summary>
        /// The API Secret that corresponds to the <see cref="ApiKey"/>.
        /// </summary>
        public static string ApiSecret { get; set; }
    }
}
