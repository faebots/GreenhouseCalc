using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenhouseCalc
{
    public class YieldTier
    {
        public decimal Tier;
        public (int Lower, int Upper) ScoreRange;
        public decimal Ratio;
        public int StatCoefficient;
    }
}
