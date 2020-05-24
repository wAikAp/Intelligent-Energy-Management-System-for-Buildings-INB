using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_WEB_APP.Models
{
    public class ScheduleModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public string Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public string Time { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Duration { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string EventName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string EventType { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Location { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string UsagePlan { get; set; }

        [DataType(DataType.Text)]
        public string User { get; set; }

    }
}
