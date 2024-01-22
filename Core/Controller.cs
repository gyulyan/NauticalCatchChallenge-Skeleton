using NauticalCatchChallenge.Core.Contracts;
using NauticalCatchChallenge.Models;
using NauticalCatchChallenge.Models.Contracts;
using NauticalCatchChallenge.Repositories;
using NauticalCatchChallenge.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NauticalCatchChallenge.Core
{
    public class Controller : IController
    {
        private FishRepository fish;
        private DiverRepository divers;
        public Controller()
        {
            fish = new FishRepository();
            divers = new DiverRepository();
        }

        public string ChaseFish(string diverName, string fishName, bool isLucky)
        {
            if (divers.GetModel(diverName) == null)
            {
                return string.Format(OutputMessages.DiverNotFound, divers.GetType().Name, diverName);
            }

            if (fish.GetModel(fishName) == null)
            {
                return string.Format(OutputMessages.FishNotAllowed, fishName);
            }

            var diver = divers.GetModel(diverName);
            var fishToChatch = fish.GetModel(fishName);

            if (diver.HasHealthIssues == true)
            {
                return string.Format(OutputMessages.DiverHealthCheck, diverName);
            }

            if (diver.OxygenLevel < fishToChatch.TimeToCatch)
            {
                diver.Miss(fishToChatch.TimeToCatch);

                return string.Format(OutputMessages.DiverMisses, diverName, fishName);
            }
            else if (diver.OxygenLevel == fishToChatch.TimeToCatch)
            {
                if (isLucky == true)
                {
                    diver.Hit(fishToChatch);

                    return string.Format(OutputMessages.DiverHitsFish, diverName, fishToChatch.Points, fishName);
                }
                else
                {
                    diver.Miss(fishToChatch.TimeToCatch);

                    return string.Format(OutputMessages.DiverMisses, diverName, fishName);
                }
            }
            else
            {
                diver.Hit(fishToChatch);

                return string.Format(OutputMessages.DiverHitsFish, diverName, fishToChatch.Points, fishName);
            }

        }

        public string CompetitionStatistics()
        {
            var orderedDivers = divers.Models
                .Where(x => x.OxygenLevel > 0)
                 .OrderByDescending(x => x.CompetitionPoints)
                 .ThenByDescending(x => x.Catch.Count)
                 .ThenBy(x => x.Name)
                 .ThenBy(x => x.HasHealthIssues == false);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("**Nautical-Catch-Challenge**");

            foreach (var diver in orderedDivers)
            {
                sb.AppendLine(diver.ToString());
            }

            return sb.ToString().TrimEnd();
        }

        public string DiveIntoCompetition(string diverType, string diverName)
        {
            IDiver diver = null;
            if (diverType != "FreeDiver" && diverType != "ScubaDiver")
            {
                return string.Format(OutputMessages.DiverTypeNotPresented, diverType);
            }

            if (divers.GetModel(diverName) != null)
            {
                return string.Format(OutputMessages.DiverNameDuplication, diverName, divers.GetType().Name);
            }

            if (diverType == "FreeDiver")
            {
                diver = new FreeDiver(diverName);
            }
            else if (diverType == "ScubaDiver")
            {
                diver = new ScubaDiver(diverName);
            }

            divers.AddModel(diver);
            return string.Format(OutputMessages.DiverRegistered, diverName, divers.GetType().Name);

        }

        public string DiverCatchReport(string diverName)
        {
            var diver = divers.GetModel(diverName);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(diver.ToString());
            sb.AppendLine("Catch Report:");

            foreach (string fishName in diver.Catch)
            {
                var fishToAdd = this.fish.GetModel(fishName);
                sb.AppendLine(fishToAdd.ToString());
            }

            return sb.ToString().TrimEnd();
        }

        public string HealthRecovery()
        {
            int count = 0;

            foreach (var diver in divers.Models)
            {
                if (diver.HasHealthIssues == true)
                {
                    diver.UpdateHealthStatus();
                    diver.RenewOxy();
                    count++;
                }
            }

            return string.Format(OutputMessages.DiversRecovered, count);
        }

        public string SwimIntoCompetition(string fishType, string fishName, double points)
        {
            IFish newFish = null;

            if (fishType != "ReefFish" && fishType != "DeepSeaFish" && fishType != "PredatoryFish")
            {
                return string.Format(OutputMessages.FishTypeNotPresented, fishType);
            }

            if (fish.GetModel(fishName) != null)
            {
                return string.Format(OutputMessages.FishNameDuplication, fishName, fish.GetType().Name);
            }

            if (fishType == "ReefFish")
            {
                newFish = new ReefFish(fishName, points);
            }
            else if (fishType == "DeepSeaFish")
            {
                newFish = new DeepSeaFish(fishName, points);
            }
            else if (fishType == "PredatoryFish")
            {
                newFish = new PredatoryFish(fishName, points);
            }

            fish.AddModel(newFish);
            return string.Format(OutputMessages.FishCreated, fishName);
        }
    }
}
