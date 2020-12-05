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
            EmailProcessor emailProcessor = new EmailProcessor();
            emailProcessor.ProcessEmails();
        }
    }
}
