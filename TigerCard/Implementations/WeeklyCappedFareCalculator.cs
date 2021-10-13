using System;
using System.Collections.Generic;
using System.Linq;
using TigerCard.Interfaces;
using TigerCard.Models;

namespace TigerCard.Implementations
{
    public class WeeklyCappedFareCalculator : IFareCalculator
    {
        private bool isPeakHour;
        private int dayFare;
        public List<FarthestDayWiseDistanceZones> FarthestZonesDayWise { get; set; }
        public List<FarthestWeekWiseDistance> FarthestZonesWeekWise { get; set; }

        private readonly ICommonFeatures commonFeatures;
        public WeeklyCappedFareCalculator(ICommonFeatures commonFeatures)
        {
            this.commonFeatures = commonFeatures;
        }
        public int CalculateFare(List<Journey> journeys)
        {
            try
            {
                int totalFare = 0;

                FarthestZonesDayWise = commonFeatures.GetFarthestZonesTraveledInADay(journeys);

                FarthestZonesWeekWise = commonFeatures.GetFarthestZonesTraveledInAWeek(journeys);

                foreach (var journey in journeys.GroupBy(s => new { s.Week, s.Day })
                    .Select(s => new { s.Key.Week, s.Key.Day, journeydetails = s.ToList() }))
                {
                    commonFeatures.CheckIfWeekChanged(journey.Week);
                    commonFeatures.DayWiseTotalFare = 0;

                    var dailyCappedFare = commonFeatures.GetSpecificDayCappedFare(journey.Week, journey.Day, FarthestZonesDayWise);

                    var farthestFromZone = FarthestZonesWeekWise.FirstOrDefault(s => s.Week == journey.Week)?.FromZone ?? 0;
                    var farthestToZone = FarthestZonesWeekWise.FirstOrDefault(s => s.Week == journey.Week)?.ToZone ?? 0;

                    var weeklyCappedFare = commonFeatures.GetZoneWiseWeeklyCappedFare(farthestFromZone, farthestToZone);

                    foreach (var detail in journey.journeydetails)
                    {
                        isPeakHour = commonFeatures.CheckIfPeakHour(detail.Day, detail.Time);
                        var journeyFare = commonFeatures.GetFare(detail.FromZone, detail.ToZone, isPeakHour);
                        dayFare = commonFeatures.CalculateDayWiseFare(dailyCappedFare, journeyFare);
                    }

                    var totalWeeklyFare = commonFeatures.CalculateWeeklyFare(weeklyCappedFare, dayFare);

                    if (!commonFeatures.IsNewWeek)
                    {
                        totalFare = totalWeeklyFare;
                    }
                    else
                        totalFare = totalFare + totalWeeklyFare;

                }

                return totalFare;
            }
            catch
            {
                throw new Exception("Unable to calculate fare");
            }
        }


    }
}