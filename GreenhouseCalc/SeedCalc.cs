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
        public List<Seed> SeedList = new List<Seed>();
        public List<Item> ItemList = new List<Item>();
        public List<YieldTier> YieldTiers = new List<YieldTier>();

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

        public List<SeedTier> OrderPossibleSeedsByProbability (Item )
        {

        }
        public List<ItemSet> OptimizeItemChances(string itemName)
        {
            var item = ItemList.SingleOrDefault(x => x.Name == itemName);
            if (item == null)
                return null;


            var possibleSeeds = item.Seeds.SelectMany(x => x.Tiers.Select(y => new SeedTier
            {
                Name = x.Name,
                Tiers = new List<TierCount>
                { new TierCount { Tier = y.Tier,
                    Count = y.Count } }
            }).OrderByDescending(x => x.Tiers[0].Tier * x.Tiers[0].Count);






            return new List<ItemSet>();
        }

        public List<ItemSet> GetHarvest (List<string> seedStr, int cultivationTier)
        {
            List<Seed> seeds = seedStr.Select(x => GetSeedByName(x)).ToList();
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
