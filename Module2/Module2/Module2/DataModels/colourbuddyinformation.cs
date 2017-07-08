using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;



namespace Module2.DataModels
{
    public class colourbuddyinformation
    {
        [JsonProperty(PropertyName = "Id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "Colour")]
        public string Colour { get; set; }

        [JsonProperty(PropertyName = "Probability")]
        public string Probability { get; set; }
    }
}
