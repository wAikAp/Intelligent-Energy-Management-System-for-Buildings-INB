using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISSF2020.Models
{
    public class RegionalWeatherDatabaseSettings : IRegionalWeatherDatabaseSettings
    {
        public string RegionalWeatherCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IRegionalWeatherDatabaseSettings
    {
        string RegionalWeatherCollectionName { get; set; }
        public string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
