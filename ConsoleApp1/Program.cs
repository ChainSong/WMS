using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                for (int i = 0; i < 100000; i++)
                {
                    var s = RedisOperation.GetList<string>("BackRF:pear1");
                    //RedisOperation.Exists("pear1");
                    Console.WriteLine("---" + s + "--" + i);

                }
            }
            catch (Exception ex)
            {

            }

            Console.ReadLine();
        }
    }
}
