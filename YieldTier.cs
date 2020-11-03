using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenhouseCalc
{
    public class YieldTier
    {
        public string Tier;
        public List<Item> TopRow;
        public List<Item> BottomRow;
        public string TopRatio;
        public string BottomRatio;
        public int UpperStatBoosterCoefficient;
        public int LowerStatBoosterCoefficient;
        public int ScoreRangeLowerMin;
        public int ScoreRangeLowerMax;
        public int ScoreRangeUpperMin;
        public int ScoreRangeUpperMax;

        public YieldTier(string _tier, string _topRatio, string _bottomRatio, 
            int _sbCoeffUp, int _sbCoeffLow, int _scLowerMin, int _scLowerMax, int _scUpperMin, int _scUpperMax)

        {
            this.Tier = _tier;
            this.TopRatio = _topRatio;
            this.BottomRatio = _bottomRatio;
            this.UpperStatBoosterCoefficient = _sbCoeffUp;
            this.LowerStatBoosterCoefficient = _sbCoeffLow;
            this.ScoreRangeLowerMin = _scLowerMin;
            this.ScoreRangeLowerMax = _scLowerMax;
            this.ScoreRangeUpperMin = _scUpperMin;
            this.ScoreRangeUpperMax = _scUpperMax;
            this.TopRow = new List<Item>();
            this.BottomRow = new List<Item>();

        }

    }
}
