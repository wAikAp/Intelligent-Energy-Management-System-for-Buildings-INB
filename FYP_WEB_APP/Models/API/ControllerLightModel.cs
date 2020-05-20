using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_WEB_APP.Models.API
{
    public class ControllerLightModel
    {
        private int[] lightArray = { 0, 1, 0, 1, 0, 1, 0, 1 };
        public int[] Light { 
            get { return lightArray; }
            set { this.lightArray = value; }
        }

    }
}
