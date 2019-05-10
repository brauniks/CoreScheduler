namespace MyScheduler.App.Tools.Email
{
    public interface IEmail
    {
        ISchedSmtpClient SmtpClient { get; }

        void SendEmail(string subject, string htmlBody);
    }
}