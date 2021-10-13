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
        int totalFare;
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
                try
                {

                    Console.WriteLine("\t" + line);
                    var inputData = line.Split("|");
                    if (!inputData[0].Trim().Equals("Date", StringComparison.InvariantCultureIgnoreCase) && inputData.Length > 1)
                    {
                        Journey obj = new Journey
                        {
                            Date = Convert.ToDateTime(inputData[0].Trim()),
                            Time = Convert.ToInt32(inputData[1].Trim().Replace(":", string.Empty)),
                            FromZone = Convert.ToInt32(inputData[2].Trim()),
                            ToZone = Convert.ToInt32(inputData[3].Trim())
                        };

                        journeys.Add(obj);
                    }
                    else if (journeys.Count > 0)
                    {
                        var fareCalculator = serviceProvider.GetService<IFareCalculator>();
                        if (fareCalculator != null) totalFare = fareCalculator.CalculateFare(journeys);
                        if (totalFare == 0)
                            Console.WriteLine("TotalFare :" + totalFare + ". Provided invalid input values");
                        else
                        {
                            Console.WriteLine("TotalFare :" + totalFare);
                        }

                        Console.WriteLine("       -------------------------------------------------------------------------------------");
                        totalFare = 0;
                        journeys.Clear();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + ". Provided invalid input values ");

                }

            }
        }
    }
}
