using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailUtility
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Utility Started");
            EmailProcessor emailProcessor = new EmailProcessor();
            emailProcessor.ProcessEmails();
            Console.WriteLine("Utility Completed");
        }
    }
}
