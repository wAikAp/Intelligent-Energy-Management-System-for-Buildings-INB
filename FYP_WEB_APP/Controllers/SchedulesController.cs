using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP_WEB_APP.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FYP_APP.Controllers
{
    public class SchedulesController : Controller
    {
        string ScheduleCollectionName = "schedules";
        private List<ScheduleModel> _scheduleModel;
        List<ScheduleSectionsViewModel> _Sections;
        public IActionResult Schedules()
        {
            return View();
        }
        public List<ScheduleModel> GetScheduleList() {
            var scheduleDb = new DBManger().ScheduleDatabase.GetCollection<ScheduleModel>(ScheduleCollectionName);

            var sort = Builders<ScheduleModel>.Sort.Descending("_id"); // Newest element top of the list
            return scheduleDb.Find(schedule => true).Sort(sort).ToList();
        }
        public string SchedulesJson()
        {

            return GetScheduleList().ToJson();
        }
        public void SetSections() {
            _Sections = new List<ScheduleSectionsViewModel>();

            var roomList =new RoomsController().getRoomListFromDB();
            ViewData["RoomList"] = roomList;

            int x = 0;
            foreach (var get in roomList){
                x++;
               var data = new ScheduleSectionsViewModel()
                {
                   key=x,label=get.roomId
               };
                _Sections.Add(data);

            }
        }


        /*
         get room list
         get power plan list


         */

    }
    public class ScheduleViewModel // create Schedules Json model
    {
        public ObjectId _id { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int Duration { get; set; }
        public string EventName { get; set; }
        public string EventType { get; set; }
        public string section_id { get; set; }//Location
        public string PowerPlan { get; set; }
        public string User { get; set; }
    }
    public class ScheduleSectionsViewModel // create Schedules Json model
    {
        public int key { get; set; }
        public string label { get; set; }
   
    }
}
/*
 { "_id" : ObjectId("5ec892d59422fd3bd08bb252"),
 "Date" : "2020-05-24",
 "Time" : "00:00",
 "Duration" : "60",
 "EventName" : "123", 
 "EventType" : "abc", 
 "Location" : "348",
 "UsagePlan" : "smallclass",
 "User" : null }
     */
