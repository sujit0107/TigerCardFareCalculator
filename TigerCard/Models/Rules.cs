namespace TigerCard.Models
{


    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class Rules
    {

        private RulesTiming[] peakTimingsField;

        private RulesFare[] faresField;

        private RulesCaping[] capingsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Timing", IsNullable = false)]
        public RulesTiming[] PeakTimings
        {
            get => peakTimingsField;
            set => peakTimingsField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Fare", IsNullable = false)]
        public RulesFare[] Fares
        {
            get => faresField;
            set => faresField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Caping", IsNullable = false)]
        public RulesCaping[] Capings
        {
            get => capingsField;
            set => capingsField = value;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class RulesTiming
    {

        private string dayField;

        private ushort fromField;

        private ushort toField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Day
        {
            get => dayField;
            set => dayField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort From
        {
            get => fromField;
            set => fromField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort To
        {
            get => toField;
            set => toField = value;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class RulesFare
    {

        private int fromZoneField;

        private int toZoneField;

        private int peakHoursFareField;

        private int offPeakHourFareField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int FromZone
        {
            get => fromZoneField;
            set => fromZoneField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int ToZone
        {
            get => toZoneField;
            set => toZoneField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int PeakHoursFare
        {
            get => peakHoursFareField;
            set => peakHoursFareField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int OffPeakHourFare
        {
            get => offPeakHourFareField;
            set => offPeakHourFareField = value;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class RulesCaping
    {

        private int fromZoneField;

        private int toZoneField;

        private int dailyCapField;

        private ushort weeklyCapField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int FromZone
        {
            get => fromZoneField;
            set => fromZoneField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int ToZone
        {
            get => toZoneField;
            set => toZoneField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int DailyCap
        {
            get => dailyCapField;
            set => dailyCapField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort WeeklyCap
        {
            get => weeklyCapField;
            set => weeklyCapField = value;
        }
    }



}
