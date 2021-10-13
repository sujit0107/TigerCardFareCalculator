using System;
using TigerCard.Interfaces;
using TigerCard.Models;

namespace TigerCard.Implementations
{
    public class XmlFileRulesReader : IRulesReader
    {
        public Rules PopulateRules()
        {
            try
            {
                System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(Rules));
                System.IO.StreamReader file = new System.IO.StreamReader(@"Rules.xml");
                return (Rules)reader.Deserialize(file);
            }
            catch
            {
                throw new Exception("Unable to populate rules");
            }
        }
    }
}
