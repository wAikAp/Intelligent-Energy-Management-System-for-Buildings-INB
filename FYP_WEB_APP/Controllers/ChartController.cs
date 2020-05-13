using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP_WEB_APP.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace FYP_WEB_APP.Controllers
{
    public class ChartController : Controller
    {
        public string Chart(int count, List<string> labelss, List<object> data)
        {
			if (count != labelss.Count() & count != data.Count())
			{
				throw new System.InvalidOperationException("The List and labels and data .count Not match !! ");
			}
			else { 
			List<string> Color = new List<string>();

			List<object> datasets = new List<object>();

			for (int i = 0; i < count; i++)
			{
				Color.Add(getRandomColor());
			}
			for (int i = 0; i < count; i++)
			{

				var chartdata = new chartModel()
				{
					fill = false,
					spanGaps = false,
					label = labelss[i],
					borderColor = Color[i],
					data = data[i]
				};

				datasets.Add(chartdata);
			}

			return JsonConvert.SerializeObject(datasets);
			}
		}

		public string getRandomColor()
		{
			var random = new Random();
			var rmcolor = String.Format("#{0:X6}", random.Next(0x1000000));
			return rmcolor;
		}
		public string getRandomDivId()
		{
			var random = new Random();
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, 10)
			  .Select(s => s[random.Next(s.Length)]).ToArray());
			//return rmcolor;
		}
		public string getChartTime()
		{
			DateTime today = DateTime.Now;
			int hour = today.Hour;
			int Minute = today.Minute;

			int oMinute = today.Minute;
			//set time
			int ix = 0;
			List<string> time = new List<string>();
			for (int i = hour; i < 24; i++)
			{
				for (ix = Minute; ix < 60; ix += 5)
				{
					time.Add(i.ToString() + ":" + ix.ToString());
					if (ix > 55)
					{
						Minute = ix + 5;
						Minute %= 60;
					}
					else if (ix == 55)
					{
						Minute = 0;
					}
				}
			}

			for (int i = 0; i < hour + 1; i++)
			{
				for (int ixx = Minute; ixx < 60; ixx += 5)
				{
					if (i == hour && ixx > oMinute)
					{

					}
					else
					{
						time.Add(i.ToString() + ":" + ixx.ToString());

					}
				}
			}
			return time.ToJson();

		}
	}
}