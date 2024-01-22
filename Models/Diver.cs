using NauticalCatchChallenge.Models.Contracts;
using NauticalCatchChallenge.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NauticalCatchChallenge.Models
{
    public abstract class Diver : IDiver
    {
        private string name;
        private int oxygenLevel;
        private readonly List<string> catches;

        protected Diver(string name, int oxygenLevel)
        {
            Name = name;
            OxygenLevel = oxygenLevel;
            catches = new List<string>();
            CompetitionPoints = 0;
            HasHealthIssues = false;
        }

        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.DiversNameNull);
                }
                name = value;
            }
        }

        public int OxygenLevel
        {
            get => oxygenLevel;
            protected set
            {
                if (value <= 0)
                {
                    oxygenLevel = 0;
                    HasHealthIssues = true;
                }
                else
                {
                    oxygenLevel = value;
                }
            }
        }

        public IReadOnlyCollection<string> Catch => catches.AsReadOnly();

        public double CompetitionPoints { get; private set; }

        public bool HasHealthIssues { get; private set; }

        public void Hit(IFish fish)
        {
            OxygenLevel -= fish.TimeToCatch;
            catches.Add(fish.Name);
            CompetitionPoints += fish.Points;
        }

        public abstract void Miss(int TimeToCatch);
        public abstract void RenewOxy();

        public void UpdateHealthStatus()
        {
            HasHealthIssues = !HasHealthIssues;
        }

        public override string ToString()
        {
            return $"Diver [ Name: {Name}, Oxygen left: {OxygenLevel}, Fish caught: {catches.Count}, Points earned: {Math.Round(CompetitionPoints,1)} ]";
        }
    }
}
