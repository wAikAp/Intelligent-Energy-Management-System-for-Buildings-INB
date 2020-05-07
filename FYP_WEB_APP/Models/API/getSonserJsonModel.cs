using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_WEB_APP.Models.API
{
    public class getSonserJsonModel
    {
        public string label { get; set; }
        public string borderColor { get; set; }
        public bool fill { get; set; }
        public bool spanGaps { get; set; }
        public List<int> data { get; set; }
        
    }
}
