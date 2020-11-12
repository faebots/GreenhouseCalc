using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            //CalcTest();
            /*
            string response;
            int count;
            do
            {
                Console.WriteLine("How many items?");
                response = Console.ReadLine();
            } while (!int.TryParse(response, out count));

            Console.WriteLine($"Type {count} characters.");
            int slots;
            var charlist = new List<char>();
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"\n{i + 1}:");
                var key = Console.ReadKey();
                charlist.Add(key.KeyChar);

            }

            do
            {
                Console.WriteLine("How many slots?");
                response = Console.ReadLine();
            } while (!int.TryParse(response, out slots));
            */
            var startlist = new List<char>() { 'a' };
            var charlist = new Stack<char>(new List<char>() { 'a', 'b', 'c' });
            var slots = 4;

            Level = 0;

            var stuff = new List<Dictionary<char, int>>();
            for (int x = 1; x < 6; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    if (x + y > 5)
                        break;
                    for (int z = 0; z < 6; z++)
                    {
                        if (x + y + z > 5)
                            break;
                        var dic = new Dictionary<char, int>();
                        dic.Add('a', x);
                        dic.Add('b', y);
                        dic.Add('c', z);
                        stuff.Add(dic);

                    }
                }
            }

            foreach (var blrag in stuff)
            {
                var listy = new List<String>()
                {
                    $"'a':{blrag['a']}",
                    $"'b':{blrag['b']}",
                    $"'c':{blrag['c']}"
                };
                Console.WriteLine(PrintList(listy));
            }


            /*
            foreach (var per in permutations)
            {
                foreach (var chr in per)
                    Console.Write($"{chr},");
                Console.Write('\n');
            }
            
            /*
            foreach (var per in permutations)
            {
                var counts = new Dictionary<char, int>();
                foreach (var ch in charlist)
                    counts.Add(ch, per.Count(x => x == ch));
                Console.WriteLine($"{{ {counts[charlist[2]]}, {counts[charlist[1]]}, {counts[charlist[0]]} }}");
            }
            */

            Console.ReadKey();
        }

        static string PrintList(List<char> list)
        {
            var sb = new StringBuilder();
            sb.Append("{ ");
            for (int i = 0; i < list.Count; i++)
            {
                if (i != 0)
                    sb.Append(',');
                sb.Append(list[i]);
            }
            sb.Append(" }");
            return sb.ToString();
        }

        static string PrintList(List<string> list)
        {
            var sb = new StringBuilder();
            sb.Append("{ ");
            for (int i = 0; i < list.Count; i++)
            {
                if (i != 0)
                    sb.Append(',');
                sb.Append(list[i]);
            }
            sb.Append(" }");
            return sb.ToString();
        }

        static string PrintList(Stack<char> list)
        {
            return PrintList(list.ToList());
        }

        static int Level;

       static List<Dictionary<char, int>> Permutation2(Stack<char> availableItems, int slots)
        {
            var results = new List<Dictionary<char, int>>() { };
            if (slots == 0)
            {
                var dic = new Dictionary<char, int>();
                foreach (var x in availableItems)
                {
                    dic.Add(x, 0);
                }
                return new List<Dictionary<char, int>>() { dic };
            }
            if (availableItems.Count == 0)
                results.Add(new Dictionary<char, int>());
            while (availableItems.Count > 0)
            {
                var item = availableItems.Pop();
                for (int i = 0; i <= slots; i++)
                {
                    var branch = Permutation2(availableItems, slots - i);
                    foreach (var twig in branch)
                        twig.Add(item, i);
                    results.AddRange(branch);
                }
            }

            return results;
        }

        static List<List<char>> PermutationGetter(List<char> currentList, Stack<char> availableItems, int slots)
        {
            var spacing = string.Concat(Enumerable.Repeat('\t', Level).ToArray());
            Level++;
            Console.WriteLine($"{spacing}CALL: {PrintList(currentList)}, {PrintList(availableItems)}, {slots} AT LEVEL: {Level}");
            if (currentList == null || availableItems == null)
            {
                Console.WriteLine($"{spacing}RETURN: {{  }}");
                Level--;
                return new List<List<char>>();
            }
            if (currentList.Count == 0)
            {
                Console.WriteLine($"{spacing}RETURN: {{  }}");
                Level--;
                return new List<List<char>>();
            }
            if (availableItems.Count == 0 || slots == 0)
            {
                Console.WriteLine($"{spacing}RETURN: {PrintList(currentList)}");
                Level--;
                var x = Console.ReadKey();
                if (x.KeyChar != '\n')
                    Console.Write('\n');
                return new List<List<char>>() { currentList };
            }


            //cycle through adding items
            var results = new List<List<char>>();
          
            while (availableItems.Count > 0) { 
                var item = availableItems.Pop();
                Console.WriteLine($"{spacing}{item}");
                Console.WriteLine($"{spacing}Loop:");
                for (int i = 0; i <= slots; i++)
                {
                    Console.WriteLine($"{spacing}{i}");
                    var newList = new List<char>();
                    newList.AddRange(currentList);
                    newList.AddRange(Enumerable.Repeat(item, i));
                    results.AddRange(PermutationGetter(newList, availableItems, slots - i));
                }
            }
            
            Level--;
            return results;
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
