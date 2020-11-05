using System;
using GreenhouseCalc;
using System.Collections.Generic;
using System.Linq;

namespace GreenhouseConsoleTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var calc = new SeedCalc();
            var seedList = calc.SeedList;
            var itemList = calc.ItemList;
            var tierList = calc.YieldTiers;
            //ValidateAndPrint();
        }

        
    }
}
