using ISSF2020.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISSF2020.Services
{
    public class ScheduleService
    {

        private readonly IMongoCollection<ScheduleModel> _schedule;

        public ScheduleService(IScheduleDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _schedule = database.GetCollection<ScheduleModel>(settings.ScheduleCollectionName);
        }

        public List<ScheduleModel> Get()
        {
            var sort = Builders<ScheduleModel>.Sort.Descending("_id"); // Newest element top of the list
            return _schedule.Find(schedule => true).Sort(sort).ToList();
        }

        public ScheduleModel Get(string eventname) =>
            _schedule.Find<ScheduleModel>(schedule => schedule.EventName == eventname).FirstOrDefault();

        public bool CheckUser(string username, string scheduleOwner)
        {
            var schedule = _schedule.Find<ScheduleModel>(schedule => schedule.User == scheduleOwner).FirstOrDefault();

            if (schedule.User == username)
            {
                return true;
            }
            return false;
        }

        public ScheduleModel Create(ScheduleModel schedule)
        {
            _schedule.InsertOne(schedule);
            return schedule;
        }

        public void Update(string id, ScheduleModel scheduleIn) =>
            _schedule.ReplaceOne(schedule => schedule.Id == id, scheduleIn);

        public void Remove(ScheduleModel scheduleIn) =>
            _schedule.DeleteOne(schedule => schedule.Id == scheduleIn.Id);

        public void Remove(string id) =>
            _schedule.DeleteOne(schedule => schedule.Id == id);

    }
}
