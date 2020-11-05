using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenhouseCalc
{
    public class Item
    {
        public string Name;
        public List<SeedTier> Seeds;
    }

    public class SeedTier {
        public string Name;
        public List<TierCount> Tiers;

    }

    public class TierCount {
        public decimal Tier;
        public int Count;
    }
}
