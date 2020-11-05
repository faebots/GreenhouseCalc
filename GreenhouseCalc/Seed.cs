using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenhouseCalc
{
    public class Seed
    {
        public string Name { get; set; }
        public int Rank { get; set; }
        public int Grade { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is string)
                return (string)obj == Name;
            else if (obj is Seed)
            {
                var obj2 = obj as Seed;
                return obj2.Name == Name;
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(Seed x, Seed y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Seed x, Seed y)
        {
            return !(x == y);
        }
    }
}
