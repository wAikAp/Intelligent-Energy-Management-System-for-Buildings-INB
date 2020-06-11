using ISSF2020.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISSF2020.Services
{
    public class RegionalWeatherService
    {

        private readonly IMongoCollection<RegionalWeatherModel> _weather;

        public RegionalWeatherService(IRegionalWeatherDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _weather = database.GetCollection<RegionalWeatherModel>(settings.RegionalWeatherCollectionName);
        }

        public List<RegionalWeatherModel> GetAll()
        {
            var sort = Builders<RegionalWeatherModel>.Sort.Descending("_id"); // Newest element top of the list
            return _weather.Find(weather => true).Sort(sort).ToList();
        }

        public RegionalWeatherModel GetLast()
        {
            var sort = Builders<RegionalWeatherModel>.Sort.Descending("_id");
            return _weather.Find<RegionalWeatherModel>(schedule => true).Sort(sort).FirstOrDefault();
        }

        public RegionalWeatherModel GetTime(string time) =>
            _weather.Find<RegionalWeatherModel>(schedule => schedule.LastUpdate == time).FirstOrDefault();

        public RegionalWeatherModel Create(RegionalWeatherModel weather)
        {
            _weather.InsertOne(weather);
            return weather;
        }

        public void Update(string id, RegionalWeatherModel weatherIn) =>
            _weather.ReplaceOne(weather => weather.Id == id, weatherIn);

        public void Remove(RegionalWeatherModel weatherIn) =>
            _weather.DeleteOne(weather => weather.Id == weatherIn.Id);

        public void Remove(string id) =>
            _weather.DeleteOne(weather => weather.Id == id);

    }
}
