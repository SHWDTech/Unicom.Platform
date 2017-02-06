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
        private static readonly Dictionary<string, NotifyCountArgs> DevNotifyCount = new Dictionary<string, NotifyCountArgs>();

        public static void Notify(string dev, string message)
        {
            if (!DevNotifyCount.ContainsKey(dev))
            {
                DevNotifyCount.Add(dev, new NotifyCountArgs());
            }
            if (DevNotifyCount[dev].LastDateTime < DateTime.Today)
            {
                DevNotifyCount[dev].Count = 0;
            }
            if (DevNotifyCount[dev].Count > 3) return;

            ExecuteNotify(dev, message);
        }

        private static void ExecuteNotify(string dev, string message)
        {
            var client = new SmtpClient("smtp.exmail.qq.com", 25)
            {
                Credentials = new NetworkCredential("devMailServer@shweidong.com", "SHWDDevServer2017"),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            var serverMail = new MailAddress("devMailServer@shweidong.com", "SHWD_Dev_Server", Encoding.UTF8);
            var mailMsg = new MailMessage { From = serverMail };
            mailMsg.To.Add("autyan@shweidong.com");
            mailMsg.Subject = "Unicom Data Upgrade Failed.";
            mailMsg.SubjectEncoding = Encoding.UTF8;
            mailMsg.Body = message;
            mailMsg.BodyEncoding = Encoding.UTF8;
            mailMsg.Priority = MailPriority.High;
            client.SendCompleted += OnMailSendCompleted;
            try
            {
                client.SendAsync(mailMsg, dev);
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
            DevNotifyCount[asyncCompletedEventArgs.UserState.ToString()].Count += 1;
            DevNotifyCount[asyncCompletedEventArgs.UserState.ToString()].LastDateTime = DateTime.Now;
        }
    }

    internal class NotifyCountArgs
    {
        public int Count { get; set; }

        public DateTime LastDateTime { get; set; }
    }
}
