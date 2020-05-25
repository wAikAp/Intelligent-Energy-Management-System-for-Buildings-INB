using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_WEB_APP.Models
{
    public class RoomsListModel
    {
        public ObjectId _id { get; set; }
        public string roomId { get; set; }
        public string desc { get; set; }
        public string roomName { get; set; }
        public string type { get; set; }
        public string temp { get; set; }
        public string hum { get; set; }
        public string lig { get; set; }
        public double power { get; set; }
        public string lightListId { get; set; }
        public string acListId { get; set; }
        public string humListId { get; set; }
        public string exhasFanListId { get; set; }
        public string sensorListId { get; set; }
        public string floor { get; set; }
        public string floorPlanImg { get; set; }
    }
}
