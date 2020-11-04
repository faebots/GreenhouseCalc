using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
namespace GreenhouseCalc
{
    public class SeedCalc
    {        
        public List<Seed> SeedList = new List<Seed>();

        public List<Item> ItemList = new List<Item>();

        public List<YieldTier> YieldTiers = new List<YieldTier>();

        public YieldTier GetTier(int tier)
        {
            return YieldTiers.Single(x => x.Tier == tier.ToString());
        }

        public Seed GetSeedByName(string name)
        {
            return SeedList.Single(x => x.Name == name);
        }

        public decimal GetScore(Seed[] seeds, int cultivation) {
            var combinedRank = 0;
            var combinedGrade = 0;
            foreach (var seed in seeds) 
            {
                combinedRank += seed.Rank;
                combinedGrade += seed.Grade;
            }

            return      ((12 - (combinedRank % 12)) * 5) +
                        (4 * (combinedGrade / 5)) +
                        ((cultivation + 4)* 2);
        }

        public YieldTier GetTier(decimal score) {
            return YieldTiers.Single(x => x.ScoreRange[0] <= score &&
                                          x.ScoreRange[1] >= score);
        }

        public List<(string, decimal)> GetPossibleItems(Seed seed, YieldTier tier)
        {
            var possibleItems = ItemList.Where(item => 
                    item.Seeds.Any(s => s.Name == seed.Name &&
                                    s.Tiers.Any(t => Math.Floor(t.Tier) == Math.Floor(tier.Tier))));
            
            foreach (var item in possibleItems) {
                // take count for each applicable tier
                // count x ratio for that tier
                // add them together probably???
            }
        }

        public void InitializeYieldTiers() {
            var tierList = new List<YieldTier>() {
                new YieldTier("1", "2:8", "7:3", 3, 1, 0, 20, 21, 40),
                new YieldTier("2", "4:6", "7:3", 10, 5, 41, 60, 61, 80),
                new YieldTier("3", "3:7", "8:2", 20, 15, 81, 90, 91, 100)
            };

            foreach (var tier in tierList)
            {
                foreach (var seed in SeedList)
                {
                    switch (seed.Name)
                    {
                        case "Mixed Herb Seeds":
                            switch (tier.Tier)
                            {
                                case "1":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Weeds",seed.Name,2),
                                        new Item("Onion",seed.Name,1),
                                        new Item("Mixed Herb Seeds",seed.Name,2)
                                    });
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Mixed Herb Seeds",seed.Name,2),
                                        new Item("Turnip",seed.Name,1),
                                        new Item("Cabbage",seed.Name,1),
                                        new Item("Peach Currant", seed.Name,1)
                                    });
                                    break;
                                case "2":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Mixed Herb Seeds", seed.Name, 1),
                                        new Item("Albinean Berries", seed.Name, 1),
                                        new Item("Turnip", seed.Name, 2),
                                        new Item("Mixed Fruit Seeds", seed.Name, 1)
                                    });
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Mixed Herb Seeds", seed.Name, 1),
                                        new Item("Peach Currant", seed.Name, 1),
                                        new Item("Turnip", seed.Name, 1),
                                        new Item("Yellow Flower Seeds", seed.Name, 1),
                                        new Item("Mixed Fruit Seeds", seed.Name, 1)
                                    });
                                    break;
                                case "3":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Mixed Herb Seeds"),
                                        new Item("Carrot"),
                                        new Item("Northern Fodlan Seeds"),
                                        new Item("Chickpeas"),
                                        new Item("Cabbage")
                                    }.Select(x => new Item(x.Name, seed.Name)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Western Fodlan Seeds"),
                                        new Item("Tomato"),
                                        new Item("Northern Fodlan Seeds"),
                                        new Item("Daffodil"),
                                        new Item("Chickpeas")
                                    }.Select(x => new Item(x.Name, seed.Name)));
                                    break;
                            }
                            break;
                        case "Western Fodlan Seeds":
                            switch (tier.Tier)
                            {
                                case "1":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Zanado Fruit",seed.Name,2),
                                        new Item("Carrot",seed.Name, 1),
                                        new Item("Western Fodlan Seeds",seed.Name,2)
                                    });
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Western Fodlan Seeds", seed.Name, 1),
                                        new Item("Noa Fruit", seed.Name, 1),
                                        new Item("Chickpeas", seed.Name, 1),
                                        new Item("Cabbage", seed.Name, 1),
                                        new Item("Root Vegetable Seeds", seed.Name, 1)
                                    });
                                    break;
                                case "2":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Western Fodlan Seeds", seed.Name, 1),
                                        new Item("Noa Fruit", seed.Name, 2),
                                        new Item("Peach Currant", seed.Name, 1),
                                        new Item("Vegetable Seeds", seed.Name, 1)
                                    });
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Western Fodlan Seeds", seed.Name, 1),
                                        new Item("Noa Fruit", seed.Name, 1),
                                        new Item("Tomato", seed.Name, 1),
                                        new Item("Green Flower Seeds", seed.Name, 1),
                                        new Item("Albinean Berries", seed.Name, 1)
                                    });
                                    break;
                                case "3":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Western Fodlan Seeds"),
                                        new Item("Noa Fruit"),
                                        new Item("Southern Fodlan Seeds"),
                                        new Item("Blue Flower Seeds"),
                                        new Item("Carrot")
                                    }.Select(x => new Item(x.Name, seed.Name)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Root Vegetable Seeds"),
                                        new Item("Noa Fruit"),
                                        new Item("Southern Fodlan Seeds"),
                                        new Item("Sunflower"),
                                        new Item("Onion")
                                    }.Select(x => new Item(x.Name, seed.Name)));
                                    break;
                            }
                            break;
                        case "Root Vegetable Seeds":
                            switch (tier.Tier)
                            {
                                case "1":
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Root Vegetable Seeds",seed.Name,1),
                                        new Item("Albinean Berries", seed.Name, 1),
                                        new Item("Tomato", seed.Name, 1),
                                        new Item("Cabbage", seed.Name, 2)
                                    });
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Weeds", seed.Name, 2),
                                        new Item("Chickpeas", seed.Name, 1),
                                        new Item("Root Vegetable Seeds", seed.Name, 2)
                                    });
                                    break;
                                case "2":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Root Vegetable Seeds", seed.Name, 1),
                                        new Item("Carrot", seed.Name, 1),
                                        new Item("Cabbage", seed.Name, 1),
                                        new Item("Peach Currant", seed.Name, 1),
                                        new Item("Northern Fodlan Seeds", seed.Name, 1)
                                    });
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Root Vegetable Seeds", seed.Name, 1),
                                        new Item("Noa Fruit", seed.Name, 1),
                                        new Item("Cabbage", seed.Name, 1),
                                        new Item("Pale-Blue Flower Seeds", seed.Name, 1),
                                        new Item("Northern Fodlan Seeds", seed.Name, 1)
                                    });
                                    break;
                                case "3":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Root Vegetable Seeds"),
                                        new Item("Tomato"),
                                        new Item("Morfis Seeds"),
                                        new Item("Purple Flower Seeds"),
                                        new Item("Chickpeas")
                                    }.Select(x => new Item(x.Name, seed.Name)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Blue Flower Seeds"),
                                        new Item("Peach Currant"),
                                        new Item("Morfis Seeds"),
                                        new Item("Violet"),
                                        new Item("Turnip")
                                    }.Select(x => new Item(x.Name, seed.Name)));
                                    break;
                            }
                            break;
                        case "Vegetable Seeds":
                            switch (tier.Tier)
                            {
                                case "1":
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Dried Vegetables", seed.Name, 1),
                                        new Item("Chickpeas", seed.Name, 1),
                                        new Item("Onion", seed.Name, 1),
                                        new Item("Peach Currant", seed.Name, 1),
                                        new Item("Turnip", seed.Name, 1)
                                    });
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Dried Vegetables", seed.Name, 2),
                                        new Item("Chickpeas", seed.Name, 1),
                                        new Item("Vegetable Seeds", seed.Name, 2)
                                    });
                                    break;
                                case "2":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Vegetable Seeds", seed.Name, 1),
                                        new Item("Onion", seed.Name, 1),
                                        new Item("Tomato", seed.Name, 1),
                                        new Item("Turnip", seed.Name, 1),
                                        new Item("Mixed Herb Seeds", seed.Name, 1)
                                    });
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Vegetable Seeds", seed.Name, 1),
                                        new Item("Peach Currant", seed.Name, 1),
                                        new Item("Onion", seed.Name, 1),
                                        new Item("Purple Flower Seeds", seed.Name, 1),
                                        new Item("Noa Fruit", seed.Name, 1)
                                    });
                                    break;
                                case "3":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Noa Fruit"),
                                        new Item("Onion"),
                                        new Item("Albinean Seeds"),
                                        new Item("Magdred Kirsch"),
                                        new Item("Carrot")
                                    }.Select(x => new Item(x.Name, seed.Name)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Nordsalat Seeds"),
                                        new Item("Verona"),
                                        new Item("Albinean Seeds"),
                                        new Item("Lily of the Valley"),
                                        new Item("Onion")
                                    }.Select(x => new Item(x.Name, seed.Name)));
                                    break;
                            }
                            break;
                        case "Northern Fodlan Seeds":
                            switch (tier.Tier)
                            {
                                case "1":
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Northern Fodlan Seeds", seed.Name, 1),
                                        new Item("Tomato", seed.Name, 1),
                                        new Item("Chickpeas", seed.Name, 1),
                                        new Item("Mixed Fruit Seeds", seed.Name, 2)
                                    });
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Dried Vegetables", seed.Name, 2),
                                        new Item("Onion", seed.Name, 1),
                                        new Item("Northern Fodlan Seeds", seed.Name, 2)
                                    });
                                    break;
                                case "2":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Northern Fodlan Seeds", seed.Name, 1),
                                        new Item("Carrot", seed.Name, 1),
                                        new Item("Noa Fruit", seed.Name, 1),
                                        new Item("Mixed Fruit Seeds", seed.Name, 1),
                                        new Item("Root Vegetable Seeds", seed.Name, 1)
                                    });
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Northern Fodlan Seeds", seed.Name, 1),
                                        new Item("Peach Currant", seed.Name, 1),
                                        new Item("Noa Fruit", seed.Name, 1),
                                        new Item("Yellow Flower Seeds", seed.Name, 1),
                                        new Item("Albinean Berries", seed.Name, 1)
                                    });
                                    break;
                                case "3":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Northern Fodlan Seeds"),
                                        new Item("Morfis Plum"),
                                        new Item("Morfis Seeds"),
                                        new Item("White Flower Seeds"),
                                        new Item("Chickpeas")
                                    }.Select(x => new Item(x.Name, seed.Name)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Southern Fodlan Seeds"),
                                        new Item("Chickpeas"),
                                        new Item("Morfis Seeds"),
                                        new Item("Rose"),
                                        new Item("Turnip")
                                    }.Select(x => new Item(x.Name, seed.Name)));
                                    break;
                            }
                            break;
                        case "Morfis-Plum Seeds":
                            switch (tier.Tier)
                            {
                                case "1":
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Zanado Fruit", seed.Name, 1),
                                        new Item("Morfis-Plum Seeds", seed.Name, 1),
                                        new Item("Morfis Plum", seed.Name, 2),
                                        new Item("Peach Currant", seed.Name, 1)
                                    });
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Zanado Fruit", seed.Name, 2),
                                        new Item("Peach Currant", seed.Name, 1),
                                        new Item("Noa Fruit", seed.Name, 1),
                                        new Item("Morfis-Plum Seeds", seed.Name, 1)
                                    });
                                    break;
                                case "2":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Morfis-Plum Seeds", seed.Name, 1),
                                        new Item("Peach Currant", seed.Name, 1),
                                        new Item("Noa Fruit", seed.Name, 1),
                                        new Item("Morfis Plum", seed.Name, 2)
                                    });
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Morfis-Plum Seeds", seed.Name, 1),
                                        new Item("Tomato", seed.Name, 1),
                                        new Item("Morfis Plum", seed.Name, 2),
                                        new Item("Albinean Seeds", seed.Name, 1)
                                    });
                                    break;
                                case "3":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Morfis Plum", 2),
                                        new Item("Eastern Fodlan Seeds"),
                                        new Item("Purple Flower Seeds"),
                                        new Item("Turnip")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Nordsalat Seeds"),
                                        new Item("Magdred Kirsch"),
                                        new Item("Eastern Fodlan Seeds"),
                                        new Item("Forget-me-nots"),
                                        new Item("Zanado Treasure Fruit")
                                    }.Select(x => new Item(x.Name, seed.Name)));
                                    break;
                            }
                            break;
                        case "Southern Fodlan Seeds":
                            switch (tier.Tier)
                            {
                                case "1":
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Southern Fodlan Seeds",seed.Name,1),
                                        new Item("Turnip",seed.Name,1),
                                        new Item("Peach Currant", seed.Name,1),
                                        new Item("Cabbage",seed.Name,1),
                                        new Item("Western Fodlan Seeds", seed.Name,1)
                                    });
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Weeds",seed.Name,2),
                                        new Item("Carrot",seed.Name,1),
                                        new Item("Southern Fodlan Seeds",seed.Name,2)
                                    });
                                    break;
                                case "2":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Southern Fodlan Seeds", seed.Name, 1),
                                        new Item("Turnip", seed.Name, 1),
                                        new Item("Cabbage", seed.Name, 1),
                                        new Item("Peach Currant", seed.Name),
                                        new Item("Northern Fodlan Seeds", seed.Name)
                                    });
                                    tier.TopRow.AddRange(new List<string>()
                                    {
                                        "Southern Fodlan Seeds",
                                        "Turnip",
                                        "Verona",
                                        "Red Flower Seeds",
                                        "Magdred Kirsch"
                                    }.Select<string, Item>(x => new Item(x, seed.Name)).ToList());
                                    
                                    break;
                                case "3":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Southern Fodlan Seeds"),
                                        new Item("Turnip"),
                                        new Item("Morfis Plum"),
                                        new Item("Yellow Flower Seeds"),
                                        new Item("Magdred Kirsch")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Verona"),
                                        new Item("Peach Currant"),
                                        new Item("Morfis Plum"),
                                        new Item("Baby's Breath"),
                                        new Item("Chickpeas")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    break;
                            }
                            break;
                        case "Morfis Seeds":
                            switch (tier.Tier)
                            {
                                case "1":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Weeds",seed.Name,2),
                                        new Item("Turnip",seed.Name,2),
                                        new Item("Morfis Seeds",seed.Name,1)
                                    });
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Morfis Seeds",seed.Name,2),
                                        new Item("Carrot",seed.Name,1),
                                        new Item("Turnip",seed.Name,1),
                                        new Item("Mixed Fruit Seeds",seed.Name,1)
                                    });
                                    break;
                                case "2":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Morfis Seeds",2),
                                        new Item("Peach Currant"),
                                        new Item("Chickpeas"),
                                        new Item("Vegetable Seeds")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<string>()
                                    {
                                        "Morfis Seeds",
                                        "Carrot",
                                        "Turnip",
                                        "White Flower Seeds",
                                        "Vegetable Seeds"
                                    }.Select<string, Item>(x => new Item(x, seed.Name)));
                                    break;
                                case "3":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Morfis Seeds"),
                                        new Item("Albinean Berries"),
                                        new Item("Tomato"),
                                        new Item("Green Flower Seeds"),
                                        new Item("Peach Currant")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Magdred Kirsch"),
                                        new Item("Carrot"),
                                        new Item("Tomato"),
                                        new Item("Anemone"),
                                        new Item("Onion")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    break;
                            }
                            break;
                        case "Nordsalat Seeds":
                            switch (tier.Tier)
                            {
                                case "1":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Dried Vegetables", seed.Name, 1),
                                        new Item("Verona", seed.Name, 1),
                                        new Item("Carrot", seed.Name, 1),
                                        new Item("Nordsalat Seeds", seed.Name, 2)
                                    });
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Dried Vegetables", seed.Name, 1),
                                        new Item("Nordsalat Seeds", seed.Name, 1),
                                        new Item("Chickpeas", seed.Name, 1),
                                        new Item("Nordsalat", seed.Name, 2)
                                    });
                                    break;
                                case "2":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Nordsalat Seeds"),
                                        new Item("Carrot"),
                                        new Item("Nordsalat",3)
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<string>()
                                    {
                                        "Nordsalat Seeds",
                                        "Nordsalat",
                                        "Magdred Kirsch",
                                        "Verona",
                                        "Albinean Berries"
                                    }.Select<string, Item>(x => new Item(x, seed.Name)));
                                    break;
                                case "3":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Nordsalat",2),
                                        new Item("Angelica Seeds"),
                                        new Item("Pale-Blue Flower Seeds"),
                                        new Item("Magdred Kirsch")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Ailell Grass"),
                                        new Item("Angelica"),
                                        new Item("Angelica Seeds"),
                                        new Item("Daffodil"),
                                        new Item("Turnip")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    break;
                            }
                            break;
                        case "Boa-Fruit Seeds":
                            switch (tier.Tier)
                            {
                                case "1":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Weeds",seed.Name,2),
                                        new Item("Peach Currant",seed.Name, 1),
                                        new Item("Boa-Fruit Seeds",seed.Name, 2)
                                    });
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Weeds", seed.Name, 1),
                                        new Item("Boa-Fruit Seeds", seed.Name, 1),
                                        new Item("Boa Fruit", seed.Name, 3)
                                    });
                                    break;
                                case "2":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Boa-Fruit Seeds"),
                                        new Item("Peach Currant"),
                                        new Item("Boa Fruit",3)
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Boa-Fruit Seeds"),
                                        new Item("Boa Fruit",3),
                                        new Item("Angelica Seeds")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    break;
                                case "3":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Boa Fruit",2),
                                        new Item("Zanado Treasure Fruit"),
                                        new Item("Red Flower Seeds"),
                                        new Item("Onion")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Angelica Seeds"),
                                        new Item("Zanado Treasure Fruit"),
                                        new Item("Mixed Herb Seeds"),
                                        new Item("Sunflower"),
                                        new Item("Cabbage")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    break;
                            }
                            break;
                        case "Albinean Seeds":
                            switch (tier.Tier)
                            {
                                case "1":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Zanado Fruit", seed.Name, 2),
                                        new Item("Peach Currant", seed.Name, 1),
                                        new Item("Albinean Seeds", seed.Name, 2)
                                    });
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Albinean Seeds", seed.Name, 1),
                                        new Item("Chickpeas", seed.Name, 1),
                                        new Item("Peach Currant", seed.Name, 1),
                                        new Item("Albinean Berries", seed.Name, 1),
                                        new Item("Mixed Fruit Seeds", seed.Name, 1)
                                    });
                                    break;
                                case "2":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Albinean Seeds", 2),
                                        new Item("Peach Currant"),
                                        new Item("Albinean Berries"),
                                        new Item("Root Vegetable Seeds")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<string>()
                                    {
                                        "Albinean Seeds",
                                        "Carrot",
                                        "Peach Currant",
                                        "Yellow Flower Seeds",
                                        "Root Vegetable Seeds"
                                    }.Select<string, Item>(x => new Item(x, seed.Name)));
                                    break;
                                case "3":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Albinean Seeds"),
                                        new Item("Carrot"),
                                        new Item("Tomato"),
                                        new Item("White Flower Seeds"),
                                        new Item("Turnip")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Albinean Berries"),
                                        new Item("Chickpeas"),
                                        new Item("Tomato"),
                                        new Item("Violet"),
                                        new Item("Carrot")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    break;
                            }
                            break;
                        case "Eastern Fodlan Seeds":
                            switch (tier.Tier)
                            {
                                case "1":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Weeds", seed.Name, 2),
                                        new Item("Tomato", seed.Name, 1),
                                        new Item("Eastern Fodlan Seeds", seed.Name, 2)
                                    });
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Eastern Fodlan Seeds", seed.Name, 1),
                                        new Item("Verona", seed.Name, 1),
                                        new Item("Onion", seed.Name, 2),
                                        new Item("Peach Currant", seed.Name, 1)
                                    });
                                    break;
                                case "2":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Eastern Fodlan Seeds", 2),
                                        new Item("Onion", 2),
                                        new Item("Morfis Seeds")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<string>()
                                    {
                                        "Eastern Fodlan Seeds",
                                        "Peach Currant",
                                        "Onion",
                                        "Green Flower Seeds",
                                        "Morfis Seeds"
                                    }.Select<string, Item>(x => new Item(x, seed.Name)));
                                    break;
                                case "3":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Eastern Fodlan Seeds"),
                                        new Item("Tomato"),
                                        new Item("Nordsalat Seeds"),
                                        new Item("Blue Flower Seeds"),
                                        new Item("Cabbage")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Albinean Berries"),
                                        new Item("Verona"),
                                        new Item("Nordsalat"),
                                        new Item("Pitcher Plant"),
                                        new Item("Chickpeas")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    break;
                            }
                            break;
                        case "Angelica Seeds":
                            switch (tier.Tier)
                            {
                                case "1":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Weeds",seed.Name,2),
                                        new Item("Tomato",seed.Name,1),
                                        new Item("Angelica Seeds", seed.Name,2)
                                    });
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Weeds",seed.Name,1),
                                        new Item("Angelica Seeds", seed.Name, 1),
                                        new Item("Angelica", seed.Name, 2),
                                        new Item("Nordsalat", seed.Name, 1)
                                    });
                                    break;
                                case "2":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Angelica Seeds"),
                                        new Item("Angelica", 3),
                                        new Item("Magdred Kirsch")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Angelica Seeds"),
                                        new Item("Angelica",3),
                                        new Item("Morfis-Plum Seeds")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    break;
                                case "3":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Angelica",2),
                                        new Item("Ailell Grass"),
                                        new Item("Yellow Flower Seeds"),
                                        new Item("Chickpeas")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Boa-Fruit Seeds"),
                                        new Item("Ailell Grass"),
                                        new Item("Western Fodlan Seeds"),
                                        new Item("Lily"),
                                        new Item("Turnip")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    break;
                            }
                            break;
                        case "Mixed Fruit Seeds":
                            switch (tier.Tier)
                            {
                                case "1":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Zanado Fruit", seed.Name, 2),
                                        new Item("Peach Currant", seed.Name, 1),
                                        new Item("Mixed Fruit Seeds", seed.Name, 2)
                                    });
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Mixed Fruit Seeds", seed.Name, 1),
                                        new Item("Northern Fodlan Seeds", seed.Name, 2),
                                        new Item("Peach Currant", seed.Name, 1),
                                        new Item("Root Vegetable Seeds", seed.Name, 1)
                                    });
                                    break;
                                case "2":
                                    tier.BottomRow.AddRange(new List<string>()
                                    {
                                        "Mixed Fruit Seeds",
                                        "Northern Fodlan Seeds",
                                        "Peach Currant",
                                        "Albinean Berries",
                                        "Root Vegetable Seeds"
                                    }.Select(x => new Item(x, seed.Name)));
                                    tier.TopRow.AddRange(new List<string>()
                                    {
                                        "Mixed Fruit Seeds",
                                        "Northern Fodlan Seeds",
                                        "Peach Currant",
                                        "White Flower Seeds",
                                        "Morfis Plum"
                                    }.Select(x => new Item(x, seed.Name)));
                                    break;
                                case "3":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Mixed Fruit Seeds"),
                                        new Item("Northern Fodlan Seeds"),
                                        new Item("Eastern Fodlan Seeds"),
                                        new Item("Green Flower Seeds"),
                                        new Item("Onion")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Northern Fodlan Seeds"),
                                        new Item("Noa Fruit"),
                                        new Item("Eastern Fodlan Seeds"),
                                        new Item("Lily of the Valley"),
                                        new Item("Cabbage")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    break;
                            }
                            break;
                        case "Red Flower Seeds":
                            switch (tier.Tier)
                            {
                                case "1":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Weeds", seed.Name, 3),
                                        new Item("Carnation", seed.Name, 1),
                                        new Item("Red Flower Seeds", seed.Name, 1)
                                    });
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Weeds", seed.Name, 1),
                                        new Item("Red Flower Seeds", seed.Name, 1),
                                        new Item("Yellow Flower Seeds", seed.Name, 1),
                                        new Item("Rose", seed.Name, 1),
                                        new Item("Carnation", seed.Name, 1)
                                    });
                                    break;
                                case "2":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Rose", 2),
                                        new Item("Carnation"),
                                        new Item("Lily"),
                                        new Item("White Flower Seeds")
                                    }.Select(x => new Item(x.Name,seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<string>()
                                    {
                                        "Daffodil",
                                        "Lavender",
                                        "Yellow Flower Seeds",
                                        "Purple Flower Seeds",
                                        "Blue Flower Seeds"
                                    }.Select(x => new Item(x, seed.Name)));
                                    break;
                                case "3":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Daffodil"),
                                        new Item("Onion"),
                                        new Item("Blue Flower Seeds"),
                                        new Item("Albinean Seeds"),
                                        new Item("Rose")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Rose"),
                                        new Item("Mixed Herb Seeds"),
                                        new Item("Albinean Seeds"),
                                        new Item("Nordsalat"),
                                        new Item("Magdred Kirsch")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    break;
                            }
                            break;
                        case "White Flower Seeds":
                            switch (tier.Tier)
                            {
                                case "1":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Weeds", seed.Name, 3),
                                        new Item("Baby's Breath", seed.Name, 1),
                                        new Item("White Flower Seeds", seed.Name, 1)
                                    });
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Weeds", seed.Name, 1),
                                        new Item("White Flower Seeds", seed.Name, 1),
                                        new Item("Green Flower Seeds", seed.Name, 1),
                                        new Item("Daffodil", seed.Name, 1),
                                        new Item("Baby's Breath", seed.Name, 1)
                                    });
                                    break;
                                case "2":
                                    tier.BottomRow.AddRange(new List<string>()
                                    {
                                        "Daffodil",
                                        "Baby's Breath",
                                        "Lily",
                                        "Lily of the Valley",
                                        "Blue Flower Seeds"
                                    }.Select(x => new Item(x, seed.Name)));
                                    tier.TopRow.AddRange(new List<string>()
                                    {
                                        "Sunflower",
                                        "Anemone",
                                        "Green Flower Seeds",
                                        "Yellow Flower Seeds",
                                        "Purple Flower Seeds"
                                    }.Select(x => new Item(x, seed.Name)));
                                    break;
                                case "3":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Sunflower"),
                                        new Item("Southern Fodlan Seeds"),
                                        new Item("Purple Flower Seeds"),
                                        new Item("Turnip"),
                                        new Item("Daffodil")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Daffodil"),
                                        new Item("Vegetable Seeds"),
                                        new Item("Western Fodlan Seeds"),
                                        new Item("Morfis-Plum Seeds"),
                                        new Item("Boa Fruit")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    break;
                            }
                            break;
                        case "Blue Flower Seeds":
                            switch (tier.Tier)
                            {
                                case "1":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Weeds", seed.Name, 3),
                                        new Item("Forget-me-nots", seed.Name, 1),
                                        new Item("Blue Flower Seeds", seed.Name, 1)
                                    });
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Weeds", seed.Name, 1),
                                        new Item("Blue Flower Seeds", seed.Name, 1),
                                        new Item("Pale-Blue Flower Seeds", seed.Name, 1),
                                        new Item("Anemone", seed.Name, 1),
                                        new Item("Forget-me-nots", seed.Name, 1)
                                    });
                                    break;
                                case "2":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Forget-me-nots",2),
                                        new Item("Anemone"),
                                        new Item("Rose"),
                                        new Item("Purple Flower Seeds")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<string>()
                                    {
                                        "Violet",
                                        "Lily of the Valley",
                                        "Pale-Blue Flower Seeds",
                                        "Green Flower Seeds",
                                        "Yellow Flower Seeds"
                                    }.Select(x => new Item(x, seed.Name)));
                                    break;
                                case "3":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Violet"),
                                        new Item("Chickpeas"),
                                        new Item("Yellow Flower Seeds"),
                                        new Item("Eastern Fodlan Seeds"),
                                        new Item("Forget-me-nots")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Forget-me-nots"),
                                        new Item("Root Vegetable Seeds"),
                                        new Item("Eastern Fodlan Seeds"),
                                        new Item("Morfis-Plum Seeds"),
                                        new Item("Nordsalat")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    break;
                            }
                            break;
                        case "Purple Flower Seeds":
                            switch (tier.Tier)
                            {
                                case "1":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Weeds",seed.Name, 3),
                                        new Item("Lavender", seed.Name, 1),
                                        new Item("Purple Flower Seeds", seed.Name, 1)
                                    });
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Weeds", seed.Name, 1),
                                        new Item("Purple Flower Seeds", seed.Name, 1),
                                        new Item("Red Flower Seeds", seed.Name, 1),
                                        new Item("Violet", seed.Name, 1),
                                        new Item("Lavender", seed.Name, 1)
                                    });
                                    break;
                                case "2":
                                    tier.BottomRow.AddRange(new List<string>()
                                    {
                                        "Violet",
                                        "Rose",
                                        "Lavender",
                                        "Forget-me-nots",
                                        "Yellow Flower Seeds"
                                    }.Select(x => new Item(x, seed.Name)));
                                    tier.TopRow.AddRange(new List<string>()
                                    {
                                        "Pitcher Plant",
                                        "Baby's Breath",
                                        "Red Flower Seeds",
                                        "Pale-Blue Flower Seeds",
                                        "Green Flower Seeds"
                                    }.Select(x => new Item(x, seed.Name)));
                                    break;
                                case "3":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Pitcher Plant"),
                                        new Item("Cabbage"),
                                        new Item("Green Flower Seeds"),
                                        new Item("Mixed Herb Seeds"),
                                        new Item("Violet")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Lavender"),
                                        new Item("Western Fodlan Seeds"),
                                        new Item("Morfis Seeds"),
                                        new Item("Eastern Fodlan Seeds"),
                                        new Item("Albinean Berries")

                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    break;
                            }
                            break;
                        case "Yellow Flower Seeds":
                            switch (tier.Tier)
                            {
                                case "1":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Weeds", seed.Name, 3),
                                        new Item("Sunflower", seed.Name, 1),
                                        new Item("Yellow Flower Seeds", seed.Name, 1)
                                    });
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Weeds", seed.Name, 1),
                                        new Item("Yellow Flower Seeds", seed.Name, 1),
                                        new Item("White Flower Seeds", seed.Name, 1),
                                        new Item("Sunflower", seed.Name, 2)
                                    });
                                    break;
                                case "2":
                                    tier.BottomRow.AddRange(new List<string>()
                                    {
                                        "Sunflower",
                                        "Anemone",
                                        "Daffodil",
                                        "Lily",
                                        "Green Flower Seeds"
                                    }.Select(x => new Item(x, seed.Name)));
                                    tier.TopRow.AddRange(new List<string>()
                                    {
                                        "Carnation",
                                        "Rose",
                                        "White Flower Seeds",
                                        "Red Flower Seeds",
                                        "Pale-Blue Flower Seeds"
                                    }.Select(x => new Item(x, seed.Name)));
                                    break;
                                case "3":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Carnation"),
                                        new Item("Morfis Seeds"),
                                        new Item("Pale-Blue Flower Seeds"),
                                        new Item("Onion"),
                                        new Item("Sunflower")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Sunflower"),
                                        new Item("Northern Fodlan Seeds"),
                                        new Item("Southern Fodlan Seeds"),
                                        new Item("Boa-Fruit Seeds"),
                                        new Item("Mixed Fruit Seeds")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    break;
                            }
                            break;
                        case "Green Flower Seeds":
                            switch (tier.Tier)
                            {
                                case "1":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Weeds", seed.Name, 3),
                                        new Item("Pitcher Plant", seed.Name, 1),
                                        new Item("Green Flower Seeds", seed.Name, 1)
                                    });
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Weeds", seed.Name, 1),
                                        new Item("Green Flower Seeds", seed.Name, 1),
                                        new Item("Blue Flower Seeds", seed.Name, 1),
                                        new Item("Pitcher Plant", seed.Name, 2)
                                    });
                                    break;
                                case "2":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Pitcher Plant", 2),
                                        new Item("Rose", 2),
                                        new Item("Pale-Blue Flower Seeds")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<string>()
                                    {
                                        "Lily",
                                        "Violet",
                                        "Blue Flower Seeds",
                                        "White Flower Seeds",
                                        "Red Flower Seeds"
                                    }.Select(x => new Item(x, seed.Name)));
                                    break;
                                case "3":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Lily"),
                                        new Item("Zanado Fruit"),
                                        new Item("Red Flower Seeds"),
                                        new Item("Cabbage"),
                                        new Item("Pitcher Plant")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Pitcher Plant"),
                                        new Item("Root Vegetable Seeds"),
                                        new Item("Eastern Fodlan Seeds"),
                                        new Item("Tomato"),
                                        new Item("Morfis Plum")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    break;
                            }
                            break;
                        case "Pale-Blue Flower Seeds":
                            switch (tier.Tier)
                            {
                                case "1":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Weeds", seed.Name, 3),
                                        new Item("Carnation", seed.Name, 1),
                                        new Item("Pale-Blue Flower Seeds", seed.Name, 1)
                                    });
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Weeds", seed.Name, 1),
                                        new Item("Pale-Blue Flower Seeds", seed.Name, 1),
                                        new Item("Purple Flower Seeds", seed.Name, 1),
                                        new Item("Carnation", seed.Name, 2)
                                    });
                                    break;
                                case "2":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Anemone",2),
                                        new Item("Forget-me-nots"),
                                        new Item("Rose"),
                                        new Item("Red Flower Seeds")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Lily of the Valley"),
                                        new Item("Pitcher Plant"),
                                        new Item("Purple Flower Seeds"),
                                        new Item("Blue Flower Seeds"),
                                        new Item("White Flower Seeds")
                                    }.Select(x => new Item(x.Name, seed.Name)));
                                    break;
                                case "3":
                                    tier.BottomRow.AddRange(new List<Item>()
                                    {
                                        new Item("Lily of the Valley"),
                                        new Item("Northern Fodlan Seeds"),
                                        new Item("White Flower Seeds"),
                                        new Item("Turnip"),
                                        new Item("Anemone")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    tier.TopRow.AddRange(new List<Item>()
                                    {
                                        new Item("Forget-me-nots"),
                                        new Item("Mixed Fruit Seeds"),
                                        new Item("Northern Fodlan Seeds"),
                                        new Item("Tomato"),
                                        new Item("Verona")
                                    }.Select(x => new Item(x.Name, seed.Name, x.Count)));
                                    break;
                            }
                            break;
                    }
                }
            
                
            }

            YieldTiers.AddRange(tierList);

        }        
    }
}
