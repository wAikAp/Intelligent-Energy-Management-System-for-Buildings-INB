using System;
using MongoDB.Bson;

namespace FYP_APP.Models.MongoModels
{
    public class MongoRoomModel
    {

        public ObjectId _id { get; set; }
        public String roomName { get; set; }
        public String roomId { get; set; }
        public String type { get; set; }
        public String desc { get; set; }
        public String power { get; set; }
        public String temp { get; set; }
        public String hum { get; set; }
        public String lig { get; set; }
        public String lightListId { get; set; }
        public String acListId { get; set; }
        public String humListId { get; set; }
        public String exhasFanListId { get; set; }
        public String sensorListId { get; set; }
        public String floor { get; set; }
        
    }
}
