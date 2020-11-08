using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenhouseCalc;

namespace GreenhouseTestingFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            var calc = new SeedCalc();
            var seed = calc.SeedList[0].Name;
            calc.OptimizeItemChances(seed);
            //CalcTest();
            Console.ReadLine();
        }

        /*static void CalcTest()
        {
            var calc = new SeedCalc();
            var vegetableSeeds = calc.GetSeedByName("Vegetable Seeds");
            List<Seed> set = new List<Seed>();
            for (int i = 0; i < 5; i++)
            {
                set.Add(vegetableSeeds);
                var num = calc.GetScore(set, 1);
                Console.WriteLine($"{i + 1} Vegetable Seeds: {num}");
                var tier = calc.GetTier(num);
                Console.WriteLine($"Tier: {tier.Tier}");

                var results = calc.GetHarvest(set, 1);
                foreach (var result in results)
                {
                    Console.WriteLine($"Seed: {result.Seed}, chances of this set: {result.Probability * 100}%");
                    foreach (var item in result.Items)
                    {
                        Console.WriteLine($"{item.Name} - {item.Probability * 100}% chance");
                    }
                }
                Console.Write("\n");
            }
        }
        private static void ValidateAndPrint()
        {
            var calc = new SeedCalc();
            for (int t = 1; t < 4; t++)
            {
                Console.WriteLine($"=====YIELD LEVEL {t}=====");
                foreach (var seed in calc.SeedList)
                {
                    
                    var items = calc.ItemList.Where(x => x.Seeds.Any(y => y.Name == seed.Name &&
                                                    y.Tiers.Any(z => decimal.Floor(z.Tier) == t)))
                                             .Select(x => new Item()
                                             {
                                                 Name = x.Name,
                                                 Seeds = x.Seeds.Where(y => y.Name == seed.Name)
                                                                   .Select(y => new SeedTier()
                                                                   {
                                                                       Name = y.Name,
                                                                       Tiers = y.Tiers.Where(z => decimal.Floor(z.Tier) == t).ToList()
                                                                   }).ToList()
                                             });

                    decimal row = t;
                    while (row < t + 1)
                    {
                        if (row == t)
                            Console.Write($"{seed.Name,-22}\t");
                        else
                            Console.Write($"{"",-22}\t");
                        var rowItems = items.Where(x => x.Seeds.Single().Tiers.Any(y => y.Tier == row));
                        foreach (var item in rowItems)
                        {

                            var count = item.Seeds.SingleOrDefault().Tiers.Single(x => x.Tier == row).Count;
                            for (int i = 0; i < count; i++)
                                Console.Write($"{item.Name,-22}\t");
                        }
                        Console.Write($"\n");
                        row += (decimal)0.5;
                    }
                }
                Console.Write("\n");
            }*/
        //}
    }
}
