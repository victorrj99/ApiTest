namespace ApiOwn;

public static class Configuration
{
    // JASON WEB TOKEN
    public static string JwtKey = "WTbguX86z1WIimC2CMy1a2edywG9izrAY5wFRTXTMgs";
    public static string ApiKeyName = "api_key";
    public static string ApiKey = "b2c7b4b0-0b7b-4b3b-8b7b-0b7b4b0b7b4b";
    public static SmtpConfiguration Smtp = new ();
    
    
    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; } = 25;
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    
}