namespace MyScheduler.App.Tools.Email
{
    using System;
    using System.Configuration;
    using System.Net.Mail;
    using global::Tools;
    using Microsoft.Extensions.Options; 

    /// <summary>
    /// Defines the <see cref="Email" />
    /// </summary>
    public partial class Email : IEmail
    {
        private readonly IOptions<AppSettingsModel> settings;

        public Email(ISchedSmtpClient smtpClient, IOptions<AppSettingsModel> settings)
        {
            SmtpClient = smtpClient;
            this.settings = settings;
        }

        public ISchedSmtpClient SmtpClient { get; }

        public void SendEmail(string subject, string htmlBody)
        {
            try
            {
                using (MailMessage message = new MailMessage
                {
                    Subject = subject,
                    From = new MailAddress(this.settings.Value.EmailFrom),
                    Body = htmlBody,
                    IsBodyHtml = true
                })
                {
                    message.To.Add(this.settings.Value.EmailTo);
                    SmtpClient.SendCompleted += (s, e) =>
                    {
                        if (e.Cancelled == true)
                        {
                            Console.WriteLine("Email sending cancelled!");
                        }
                        else if (e.Error != null)
                        {
                            Console.WriteLine(e.Error.Message);
                        }
                        else
                        {
                            Console.WriteLine("Email sent sucessfully!");
                        }
                    };
                    SmtpClient.Send(message);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
