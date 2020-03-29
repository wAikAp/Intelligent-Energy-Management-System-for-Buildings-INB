using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_WEB_APP.Models.API
{
    public class SensorsAddDataModel
    {
        public string Sensorid{ get; set; }
        public string Value{ get; set; }

        internal void Equals()
        {
            throw new NotImplementedException();
        }
    }
}
