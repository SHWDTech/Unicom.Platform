using System;
using Unicom.Platform;

namespace UnicomTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new UnicomService();
            var code = service.RegisterVendor("上海境优环保科技有限公司");
            Console.WriteLine(code);
            Console.ReadKey();
        }
    }
}
