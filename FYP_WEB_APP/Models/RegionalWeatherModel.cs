using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_WEB_APP.Models
{
    public class RegionalWeatherModel
    {
        public class DetailsModel
        {
            [Required]
            [BsonElement("temp_feels_like")]
            public double TempFeelsLike { get; set; }

            [Required]
            [BsonElement("temp_min")]
            public double TempMin { get; set; }

            [Required]
            [BsonElement("temp_max")]
            public double TempMax { get; set; }
        }


        public class HistoryModel
        {
            [Required]
            [BsonElement("time")]
            public string time { get; set; }

            [Required]
            [BsonElement("temperature")]
            public double Temperature { get; set; }

            [Required]
            [BsonElement("description")]
            public string Description { get; set; }

            [Required]
            [BsonElement("icon")]
            public string icon { get; set; }

            [Required]
            [BsonElement("wind")]
            public double Wind { get; set; }

            [Required]
            [BsonElement("humidity")]
            public double Humidity { get; set; }

            [Required]
            [BsonElement("details")]
            public DetailsModel Details { get; set; }

        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [BsonElement("date")]
        public string Date { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [BsonElement("last_update")]
        public string LastUpdate { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [BsonElement("location")]
        public string Location { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [BsonElement("temperature")]
        public double Temperature { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [BsonElement("description")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [BsonElement("humidity")]
        public double Humidity { get; set; }

        [Required]
        [BsonElement("details")]
        public DetailsModel Details { get; set; }

        [Required]
        [BsonElement("history")]
        public List<HistoryModel> History { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [BsonElement("records")]
        public double Records { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [BsonElement("icon")]
        public string icon { get; set; }

        [Required]
        [BsonElement("wind")]
        public double Wind { get; set; }//wind speed

    }
}
