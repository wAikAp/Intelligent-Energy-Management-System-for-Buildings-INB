using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_WEB_APP.Models.LogicModels
{
	public class DailyUsageModel // total power consumtion of the buliding
	{
		public string roomId { get; set; }
		public String recorded_date { get; set; }
		public double power_used { get; set; }//kwh
	}
}
