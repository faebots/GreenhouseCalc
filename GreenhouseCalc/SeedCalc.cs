using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;

namespace GreenhouseCalc
{
    public class SeedCalc
    {        
        public List<Seed> SeedList;
        public List<Item> ItemList;
        public List<YieldTier> YieldTiers;

        public SeedCalc() 
        {
            var ass = typeof(SeedCalc).Assembly;
            using (var sr = new StreamReader(ass.GetManifestResourceStream("GreenhouseCalc.data.SeedList.json")))
            {
                var seedListJson = sr.ReadToEnd();
                SeedList = JsonConvert.DeserializeObject<List<Seed>>(seedListJson);
            }
            using (var sr = new StreamReader(ass.GetManifestResourceStream("GreenhouseCalc.data.ItemList.json")))
            {
                var itemListJson = sr.ReadToEnd();
                ItemList = JsonConvert.DeserializeObject<List<Item>>(itemListJson);

            }
            using (var sr = new StreamReader(ass.GetManifestResourceStream("GreenhouseCalc.data.TierList.json")))
            {
                var tierListJson = sr.ReadToEnd();
                YieldTiers = JsonConvert.DeserializeObject<List<YieldTier>>(tierListJson);
                
            }
        }
        
        private Seed GetSeedByName(string name)
        {
            return SeedList.Single(x => x.Name == name);
        }

        private YieldTier GetTierByTier (decimal tier)
        {
            return YieldTiers.SingleOrDefault(x => x.Tier == tier);
        }

        private decimal GetScore(List<Seed> seeds, int cultivation) {
            decimal combinedRank = 0;
            decimal combinedGrade = 0;
            foreach (var seed in seeds) 
            {
                combinedRank += seed.Rank;
                combinedGrade += seed.Grade;
            }
            var a = (12 - (combinedRank % 12)) * 5;
            
            var b = decimal.Floor(4 * (combinedGrade / 5));
            var c = (cultivation + 4) * 2;

            return      ((12 - (combinedRank % 12)) * 5) +
                         decimal.Floor(4 *(combinedGrade / 5)) +
                        ((cultivation + 4) * 2);
        }

        private YieldTier GetTier(decimal score) {
            var result = YieldTiers.Single(x => x.ScoreRange.Lower <= score &&
                                          x.ScoreRange.Upper >= score);
            return result;
        }

        private List<ItemChance> GetPossibleItems(Seed seed, YieldTier tier)
        {
            var baseTier = decimal.Floor(tier.Tier);
            var actualTier = tier.Tier;
            var possibleItems = ItemList.Where(x => x.Seeds.Any(y => y.Name == seed.Name && 
                y.Tiers.Any(z => decimal.Floor(z.Tier) == baseTier)))
            .Select(x => new Item()
            {
                Name = x.Name,
                Seeds = x.Seeds.Select(y => new SeedTier()
                    {   Name = y.Name,
                        Tiers = y.Tiers.Where(z => decimal.Floor(z.Tier) == baseTier).ToList()
                    })
                    .Where(y => y.Name == seed.Name && y.Tiers.Any())
                    .ToList()
                });

            List<ItemChance> results = new List<ItemChance>();

            foreach (var item in possibleItems) {
                // take count for each applicable tier
                var tierCounts = item.Seeds.Single().Tiers.Where(x => decimal.Floor(x.Tier) == baseTier);

                // count x ratio for that tier
                var mainTierRatio = tier.Ratio;
                var subTierRatio = 1 - mainTierRatio;

                var mainTier = tierCounts.Where(x => x.Tier == actualTier).SingleOrDefault();
                var subTier = tierCounts.Where(x => x.Tier != actualTier).SingleOrDefault();
                decimal mainTierCount = mainTier == null ? 0 : mainTier.Count;
                decimal subTierCount = subTier == null ? 0 : subTier.Count;

                var mainTierChance = mainTierRatio * (mainTierCount / 5);
                var subTierChance = subTierRatio * (subTierCount / 5);

                if (mainTierChance + subTierChance > 0)
                    results.Add(new ItemChance(item.Name, mainTierChance + subTierChance));
            }
            return results;
        }
        
        public List<(decimal,SeedTier)> OrderPossibleSeedsByProbability (Item item)
        {
            var seeds = item.Seeds;
            var seedTiers = new List<(decimal, SeedTier)>();

            var seedsByTier = seeds.SelectMany(x => x.Tiers.Select(y => new
            {
                Name = x.Name,
                Count = y.Count,
                RowRatio = (decimal)y.Count / 5,
                Tier = y.Tier,
                MainTier = Math.Floor(y.Tier)
            }));
            
            foreach (var tier in seedsByTier)
            {
                var opposingTier = seedsByTier.SingleOrDefault(x => x.Name == tier.Name &&
                                                                    x.MainTier == tier.MainTier &&
                                                                    x.Tier != tier.Tier);

                var ratio = GetTierByTier(tier.Tier).Ratio;
                decimal opposingRatio = 0;
                if (opposingTier != null)
                    opposingRatio = (opposingTier.RowRatio * (1 - ratio));
                var total = (tier.RowRatio * ratio) + opposingRatio;
                seedTiers.Add((total, new SeedTier
                {
                    Name = tier.Name,
                    Tiers = new List<TierCount>()
                    {
                        new TierCount()
                        {
                            Count = tier.Count,
                            Tier = tier.Tier
                        }
                    }
                }));

                if (opposingTier == null)
                {
                    ratio = 1 - ratio;
                    total = tier.RowRatio * ratio;
                    decimal subtier;
                    if (tier.MainTier == tier.Tier)
                        subtier = tier.Tier + (decimal)0.5;
                    else
                        subtier = tier.MainTier;

                    seedTiers.Add((total, new SeedTier
                    {

                        Name = tier.Name,
                        Tiers = new List<TierCount>()
                        {
                            new TierCount()
                            {
                                Count = tier.Count,
                                Tier = subtier
                            }
                        }
                    }));
                }
            }

            seedTiers = seedTiers.OrderByDescending(x => x.Item1).ToList();

            return seedTiers;
        }

        public (int,int) GetTierMinMaxRankSum(YieldTier tier)
        {
            var minScore = tier.ScoreRange.Lower - 40;
            int modMax = 12;
            if (minScore > 0)
            {
                modMax = (int)decimal.Floor(Math.Abs(12 - (decimal)minScore / 5));
            }

            if (modMax > 11)
                modMax = 11;

            var maxScore = tier.ScoreRange.Upper - 10;
            var modMin = 12 - (int)decimal.Floor((decimal)maxScore / 5);

            if (modMin < 0)
                modMin = 0;
            return (modMin, modMax);
        }

        public (int,int) GetTierMinMaxGradeSum(YieldTier tier, (int min, int max) minMaxMod, int currGradeSum)
        {
            var maxA = (12 - minMaxMod.min) * 5;
            var minA = (12 - minMaxMod.max) * 5;

            var minScore = tier.ScoreRange.Lower - (20 + maxA);
            var maxScore = tier.ScoreRange.Upper - (10 + minA);

            int minGrade, maxGrade;
            minGrade = (int)Math.Ceiling((minScore * 5) / 4.0) - currGradeSum;
            if (minGrade < 0)
                minGrade = 0;

            maxGrade = (int)Math.Ceiling((maxScore * 5) / 4.0) - currGradeSum;
            if (maxGrade > 20)
                maxGrade = 20;

            return (minGrade, maxGrade);
        }

        public List<Seed> GetComboFromSetTypes(YieldTier tier, List<Seed> seeds, (int,int) minMaxMod, (int,int) minMaxGrade)
        {
            var seedsUnique = seeds.Distinct().ToList();
            if (seedsUnique.Count == 5)
                return seeds;

            int remainingSpace = 5 - seedsUnique.Count;
            while (seeds.Count)
        }

        public List<Seed> BuildSeedCombo (YieldTier tier, List<Seed> seeds, List<Seed> others)
        {
            var remainingSpaces = 5 - seeds.Count();
            if (remainingSpaces < 1)
                return null;
            (int min, int max) minMaxMod = GetTierMinMaxRankSum(tier);
            var currentGradeSum = seeds.Select(x => x.Grade).Sum();
            (int min, int max) minMaxGradeSum = GetTierMinMaxGradeSum(tier, minMaxMod, currentGradeSum);
            if (currentGradeSum == minMaxGradeSum.max)
                return null;
            var currentMod = seeds.Select(x => x.Rank).Sum() % 12;
            var stack = new Stack<Seed>(others);

            while (stack.Count() > 0)
            {
                var seed = stack.Pop();
                if (minMaxGradeSum.max - currentGradeSum >= seed.Grade)
                {
                    others = stack.Where(x => x.Grade <= minMaxGradeSum.max - currentGradeSum).ToList();


                } else
                {

                }

            }
        }

        public List<Seed> GetSeedCombo(Seed seed, YieldTier tier, Item item)
        {
            var seeds = new List<Seed>() { seed };
            if (GetScore(seeds, 6) > tier.ScoreRange.Lower && GetScore(seeds, 1) < tier.ScoreRange.Upper)
                return seeds;


            var rank = seed.Rank;
            var grade = seed.Grade;
            var seedMod = rank % 12;
            (int min, int max) minMaxMod = GetTierMinMaxRankSum(tier);
            for (int i = 2; i < 6; i++)
            {
                var seedComboMod = (seedMod * i) % 12;
                if (seedComboMod < minMaxMod.min || seedComboMod > minMaxMod.max)
                    continue;

                var rankCalc = (12 - seedComboMod) * 5;
                var gradeCalc = Math.Floor(((grade * i) / 5.0) * 4);
                if (rankCalc + gradeCalc > tier.ScoreRange.Lower - 20 ||
                    rankCalc + gradeCalc < tier.ScoreRange.Upper - 10)
                    return Enumerable.Repeat(seed, i).ToList();
            }
            
            var maxB = 4 * (grade + 20) / 5;
            var minB = 4 * (grade + 1) / 5;

            minMaxMod.min = minMaxMod.max - seedMod;
            if (minMaxMod.min < 0)
                minMaxMod.min = 0;
            minMaxMod.max = minMaxMod.max - (seed.Rank % 12);
            var otherSeeds = OrderPossibleSeedsByProbability(item).Where(x => x.Item2.Name != seed.Name &&
                                                                              x.Item2.Tiers.Any(y => Math.Floor(y.Tier) == Math.Floor(tier.Tier)))
                                                                  .Select(x => GetSeedByName(x.Item2.Name)).ToList();
            otherSeeds.Reverse();
            var seedStack = new Stack<Seed>(otherSeeds);
            var combo = BuildSeedCombo(tier, seeds, seedStack);

            return seeds;
        }

        public int GetOptimalCultivationTier(List<Seed> seeds, YieldTier tier)
        {
            var rawScore = GetScore(seeds, 0);
            if (rawScore < tier.ScoreRange.Lower - 10)
            {
                var diff = tier.ScoreRange.Lower - rawScore;
                if (diff > 20)
                    return 0;
                if (diff % 2 != 0)
                    diff++;
                return (int)(diff / 2 - 4);
            }
            return 1;
        }

        public (int, List<Seed>) OptimizeItemChances(string itemName)
        {
            var item = ItemList.SingleOrDefault(x => x.Name == itemName);
            if (item == null)
                return (-1, null); 

            var possibleSeeds = OrderPossibleSeedsByProbability(item);
            var possibleSeedTypes = possibleSeeds.Select(x => x.Item2.Name).Distinct();

            decimal maxRatio = 0;
            int optimalCultivationTier = 0;
            List<Seed> maxRatioSeeds = null;
            foreach (var seedRatio in possibleSeeds)
            {
                var ratio = seedRatio.Item1;
                var seed = GetSeedByName(seedRatio.Item2.Name);
                var tier = GetTierByTier(seedRatio.Item2.Tiers[0].Tier);
                var combo = GetSeedCombo(seed, tier, item);
                var seedTypeRatio = (decimal)combo.Count(x => x.Name == seed.Name) / combo.Count;
                if (seedTypeRatio * ratio > maxRatio)
                {
                    maxRatio = seedTypeRatio * ratio;
                    maxRatioSeeds = combo;
                    optimalCultivationTier = GetOptimalCultivationTier(combo, tier);
                }
            }

            return (optimalCultivationTier, maxRatioSeeds);
        }

        public List<ItemSet> GetHarvest (List<string> seedStr, int cultivationTier)
        {
            List<Seed> seeds = seedStr.Select(GetSeedByName).ToList();
            var score = GetScore(seeds, cultivationTier);
            var tier = GetTier(score);
            var results = new List<ItemSet>();
            var uniqueSeeds = seeds.Distinct();
            foreach (var seed in uniqueSeeds)
            {
                var list = GetPossibleItems(seed, tier);
                decimal chance = seeds.Count(x => x == seed) / seeds.Count();
                results.Add(new ItemSet(seed.Name, list, chance));
            }
            return results;
        }

    }
}
