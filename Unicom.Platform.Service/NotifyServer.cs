using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Unicom.Platform.Service
{
    internal class NotifyServer
    {
        private static readonly Dictionary<string, NotifyCountArgs> DevNotifyUnit = new Dictionary<string, NotifyCountArgs>();

        private static readonly SmtpClient Client;

        private static readonly MailAddress ServerMail;

        static NotifyServer()
        {
            Client = new SmtpClient("smtp.exmail.qq.com", 25)
            {
                Credentials = new NetworkCredential("devMailServer@shweidong.com", "SHWDDevServer2017"),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            Client.SendCompleted += OnMailSendCompleted;

            ServerMail = new MailAddress("devMailServer@shweidong.com", "SHWD_Dev_Server", Encoding.UTF8);
        }

        public static void Notify(string dev, string message)
        {
            if (!DevNotifyUnit.ContainsKey(dev))
            {
                DevNotifyUnit.Add(dev, new NotifyCountArgs());
            }

            var unit = DevNotifyUnit[dev];
            if (!unit.EvalSendContidion()) return;
            ExecuteNotify(dev, message);
        }

        public static void ExceedNotify(string dev, string message) => ExecuteNotify(dev, message);

        private static void ExecuteNotify(string dev, string message)
        {
            var mailMsg = new MailMessage { From = ServerMail };
            mailMsg.To.Add("autyan@shweidong.com");
            mailMsg.Subject = "Unicom Data Upgrade Failed.";
            mailMsg.SubjectEncoding = Encoding.UTF8;
            mailMsg.Body = message;
            mailMsg.BodyEncoding = Encoding.UTF8;
            mailMsg.Priority = MailPriority.High;
            try
            {
                Client.SendAsync(mailMsg, dev);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void OnMailSendCompleted(object sender, AsyncCompletedEventArgs asyncCompletedEventArgs)
        {
            if (asyncCompletedEventArgs.Error != null)
            {
                Console.WriteLine(asyncCompletedEventArgs.Error.Message);
            }
            DevNotifyUnit[asyncCompletedEventArgs.UserState.ToString()].LastDateTime = DateTime.Now;
        }
    }

    internal class NotifyCountArgs
    {
        public DateTime LastDateTime { get; set; }

        public DateTime StartDateTime { get; set; }

        public bool EvalSendContidion()
        {
            var now = DateTime.Now;
            StartDateTime = (now - LastDateTime).TotalSeconds > 120 ? now : StartDateTime;

            return (now - StartDateTime).TotalSeconds > 900;
        }
    }
}
