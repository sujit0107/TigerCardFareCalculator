using System.Collections.Generic;
using TigerCard.Models;

namespace TigerCard.Interfaces
{
    public interface ICommonFeatures
    {
        int DayWiseTotalFare { get; set; }
        bool IsNewWeek { get; }

        int CalculateDayWiseFare(int dailyCappedFare, int journeyFare);
        int CalculateWeeklyFare(int weeklyCappedFare, int dailyFare);
        bool CheckIfPeakHour(string day, int time);
        void CheckIfWeekChanged(int week);
        int GetFare(int fromZone, int toZone, bool isPeakHour);
        List<FarthestDayWiseDistanceZones> GetFarthestZonesTraveledInADay(List<Journey> journeys);
        List<FarthestWeekWiseDistance> GetFarthestZonesTraveledInAWeek(List<Journey> journeys);
        int GetSpecificDayCappedFare(int week, string day, List<FarthestDayWiseDistanceZones> farthestZonesDayWise);
        int GetZoneWiseWeeklyCappedFare(int fromZone, int toZone);
    }
}