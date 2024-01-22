using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NauticalCatchChallenge.Models
{
    public class ScubaDiver : Diver
    {
        private const int ScubaDiverOxygenLevel = 540;
        public ScubaDiver(string name) 
            : base(name, ScubaDiverOxygenLevel)
        {
        }

        public override void Miss(int TimeToCatch)
        {
            OxygenLevel -= (int)Math.Round(0.3 * TimeToCatch, MidpointRounding.AwayFromZero);
        }

        public override void RenewOxy()
        {
            OxygenLevel = ScubaDiverOxygenLevel;
        }
    }
}
