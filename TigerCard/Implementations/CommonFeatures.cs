using System;
using System.Collections.Generic;
using System.Linq;
using TigerCard.Interfaces;
using TigerCard.Models;

namespace TigerCard.Implementations
{
    public class CommonFeatures : ICommonFeatures
    {

        private int weekWiseTotalFare;

        private FarthestDayWiseDistanceZones farthestZonesInADay;
        private FarthestWeekWiseDistance farthestZonesInAWeek;
        private readonly IRulesReader rulesReader;

        public int CurrentWeek { get; set; }
        public int DayWiseTotalFare { get; set; } 
        public bool IsNewWeek { get; private set; }
        public Rules FareCalculationRules { get; set; }

        public List<FarthestDayWiseDistanceZones> FarthestZonesDayWise { get; set; } = new List<FarthestDayWiseDistanceZones>();
        public List<FarthestWeekWiseDistance> FarthestZonesWeekWise { get; set; } = new List<FarthestWeekWiseDistance>();

        public void CheckIfWeekChanged(int week)
        {
            try
            {
                IsNewWeek = CurrentWeek != week;
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
            FareCalculationRules = this.rulesReader.PopulateRules();
        }



        public int GetSpecificDayCappedFare(int week, string day, List<FarthestDayWiseDistanceZones> farthestZonesDayWise)
        {
            try
            {
                var farthestZonesTraveled = farthestZonesDayWise.FirstOrDefault(s => s.Day.Equals(day) && s.Week == week);
                if (farthestZonesTraveled != null)
                {
                    var dailyCappedFare = GetZoneWiseDailyCappedFare(farthestZonesTraveled.FromZone, farthestZonesTraveled.ToZone);
                    return dailyCappedFare;
                }

                return 0;
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
                var dailyCappedFare = FareCalculationRules.Capings.Where(s => s.FromZone == fromZone && s.ToZone == toZone).Select(s => s.DailyCap).FirstOrDefault();
                return dailyCappedFare;
            }
            catch
            {
                throw new Exception("Unable to get capped daily fare");
            }
        }

        public int GetZoneWiseWeeklyCappedFare(int fromZone, int toZone)
        {
            var weeklyCappedFare = FareCalculationRules.Capings.Where(s => s.FromZone == fromZone && s.ToZone == toZone).Select(s => s.WeeklyCap).FirstOrDefault();
            return weeklyCappedFare;
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
                DayWiseTotalFare = DayWiseTotalFare + journeyFare;
                if (DayWiseTotalFare > dailyCappedFare)
                {
                    DayWiseTotalFare = dailyCappedFare;
                }
                return DayWiseTotalFare;
            }
            catch
            {
                throw new Exception("Unable to Calculate day wise fare");
            }
        }



        public int CalculateWeeklyFare(int weeklyCappedFare, int dayWiseFare)
        {
            try
            {
                if (IsNewWeek)
                {
                    weekWiseTotalFare = 0;
                }
                weekWiseTotalFare = weekWiseTotalFare + dayWiseFare;
                if (weekWiseTotalFare > weeklyCappedFare)
                {
                    weekWiseTotalFare = weeklyCappedFare;
                }
                return weekWiseTotalFare;
            }
            catch
            {
                throw new Exception("Unable to Calculate weekly fare");
            }
        }

        public List<FarthestDayWiseDistanceZones> GetFarthestZonesTraveledInADay(List<Journey> journeys)
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
                throw new Exception("Unable to get farthest zone traveled in a day");
            }
        }

        public List<FarthestWeekWiseDistance> GetFarthestZonesTraveledInAWeek(List<Journey> journeys)
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
                throw new Exception("Unable to get farthest zone traveled in a week");
            }
        }
    }
}
