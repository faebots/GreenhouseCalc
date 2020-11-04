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
            ValidateAndPrint();
        }

        private static void ValidateAndPrint()
        {
            var calc = new SeedCalc();
            var errors = new List<string>();
            foreach (var tier in calc.YieldTiers)
            {
                Console.WriteLine($"==============TIER {tier.Tier}==============");
                foreach (var seed in calc.SeedList)
                {
                    Console.Write($"{seed.Name,-22}\t");
                    int total = 0;
                    var bottom = tier.BottomRow.Where(x => x.Seed == seed.Name);
                    foreach (var item in bottom)
                    {
                        total += item.Count;
                        for (int i = 0; i < item.Count; i++)
                            Console.Write($"{item.Name,-22}\t");
                    }
                    Console.Write($"\n{"",-22}\t");
                    if (!bottom.Any())
                        errors.Add($"Missing bottom row: tier {tier.Tier}, seed {seed.Name}");
                    else
                    {
                        if (total != 5)
                            errors.Add($"Count err: Tier {tier.Tier}, seed {seed.Name}, bottom row, total: {total}");
                    }

                    total = 0;
                    var top = tier.TopRow.Where(x => x.Seed == seed.Name);
                    foreach (var item in top)
                    {
                        total += item.Count;
                        for (int i = 0; i < item.Count; i++)
                            Console.Write($"{item.Name,-22}\t");
                    }
                    Console.Write("\n\n");
                    if (!top.Any())
                        errors.Add($"Missing top row: tier {tier.Tier}, seed {seed.Name}");
                    else
                    {
                        if (total != 5)
                            errors.Add($"Count err: Tier {tier.Tier}, seed {seed.Name}, top row, total: {total}");
                    }
                }
                var anomalyBottom = tier.BottomRow.Where(x => !calc.ItemList.Contains(x.Name));
                var anomalyTop = tier.TopRow.Where(x => !calc.ItemList.Contains(x.Name));
                foreach (var item in anomalyBottom)
                    errors.Add($"Unknown item {item.Name}: tier {tier.Tier}, seed {item.Seed}, bottom row");
                foreach (var item in anomalyTop)
                    errors.Add($"Unknown item {item.Name}: tier {tier.Tier}, seed {item.Seed}, top row");
            }

            if (errors.Count == 0)
                Console.WriteLine("No errors!");
            else
            {
                foreach (var err in errors)
                {
                    Console.WriteLine(err);
                }
            }
        }
    }
}
