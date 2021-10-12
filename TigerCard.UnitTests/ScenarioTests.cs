using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TigerCard.Implementations;
using TigerCard.Models;

namespace TigerCard.UnitTests
{
    [TestClass]
    public class ScenarioTests
    {
        public List<Journey> Inputs { get; set; } = new List<Journey>();
        public CommonFeatures CommonFeaturesObj;
        readonly WeeklyCappedFareCalculator calculatefareObj;
        readonly XmlFileRulesReader rulerReaderobj = new XmlFileRulesReader();

        public ScenarioTests()
        {

            CommonFeaturesObj = new CommonFeatures(rulerReaderobj);
            calculatefareObj = new WeeklyCappedFareCalculator(CommonFeaturesObj);

        }

        [TestMethod]
        public void InvalidDataTest()
        {
            Inputs = new List<Journey>();
            var input1 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 2, ToZone = 3, Time = 1020 };
            var input2 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 4, Time = 1045 };
            var input3 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 5, ToZone = 1, Time = 1615 };
            var input4 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 1, Time = 5000 };
            var input5 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 2, Time = 1900 };

            Inputs.Add(input1);
            Inputs.Add(input2);
            Inputs.Add(input3);
            Inputs.Add(input4);
            Inputs.Add(input5);


            var fare = calculatefareObj.CalculateFare(Inputs);
            Assert.AreEqual(0, fare);
        }

        [TestMethod]
        public void SameDayJourneysCappingReachedTest()
        {
            Inputs = new List<Journey>();
            var input1 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 2, ToZone = 1, Time = 1020 };
            var input2 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input3 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
            var input4 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 1, Time = 1815 };
            var input5 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 2, Time = 1900 };

            Inputs.Add(input1);
            Inputs.Add(input2);
            Inputs.Add(input3);
            Inputs.Add(input4);
            Inputs.Add(input5);


            var fare = calculatefareObj.CalculateFare(Inputs);
            Assert.AreEqual(120, fare);
        }

        [TestMethod]
        public void SameDayJourneysCappingNotReachedTest()
        {
            Inputs = new List<Journey>();
            var input1 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 2, ToZone = 1, Time = 1020 };
            var input2 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input3 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
           

            Inputs.Add(input1);
            Inputs.Add(input2);
            Inputs.Add(input3);
       


            var fare = calculatefareObj.CalculateFare(Inputs);
            Assert.AreEqual(85, fare);
        }

        [TestMethod]
        public void TwoDayJourneysCappingNotReachedBothTest()
        {
            Inputs = new List<Journey>();
            var input1 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 2, ToZone = 1, Time = 1020 };
            var input2 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input3 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
            var input4 = new Journey() { Date = Convert.ToDateTime("05/10/2021"), FromZone = 2, ToZone = 1, Time = 1020 };
            var input5 = new Journey() { Date = Convert.ToDateTime("05/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input6 = new Journey() { Date = Convert.ToDateTime("05/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };


            Inputs.Add(input1);
            Inputs.Add(input2);
            Inputs.Add(input3);
            Inputs.Add(input4);
            Inputs.Add(input5);
            Inputs.Add(input6);



            var fare = calculatefareObj.CalculateFare(Inputs);
            Assert.AreEqual(170, fare);
        }

        [TestMethod]
        public void TwoDayJourneysCappingReachedBothDaysTest()
        {
            Inputs = new List<Journey>();
            var input1 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 2, ToZone = 1, Time = 1020 };
            var input2 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input3 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
            var input4 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 1, Time = 1815 };
            var input5 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 2, Time = 1900 };

            var input6 = new Journey() { Date = Convert.ToDateTime("05/10/2021"), FromZone = 2, ToZone = 1, Time = 1020 };
            var input7 = new Journey() { Date = Convert.ToDateTime("05/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input8 = new Journey() { Date = Convert.ToDateTime("05/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
            var input9 = new Journey() { Date = Convert.ToDateTime("05/10/2021"), FromZone = 1, ToZone = 1, Time = 1815 };
            var input10 = new Journey() { Date = Convert.ToDateTime("05/10/2021"), FromZone = 1, ToZone = 2, Time = 1900 };

            Inputs.Add(input1);
            Inputs.Add(input2);
            Inputs.Add(input3);
            Inputs.Add(input4);
            Inputs.Add(input5);
            Inputs.Add(input6);
            Inputs.Add(input7);
            Inputs.Add(input8);
            Inputs.Add(input9);
            Inputs.Add(input10);

            var fare = calculatefareObj.CalculateFare(Inputs);
            Assert.AreEqual(240, fare);
        }


        [TestMethod]
        public void TwoDayJourneysCappingReachedOneDayTest()
        {
            Inputs = new List<Journey>();
            var input1 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 2, ToZone = 1, Time = 1020 };
            var input2 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input3 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
            var input4 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 1, Time = 1815 };
            var input5 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 2, Time = 1900 };

            var input6 = new Journey() { Date = Convert.ToDateTime("05/10/2021"), FromZone = 2, ToZone = 1, Time = 1020 };
            var input7 = new Journey() { Date = Convert.ToDateTime("05/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input8 = new Journey() { Date = Convert.ToDateTime("05/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
           
            Inputs.Add(input1);
            Inputs.Add(input2);
            Inputs.Add(input3);
            Inputs.Add(input4);
            Inputs.Add(input5);
            Inputs.Add(input6);
            Inputs.Add(input7);
            Inputs.Add(input8);
            

            var fare = calculatefareObj.CalculateFare(Inputs);
            Assert.AreEqual(205, fare);
        }

        [TestMethod]
        public void TwoWeeksJourneryTests()
        {
            Inputs = new List<Journey>();
            var input1 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 2, ToZone = 1, Time = 1020 };
            var input2 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input3 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
            var input4 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 1, Time = 1815 };
            var input5 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 2, Time = 1900 };

            var input6 = new Journey() { Date = Convert.ToDateTime("05/10/2021"), FromZone = 2, ToZone = 1, Time = 1020 };
            var input7 = new Journey() { Date = Convert.ToDateTime("05/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input8 = new Journey() { Date = Convert.ToDateTime("05/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
            var input9 = new Journey() { Date = Convert.ToDateTime("05/10/2021"), FromZone = 1, ToZone = 1, Time = 1815 };
            var input10 = new Journey() { Date = Convert.ToDateTime("05/10/2021"), FromZone = 1, ToZone = 2, Time = 1900 };

            var input11 = new Journey() { Date = Convert.ToDateTime("06/10/2021"), FromZone = 2, ToZone = 1, Time = 1020 };
            var input12 = new Journey() { Date = Convert.ToDateTime("06/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input13 = new Journey() { Date = Convert.ToDateTime("06/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
            var input14 = new Journey() { Date = Convert.ToDateTime("06/10/2021"), FromZone = 1, ToZone = 1, Time = 1815 };
            var input15 = new Journey() { Date = Convert.ToDateTime("06/10/2021"), FromZone = 1, ToZone = 2, Time = 1900 };


            var input16 = new Journey() { Date = Convert.ToDateTime("07/10/2021"), FromZone = 2, ToZone = 1, Time = 1020 };
            var input17 = new Journey() { Date = Convert.ToDateTime("07/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input18 = new Journey() { Date = Convert.ToDateTime("07/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
            var input19 = new Journey() { Date = Convert.ToDateTime("07/10/2021"), FromZone = 1, ToZone = 1, Time = 1815 };
            var input20 = new Journey() { Date = Convert.ToDateTime("07/10/2021"), FromZone = 1, ToZone = 2, Time = 1900 };


            var input21 = new Journey() { Date = Convert.ToDateTime("08/10/2021"), FromZone = 2, ToZone = 1, Time = 1020 };
            var input22 = new Journey() { Date = Convert.ToDateTime("08/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input23 = new Journey() { Date = Convert.ToDateTime("08/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
            var input24 = new Journey() { Date = Convert.ToDateTime("08/10/2021"), FromZone = 1, ToZone = 1, Time = 1815 };
            var input25 = new Journey() { Date = Convert.ToDateTime("08/10/2021"), FromZone = 1, ToZone = 2, Time = 1900 };

            var input26 = new Journey() { Date = Convert.ToDateTime("09/10/2021"), FromZone = 2, ToZone = 1, Time = 1020 };
            var input27 = new Journey() { Date = Convert.ToDateTime("09/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input28 = new Journey() { Date = Convert.ToDateTime("09/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
            var input29 = new Journey() { Date = Convert.ToDateTime("09/10/2021"), FromZone = 1, ToZone = 1, Time = 1815 };
            var input30 = new Journey() { Date = Convert.ToDateTime("09/10/2021"), FromZone = 1, ToZone = 2, Time = 1900 };

            var input31 = new Journey() { Date = Convert.ToDateTime("10/10/2021"), FromZone = 2, ToZone = 1, Time = 1020 };
            var input32 = new Journey() { Date = Convert.ToDateTime("10/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input33 = new Journey() { Date = Convert.ToDateTime("10/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
            var input34 = new Journey() { Date = Convert.ToDateTime("10/10/2021"), FromZone = 1, ToZone = 1, Time = 1815 };
            var input35 = new Journey() { Date = Convert.ToDateTime("10/10/2021"), FromZone = 1, ToZone = 2, Time = 1900 };


            var input36 = new Journey() { Date = Convert.ToDateTime("11/10/2021"), FromZone = 1, ToZone = 1, Time = 1020 };
            var input37 = new Journey() { Date = Convert.ToDateTime("11/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input38 = new Journey() { Date = Convert.ToDateTime("11/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
            var input39 = new Journey() { Date = Convert.ToDateTime("11/10/2021"), FromZone = 1, ToZone = 1, Time = 1815 };
            var input40 = new Journey() { Date = Convert.ToDateTime("11/10/2021"), FromZone = 1, ToZone = 1, Time = 1900 };


            Inputs.Add(input1);
            Inputs.Add(input2);
            Inputs.Add(input3);
            Inputs.Add(input4);
            Inputs.Add(input5);
            Inputs.Add(input6);
            Inputs.Add(input7);
            Inputs.Add(input8);
            Inputs.Add(input9);
            Inputs.Add(input10);
            Inputs.Add(input11);
            Inputs.Add(input12);
            Inputs.Add(input13);
            Inputs.Add(input14);
            Inputs.Add(input15);
            Inputs.Add(input16);
            Inputs.Add(input17);
            Inputs.Add(input18);
            Inputs.Add(input19);
            Inputs.Add(input20);
            Inputs.Add(input21);
            Inputs.Add(input22);
            Inputs.Add(input23);
            Inputs.Add(input24);
            Inputs.Add(input25);
            Inputs.Add(input26);
            Inputs.Add(input27);
            Inputs.Add(input28);
            Inputs.Add(input29);
            Inputs.Add(input30);
            Inputs.Add(input31);
            Inputs.Add(input32);
            Inputs.Add(input33);
            Inputs.Add(input34);
            Inputs.Add(input35);
            Inputs.Add(input30);
            Inputs.Add(input36);
            Inputs.Add(input37);
            Inputs.Add(input38);
            Inputs.Add(input39);
            Inputs.Add(input40);


            var fare = calculatefareObj.CalculateFare(Inputs);
            Assert.AreEqual(700, fare);
        }

        [TestMethod]
        public void ThreeWeeksJourneryTests()
        {
            Inputs = new List<Journey>();
            var input1 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 2, ToZone = 1, Time = 1020 };
            var input2 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input3 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
            var input4 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 1, Time = 1815 };
            var input5 = new Journey() { Date = Convert.ToDateTime("04/10/2021"), FromZone = 1, ToZone = 2, Time = 1900 };

            var input6 = new Journey() { Date = Convert.ToDateTime("05/10/2021"), FromZone = 2, ToZone = 1, Time = 1020 };
            var input7 = new Journey() { Date = Convert.ToDateTime("05/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input8 = new Journey() { Date = Convert.ToDateTime("05/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
            var input9 = new Journey() { Date = Convert.ToDateTime("05/10/2021"), FromZone = 1, ToZone = 1, Time = 1815 };
            var input10 = new Journey() { Date = Convert.ToDateTime("05/10/2021"), FromZone = 1, ToZone = 2, Time = 1900 };

            var input11 = new Journey() { Date = Convert.ToDateTime("06/10/2021"), FromZone = 2, ToZone = 1, Time = 1020 };
            var input12 = new Journey() { Date = Convert.ToDateTime("06/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input13 = new Journey() { Date = Convert.ToDateTime("06/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
            var input14 = new Journey() { Date = Convert.ToDateTime("06/10/2021"), FromZone = 1, ToZone = 1, Time = 1815 };
            var input15 = new Journey() { Date = Convert.ToDateTime("06/10/2021"), FromZone = 1, ToZone = 2, Time = 1900 };


            var input16 = new Journey() { Date = Convert.ToDateTime("07/10/2021"), FromZone = 2, ToZone = 1, Time = 1020 };
            var input17 = new Journey() { Date = Convert.ToDateTime("07/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input18 = new Journey() { Date = Convert.ToDateTime("07/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
            var input19 = new Journey() { Date = Convert.ToDateTime("07/10/2021"), FromZone = 1, ToZone = 1, Time = 1815 };
            var input20 = new Journey() { Date = Convert.ToDateTime("07/10/2021"), FromZone = 1, ToZone = 2, Time = 1900 };


            var input21 = new Journey() { Date = Convert.ToDateTime("08/10/2021"), FromZone = 2, ToZone = 1, Time = 1020 };
            var input22 = new Journey() { Date = Convert.ToDateTime("08/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input23 = new Journey() { Date = Convert.ToDateTime("08/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
            var input24 = new Journey() { Date = Convert.ToDateTime("08/10/2021"), FromZone = 1, ToZone = 1, Time = 1815 };
            var input25 = new Journey() { Date = Convert.ToDateTime("08/10/2021"), FromZone = 1, ToZone = 2, Time = 1900 };

            var input26 = new Journey() { Date = Convert.ToDateTime("09/10/2021"), FromZone = 2, ToZone = 1, Time = 1020 };
            var input27 = new Journey() { Date = Convert.ToDateTime("09/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input28 = new Journey() { Date = Convert.ToDateTime("09/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
            var input29 = new Journey() { Date = Convert.ToDateTime("09/10/2021"), FromZone = 1, ToZone = 1, Time = 1815 };
            var input30 = new Journey() { Date = Convert.ToDateTime("09/10/2021"), FromZone = 1, ToZone = 2, Time = 1900 };

            var input31 = new Journey() { Date = Convert.ToDateTime("10/10/2021"), FromZone = 2, ToZone = 1, Time = 1020 };
            var input32 = new Journey() { Date = Convert.ToDateTime("10/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input33 = new Journey() { Date = Convert.ToDateTime("10/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
            var input34 = new Journey() { Date = Convert.ToDateTime("10/10/2021"), FromZone = 1, ToZone = 1, Time = 1815 };
            var input35 = new Journey() { Date = Convert.ToDateTime("10/10/2021"), FromZone = 1, ToZone = 2, Time = 1900 };


            var input36 = new Journey() { Date = Convert.ToDateTime("11/10/2021"), FromZone = 1, ToZone = 1, Time = 1020 };
            var input37 = new Journey() { Date = Convert.ToDateTime("11/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input38 = new Journey() { Date = Convert.ToDateTime("11/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
            var input39 = new Journey() { Date = Convert.ToDateTime("11/10/2021"), FromZone = 1, ToZone = 1, Time = 1815 };
            var input40 = new Journey() { Date = Convert.ToDateTime("11/10/2021"), FromZone = 1, ToZone = 1, Time = 1900 };

            var input41 = new Journey() { Date = Convert.ToDateTime("18/10/2021"), FromZone = 1, ToZone = 1, Time = 1020 };
            var input42 = new Journey() { Date = Convert.ToDateTime("18/10/2021"), FromZone = 1, ToZone = 1, Time = 1045 };
            var input43 = new Journey() { Date = Convert.ToDateTime("18/10/2021"), FromZone = 1, ToZone = 1, Time = 1615 };
            var input44 = new Journey() { Date = Convert.ToDateTime("18/10/2021"), FromZone = 1, ToZone = 1, Time = 1815 };
            var input45 = new Journey() { Date = Convert.ToDateTime("18/10/2021"), FromZone = 1, ToZone = 1, Time = 1900 };


            Inputs.Add(input1);
            Inputs.Add(input2);
            Inputs.Add(input3);
            Inputs.Add(input4);
            Inputs.Add(input5);
            Inputs.Add(input6);
            Inputs.Add(input7);
            Inputs.Add(input8);
            Inputs.Add(input9);
            Inputs.Add(input10);
            Inputs.Add(input11);
            Inputs.Add(input12);
            Inputs.Add(input13);
            Inputs.Add(input14);
            Inputs.Add(input15);
            Inputs.Add(input16);
            Inputs.Add(input17);
            Inputs.Add(input18);
            Inputs.Add(input19);
            Inputs.Add(input20);
            Inputs.Add(input21);
            Inputs.Add(input22);
            Inputs.Add(input23);
            Inputs.Add(input24);
            Inputs.Add(input25);
            Inputs.Add(input26);
            Inputs.Add(input27);
            Inputs.Add(input28);
            Inputs.Add(input29);
            Inputs.Add(input30);
            Inputs.Add(input31);
            Inputs.Add(input32);
            Inputs.Add(input33);
            Inputs.Add(input34);
            Inputs.Add(input35);
            Inputs.Add(input30);
            Inputs.Add(input36);
            Inputs.Add(input37);
            Inputs.Add(input38);
            Inputs.Add(input39);
            Inputs.Add(input40);
            Inputs.Add(input41);
            Inputs.Add(input42);

            Inputs.Add(input43);
            Inputs.Add(input44);
            Inputs.Add(input45);

            var fare = calculatefareObj.CalculateFare(Inputs);
            Assert.AreEqual(800, fare);
        }
    }
}
