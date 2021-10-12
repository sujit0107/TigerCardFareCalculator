using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using TigerCard.Implementations;
using TigerCard.Interfaces;
using TigerCard.Models;

namespace TigerCard
{
    public class InputFileReader
    {
        int totalfare;
        public void ReadInputFile()
        {
            var serviceProvider = new ServiceCollection()
                .AddTransient<IRulesReader, XmlFileRulesReader>()
                .AddTransient<ICommonFeatures, CommonFeatures>()
                .AddTransient<IFareCalculator, WeeklyCappedFareCalculator>()
                .BuildServiceProvider();

            string[] lines = System.IO.File.ReadAllLines(@"InputFile.txt");
            List<Journey> journeys = new List<Journey>();

            foreach (string line in lines)
            {
                Console.WriteLine("\t" + line);
                var inputdata = line.Split("|");
                if (!inputdata[0].Trim().Equals("Date", StringComparison.InvariantCultureIgnoreCase) && inputdata.Length > 1)
                {
                    Journey obj = new Journey
                    {
                        Date = Convert.ToDateTime(inputdata[0].Trim()),
                        Time = Convert.ToInt32(inputdata[1].Trim().Replace(":", string.Empty)),
                        FromZone = Convert.ToInt32(inputdata[2].Trim()),
                        ToZone = Convert.ToInt32(inputdata[3].Trim())
                    };

                    journeys.Add(obj);
                }
                else if (journeys.Count > 0)
                {
                    var fareCalculator = serviceProvider.GetService<IFareCalculator>();
                    if (fareCalculator != null) totalfare = fareCalculator.CalculateFare(journeys);
                    Console.WriteLine("TotalFare :" + totalfare);
                    Console.WriteLine("       -------------------------------------------------------------------------------------");
                    totalfare = 0;
                    journeys.Clear();
                }

            }
        }
    }
}
