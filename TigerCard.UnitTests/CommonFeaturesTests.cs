using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TigerCard.Implementations;
using TigerCard.Models;

namespace TigerCard.UnitTests
{
    [TestClass]
    public class CommonFeaturesTests
    {
        public List<Journey> Inputs { get; set; }
        public CommonFeatures CommonFeaturesObj;
        readonly XmlFileRulesReader rulerReaderobj = new XmlFileRulesReader();

        public CommonFeaturesTests()
        {

            Inputs = new List<Journey>();
            var input1 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 2, ToZone = 1, Time = 1020 };
            var input7 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 1, Time = 1100 };
            var input2 = new Journey() { Date = Convert.ToDateTime("05/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input3 = new Journey() { Date = Convert.ToDateTime("06/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
            var input4 = new Journey() { Date = Convert.ToDateTime("07/10/2021"), FromZone = 1, ToZone = 1, Time = 1815 };
            var input5 = new Journey() { Date = Convert.ToDateTime("08/10/2021"), FromZone = 1, ToZone = 2, Time = 1900 };
            var input6 = new Journey() { Date = Convert.ToDateTime("11/10/2021"), FromZone = 1, ToZone = 1, Time = 1900 };

            Inputs.Add(input1);
            Inputs.Add(input7);
            Inputs.Add(input2);
            Inputs.Add(input3);
            Inputs.Add(input4);
            Inputs.Add(input5);
            Inputs.Add(input6);

            CommonFeaturesObj = new CommonFeatures(rulerReaderobj);
        }



        [TestMethod]
        public void GetFarthestZoneDayWiseTest()
        {
            var farthestZones = CommonFeaturesObj.GetFarthestZonesTravelledInADay(Inputs);
            Assert.AreEqual(6, farthestZones.Count);
        }

        [TestMethod]
        public void GetFarthestZoneDayWiseForSpecificDayTest()
        {
            var farthestZones = CommonFeaturesObj.GetFarthestZonesTravelledInADay(Inputs);
            var farthestZoneOnModay = farthestZones.Where(s => s.Day.Equals("Monday", StringComparison.InvariantCultureIgnoreCase) && s.Week == 41).FirstOrDefault();

            Assert.AreEqual("Monday", farthestZoneOnModay.Day);
            Assert.AreEqual(2, farthestZoneOnModay.FromZone);
            Assert.AreEqual(1, farthestZoneOnModay.ToZone);
        }


        [TestMethod]
        public void GetFarthestZoneWeekWiseTest()
        {
            var farthestZones = CommonFeaturesObj.GetFarthestZonesTravelledInAWeek(Inputs);
            Assert.AreEqual(1, farthestZones.FirstOrDefault().FromZone);
            Assert.AreEqual(2, farthestZones.FirstOrDefault().ToZone);
        }


        [TestMethod]
        public void GetZoneWiseDailyCappedFareTest()
        {
            var dailycappedfare = CommonFeaturesObj.GetZoneWiseDailyCappedFare(1, 2);
            Assert.AreEqual(120, dailycappedfare);

        }

        [TestMethod]
        public void GetZoneWiseWeeklyCappedFareTest()
        {
            var dailycappedfare = CommonFeaturesObj.GetZoneWiseWeeklyCappedFare(1, 2);
            Assert.AreEqual(600, dailycappedfare);

        }
        [TestMethod]
        public void GetSpecificDayCappedFareTest()
        {
            var farthestZonesDayWise = CommonFeaturesObj.GetFarthestZonesTravelledInADay(Inputs);
            var dailycappedfare = CommonFeaturesObj.GetSpecificDayCappedFare(41, "Monday", farthestZonesDayWise);
            Assert.AreEqual(120, dailycappedfare);

        }


        [TestMethod]
        public void CheckIfPeakHourNegativeTest()
        {
            int time = 1200;
            string day = "Monday";
            var isPeakHour = CommonFeaturesObj.CheckIfPeakHour(day, time);
            Assert.AreEqual(false, isPeakHour);
        }

        [TestMethod]
        public void CheckIfPeakHourPositiveTest()
        {
            int time = 1028;
            string day = "Monday";
            var isPeakHour = CommonFeaturesObj.CheckIfPeakHour(day, time);
            Assert.AreEqual(true, isPeakHour);
        }

        [TestMethod]
        public void GetFareForPeakHourTest()
        {

            int FromZone = 1;
            int ToZone = 2;
            var fare = CommonFeaturesObj.GetFare(FromZone, ToZone, true);
            Assert.AreEqual(35, fare);
        }

        [TestMethod]
        public void GetFareForNonPeakHourTest()
        {

            int FromZone = 1;
            int ToZone = 1;
            var fare = CommonFeaturesObj.GetFare(FromZone, ToZone, false);
            Assert.AreEqual(25, fare);
        }
    }
}
