using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NauticalCatchChallenge.Models
{
    public class FreeDiver : Diver
    {
        private const int FreeDiverOxygenLevel = 120;
        public FreeDiver(string name) 
            : base(name, FreeDiverOxygenLevel)
        {
           
        }

        public override void Miss(int TimeToCatch)
        {
            OxygenLevel -= (int)Math.Round(0.6*TimeToCatch, MidpointRounding.AwayFromZero);
        }

        public override void RenewOxy()
        {
            OxygenLevel = FreeDiverOxygenLevel;
        }
    }
}
