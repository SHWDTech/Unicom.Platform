using System;
using Unicom.Platform.Model;
using Unicom.Platform.SQLite;

namespace Unicom.Platform.Service
{
    class Program
    {
        static void Main()
        {
            var context = new UnicomContext("Data Source=App_Data/Unicom_Platform.sqlite3");

            var device = new EmsDevice()
            {
                Code = "11aaaaa",
                Id = 7,
                OnTransfer = true
            };

            context.AddOrUpdate(device);

            Console.ReadKey();
        }
    }
}
