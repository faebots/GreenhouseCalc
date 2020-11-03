using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenhouseCalc
{
    public class Seed
    {
        public Seed(string name, int rank, int grade)
        {
            this.Name = name;
            this.Rank = rank;
            this.Grade = grade;
        }
        public string Name { get; set; }
        public int Rank { get; set; }
        public int Grade { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            Seed seed = (Seed)obj;
            return (seed.Name == this.Name);
        }
    }
}
