using System;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Text;
using ESMonitor.DataProvider;

namespace UnicomTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(DateTime.Today.AddHours(-4).AddMinutes(-25).AddSeconds(-55) < DateTime.Today);
            //Console.WriteLine(DateTime.Today.ToLongTimeString());
            //DoIt();
            GetMin();
            Console.ReadKey();
        }

        static void GetMin()
        {
            var provider = new EsMonitorDataProvider();
            var datas = provider.GetCurrentHourEmsDatas("17");
            if (datas != null)
            {
                foreach (var emsData in datas)
                {
                    Console.WriteLine(emsData.dust);
                }
            }
        }

        static void DoIt()
        {
            var client = new SmtpClient("smtp.exmail.qq.com", 25)
            {
                Credentials = new NetworkCredential("devMailServer@shweidong.com", "SHWDDevServer2017"),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            var serverMail = new MailAddress("devMailServer@shweidong.com", "SHWD_Dev_Server", Encoding.UTF8);
            var mailMsg = new MailMessage {From = serverMail};
            mailMsg.To.Add("autyan@shweidong.com");
            mailMsg.Subject = "Testing Mail";
            mailMsg.SubjectEncoding = Encoding.UTF8;
            mailMsg.Body = "Test if you can read this.";
            mailMsg.BodyEncoding = Encoding.UTF8;
            mailMsg.Priority = MailPriority.High;
            client.SendCompleted += ClientOnSendCompleted;
            try
            {
                client.SendAsync(mailMsg, mailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ClientOnSendCompleted(object sender, AsyncCompletedEventArgs asyncCompletedEventArgs)
        {
            Console.WriteLine($"{asyncCompletedEventArgs.Cancelled}\r\n");
            if (asyncCompletedEventArgs.Error != null)
            {
                Console.WriteLine(asyncCompletedEventArgs.Error.Message);
            }
        }
    }
}
