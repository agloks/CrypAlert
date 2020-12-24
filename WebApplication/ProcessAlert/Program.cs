using System;

namespace ProcessAlert
{
    class Program
    {
        static void Main(string[] args)
        {
            var orc = new Orchestrator("123");
            var let = new Letter(MessageType.Email, "Testing here", Env.PHONE, Env.EMAIL);
            orc.CreateWorker("ETH_BRL", "3350", "3300", "test");
            orc.RunActualWorker(let);
            Console.WriteLine("Hello World!");
        }
    }
}