using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FYP_WEB_APP.Models.API
{
    [Route("api/[controller]")]
    public class getSonserJsonController : Controller
    {
		// GET api/<controller>/5
        [HttpGet("{id}")]
        public IEnumerable<getSonserJsonModel> Get(int id)
        {
			List<getSonserJsonModel> dp= new List<getSonserJsonModel> { };
			List<List <dataPointModel>> dbdatas = new List<List<dataPointModel>>();
			List < dataPointModel>datapoint=new List<dataPointModel>{ };
			List<int> sint=new List<int> { } ;
			List<List<int>> Listint = new List<List<int>> { };
			int[] x1 = { 65, 59, 80, 81, 56, 55, 40, 50, 60, 55, 30, 78 };

			int[] x2 = { 10, 20, 60, 95, 64, 78, 90, 80, 70, 40, 70, 89 };

			int[] x3 = { 65, 59, 80, 81, 56, 55, 40, 50, 60, 55, 30, 78 };

			int[] x4 = { 50, 59, 70, 71, 56, 55, 45, 55, 60, 50, 30, 50 };
			int[] x5 = { 50, 59, 70, 71, 56, 55, 45, 55, 60, 50, 30, 50 };
			int[] x6 = { 50, 59, 70, 71, 56, 55, 45, 55, 60, 50, 30, 50 };
			int x = 1;
			foreach (int set in x1)
			{
				sint.Add(set);
			}
			Listint.Add(sint);
			sint = new List<int>();
			x = 1;
			foreach (int set in x2)
			{
				sint.Add(set);
			}
			Listint.Add(sint);



			/*dbdatas.Add(x2);
			dbdatas.Add(x3);
			dbdatas.Add(x4);
			dbdatas.Add(x5);
			dbdatas.Add(x6);*/

			/*foreach (List<int> set in Listint) {
				var data = new getSonserJsonModel()
				{	fill = false,
					spanGaps = false,
						label = "a",
                    borderColor= "#fff999",
					data = set
			};
			dp.Add(data);
			}*/
			var data = new getSonserJsonModel()
			{
				fill = false,
				spanGaps = false,
				label = "a0001",
				borderColor = "#fff999",
				data = Listint[0]
			};
			dp.Add(data);
			 data = new getSonserJsonModel()
			{
				fill = false,
				spanGaps = false,
				label = "a00002",
				borderColor = "#fff888",
				data = Listint[0]
			 };
			dp.Add(data);
			/*var str= "type: \"spline\","+
	"	visible: false," +
	"	showInLegend: true," +
	"	yValueFormatString: \"##.00mn\","+
	"	name: \"Season 1\"," +
	"	dataPoints: ";*/
			//return Content(dp.ToList(), "application/json");
			//var json = System.Text.Json.JsonSerializer.Serialize(dp);
			return dp;

			//return dp.ToList();
		}


	}
}
