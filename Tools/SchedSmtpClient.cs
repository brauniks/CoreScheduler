namespace MyScheduler.App.Tools.Email
{
    using global::Tools;
    using Microsoft.Extensions.Options; 
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Net;
    using System.Net.Mail;

    public partial class Email
    {
        public class SchedSmtpClient : ISchedSmtpClient, IDisposable
        {
            private SmtpClient smtp;

            public string Host { get; }
             
            private readonly IOptions<AppSettingsModel> settings;

            public int Port { get; private set; }
            public bool EnableSsl { get; private set; }
            public ICredentialsByHost Credentials { get; private set; }
            public event SendCompletedEventHandler SendCompleted;
            public SchedSmtpClient(IOptions<AppSettingsModel> settings)
            {
                this.settings = settings;
                Host = this.settings.Value.SmtpHost; 
                Port = this.settings.Value.Port;
                EnableSsl = true;
                Credentials = new NetworkCredential(this.settings.Value.EmailFrom, this.settings.Value.Password);

                smtp = new SmtpClient(Host)
                {
                    Credentials = Credentials,
                    Port = Port,
                    EnableSsl = EnableSsl,

                };

                smtp.SendCompleted += this.SendCompleted;
            }


            public void Send(MailMessage mailMessage)
            {
                smtp.Send(mailMessage); 
            }

            private bool disposedValue = false; // To detect redundant calls

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        smtp = null;
                    }
                    if (smtp != null)
                    {
                        smtp = null;
                    }

                    disposedValue = true;
                }
            }

            // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
            // ~SchedSmtpClient() {
            //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            //   Dispose(false);
            // }

            // This code added to correctly implement the disposable pattern.
            public void Dispose()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(true);
                // TODO: uncomment the following line if the finalizer is overridden above.
                // GC.SuppressFinalize(this);
            }

        }



    }
}
