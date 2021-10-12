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
        private int dayfare;
        public List<FarthestDayWiseDistanceZones> FarthestZonesDayWise { get; set; }
        public List<FarthestWeekWiseDistance> FarthestZonesWeekWise { get; set; }

        private readonly ICommonFeatures commonfeatures;
        public WeeklyCappedFareCalculator(ICommonFeatures commonFeatures)
        {
            commonfeatures = commonFeatures;
        }
        public int CalculateFare(List<Journey> journeys)
        {
            try
            {
                int totalfare = 0;

                FarthestZonesDayWise = commonfeatures.GetFarthestZonesTravelledInADay(journeys);

                FarthestZonesWeekWise = commonfeatures.GetFarthestZonesTravelledInAWeek(journeys);

                foreach (var journey in journeys.GroupBy(s => new { s.Week, s.Day }).Select(s => new {s.Key.Week, s.Key.Day, journeydetails = s.ToList() }))
                {
                    commonfeatures.CheckIfWeekChanged(journey.Week);
                    commonfeatures.DaywiseTotalFare = 0;

                    var dailyCappedFare = commonfeatures.GetSpecificDayCappedFare(journey.Week, journey.Day, FarthestZonesDayWise);
                    var weeklyCappedFare = commonfeatures.GetZoneWiseWeeklyCappedFare(FarthestZonesWeekWise.FirstOrDefault(s => s.Week == journey.Week).FromZone, FarthestZonesWeekWise.FirstOrDefault(s => s.Week == journey.Week).ToZone);

                    foreach (var detail in journey.journeydetails)
                    {

                        isPeakHour = commonfeatures.CheckIfPeakHour(detail.Day, detail.Time);
                        var journeyFare = commonfeatures.GetFare(detail.FromZone, detail.ToZone, isPeakHour);
                        dayfare = commonfeatures.CalculateDayWiseFare(dailyCappedFare, journeyFare);
                    }

                    var totalweeklyfare = commonfeatures.CalculateWeeklyFare(weeklyCappedFare, dayfare);
                    if (!commonfeatures.IsnewWeek)
                    {
                        totalfare = totalweeklyfare;
                    }
                    else
                        totalfare = totalfare + totalweeklyfare;

                }

                return totalfare;
            }
            catch
            {
                throw new Exception("Unable to calculate fare");
            }
        }


    }
}