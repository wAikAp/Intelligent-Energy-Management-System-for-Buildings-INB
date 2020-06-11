using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISSF2020.Models
{
    public class UserDatabaseSettings : IUserDatabaseSettings
    {
        public string UserCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IUserDatabaseSettings
    {
        string UserCollectionName { get; set; }
        public string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
