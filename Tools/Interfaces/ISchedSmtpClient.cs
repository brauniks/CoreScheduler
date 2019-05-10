using System.ComponentModel;
using System.Net.Mail;

namespace MyScheduler.App.Tools.Email
{
    public interface ISchedSmtpClient
    {
          event SendCompletedEventHandler SendCompleted;
        void  Send (MailMessage mailMessage); 
    }
}