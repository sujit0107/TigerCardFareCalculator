using System.Collections.Generic;
using TigerCard.Models;

namespace TigerCard.Interfaces
{
    public interface IFareCalculator
    {
        int CalculateFare(List<Journey> journeys);
    }
}