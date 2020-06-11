using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISSF2020.Models
{
    public class ScheduleDatabaseSettings : IScheduleDatabaseSettings
    {
        public string ScheduleCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IScheduleDatabaseSettings
    {
        string ScheduleCollectionName { get; set; }
        public string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
