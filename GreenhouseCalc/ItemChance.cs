using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenhouseCalc
{
    public class ItemSet
    {
        public string Seed;
        public List<ItemChance> Items;
        public decimal Probability;
        public ItemSet(string _seed, List<ItemChance> _items, decimal _probability)
        {
            Seed = _seed;
            Items = _items;
            Probability = _probability;
        }
    }
    public class ItemChance
    {
        public string Name;
        public decimal Probability;
        public ItemChance(string _name, decimal _probability)
        {
            Name = _name;
            Probability = _probability;
        }
    }
}
