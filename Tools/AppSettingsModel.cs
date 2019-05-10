namespace Tools
{
    public class AppSettingsModel
    {
        public string SmtpHost { get; set; }
        public int Port { get; set; }
        public string Login { get; set; }
        public string Password { get; set; } 
        public string EmailTo { get; set; }
        public string EmailFrom { get; set; }
        public decimal Price { get; set; }
    }
}