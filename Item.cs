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
        public string Seed;
        public string Row;
        public int Count;

        public Item(string _name, string _seed, int _count)
        {
            this.Name = _name;
            this.Seed = _seed;
            this.Count = _count;
        }

        public Item(string _name, string _seed)
        {
            this.Name = _name;
            this.Seed = _seed;
            this.Count = 1;
        }

        public Item(string _name)
        {
            this.Name = _name;
            this.Count = 1;
        }
        public Item (string _name, int _count)
        {
            this.Name = _name;
            this.Count = _count;
        }
    }
}
