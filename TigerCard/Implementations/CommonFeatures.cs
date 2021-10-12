using System;
using System.Collections.Generic;
using System.Linq;
using TigerCard.Interfaces;
using TigerCard.Models;

namespace TigerCard.Implementations
{
    public class CommonFeatures : ICommonFeatures
    {

        private int weekwiseTotalFare;

        private FarthestDayWiseDistanceZones farthestZonesInADay;
        private FarthestWeekWiseDistance farthestZonesInAWeek;
        private readonly IRulesReader rulesReader;

        public int CurrentWeek { get; set; }
        public int DaywiseTotalFare { get; set; } 
        public bool IsnewWeek { get; private set; }
        public Rules FareCalculationRules { get; set; }

        public List<FarthestDayWiseDistanceZones> FarthestZonesDayWise { get; set; } = new List<FarthestDayWiseDistanceZones>();
        public List<FarthestWeekWiseDistance> FarthestZonesWeekWise { get; set; } = new List<FarthestWeekWiseDistance>();

        public void CheckIfWeekChanged(int week)
        {
            try
            {
                IsnewWeek = CurrentWeek != week;
                CurrentWeek = week;
            }
            catch
            {

                throw new Exception("Unable to check if week got changed");
            }

        }

        public CommonFeatures(IRulesReader rulesReader)
        {
            this.rulesReader = rulesReader;
            FareCalculationRules = this.rulesReader.PopulatesRules();
        }



        public int GetSpecificDayCappedFare(int week, string day, List<FarthestDayWiseDistanceZones> farthestZonesDayWise)
        {
            try
            {
                var farthestzonesTravelled = farthestZonesDayWise.FirstOrDefault(s => s.Day.Equals(day) && s.Week == week);
                var dailycappedfare = GetZoneWiseDailyCappedFare(farthestzonesTravelled.FromZone, farthestzonesTravelled.ToZone);
                return dailycappedfare;
            }
            catch
            {
                throw new Exception("Unable to get capped fare for a specific day");
            }
        }

        public int GetZoneWiseDailyCappedFare(int fromZone, int toZone)
        {
            try
            {
                var dailycappedfare = FareCalculationRules.Capings.Where(s => s.FromZone == fromZone && s.ToZone == toZone).Select(s => s.DailyCap).FirstOrDefault();
                return dailycappedfare;
            }
            catch
            {
                throw new Exception("Unable to get capped daily fare");
            }
        }

        public int GetZoneWiseWeeklyCappedFare(int fromZone, int toZone)
        {
            var weeklycappedfare = FareCalculationRules.Capings.Where(s => s.FromZone == fromZone && s.ToZone == toZone).Select(s => s.WeeklyCap).FirstOrDefault();
            return weeklycappedfare;
        }

        public bool CheckIfPeakHour(string day, int time)
        {
            try
            {

                var peakHours = FareCalculationRules.PeakTimings.Where(s => s.Day.Equals(day, StringComparison.InvariantCultureIgnoreCase));
                foreach (var peakHour in peakHours)
                {
                    if (time >= peakHour.From && time <= peakHour.To)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                throw new Exception("Unable to identify Peak Hour");

            }
        }

        public int GetFare(int fromZone, int toZone, bool isPeakHour)
        {
            try
            {
                int? fare = isPeakHour ? FareCalculationRules.Fares.Where(s => s.FromZone == fromZone && s.ToZone == toZone).Select(s => s.PeakHoursFare).FirstOrDefault() : FareCalculationRules.Fares.Where(s => s.FromZone == fromZone && s.ToZone == toZone).Select(s => s.OffPeakHourFare).FirstOrDefault();
                return (int) fare;
            }
            catch
            {
                throw new Exception("Unable to Calculate Fare");
            }
        }

        public int CalculateDayWiseFare(int dailyCappedFare, int journeyFare)
        {
            try
            {
                DaywiseTotalFare = DaywiseTotalFare + journeyFare;
                if (DaywiseTotalFare > dailyCappedFare)
                {
                    DaywiseTotalFare = dailyCappedFare;
                }
                return DaywiseTotalFare;
            }
            catch
            {
                throw new Exception("Unable to Calculate day wise fare");
            }
        }



        public int CalculateWeeklyFare(int weeklyCappedFare, int daywiseFare)
        {
            try
            {
                if (IsnewWeek)
                {
                    weekwiseTotalFare = 0;
                }
                weekwiseTotalFare = weekwiseTotalFare + daywiseFare;
                if (weekwiseTotalFare > weeklyCappedFare)
                {
                    weekwiseTotalFare = weeklyCappedFare;
                }
                return weekwiseTotalFare;
            }
            catch
            {
                throw new Exception("Unable to Calculate weekly fare");
            }
        }

        public List<FarthestDayWiseDistanceZones> GetFarthestZonesTravelledInADay(List<Journey> journeys)
        {
            try
            {
                foreach (var day in journeys.GroupBy(s => new { s.Week, s.Day }).Select(s => new { Day = s.Key, journeydetails = s.ToList() }))
                {
                    int farthestDistanceInADay = 0;

                    foreach (var journey in day.journeydetails)
                    {
                        var distance = Math.Abs(journey.ToZone - journey.FromZone);
                        if (distance >= farthestDistanceInADay)
                        {
                            farthestDistanceInADay = distance;
                            farthestZonesInADay = new FarthestDayWiseDistanceZones() { Week = journey.Week, Day = journey.Day, ToZone = journey.ToZone, FromZone = journey.FromZone };
                        }
                    }

                    FarthestZonesDayWise.Add(farthestZonesInADay);
                }
                return FarthestZonesDayWise;
            }
            catch
            {
                throw new Exception("Unable to get farthest zone travelled in a day");
            }
        }

        public List<FarthestWeekWiseDistance> GetFarthestZonesTravelledInAWeek(List<Journey> journeys)
        {
            try
            {

                foreach (var week in journeys.GroupBy(s => s.Week).Select(s => new { Week = s.Key, journeydetails = s.ToList() }))
                {
                    int farthestDistanceInAWeek = 0;

                    foreach (var journey in week.journeydetails)
                    {
                        var distance = Math.Abs(journey.ToZone - journey.FromZone);
                        if (distance >= farthestDistanceInAWeek)
                        {
                            farthestDistanceInAWeek = distance;
                            farthestZonesInAWeek = new FarthestWeekWiseDistance() { Week = journey.Week, ToZone = journey.ToZone, FromZone = journey.FromZone };

                        }
                    }
                    FarthestZonesWeekWise.Add(farthestZonesInAWeek);
                }

                return FarthestZonesWeekWise;
            }
            catch
            {
                throw new Exception("Unable to get farthest zone travelled in a week");
            }
        }
    }
}
