using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FYP_WEB_APP.Models;
using FYP_WEB_APP.Models.MongoModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FYP_APP.Controllers
{
    public class SchedulesController : Controller
    {
        DateTime today = DateTime.Now;
        string ScheduleCollectionName = "schedules";
        private List<ScheduleModel> _scheduleModel;
        List<ScheduleSectionsViewModel> _Sections;
        List<ScheduleSectionsViewModel> _plan;
        List<ScheduleViewModel> _ScheduleList;

        public IActionResult Schedules()
        {
            SetSections();
            ViewBag.Sections = _Sections.ToJson();
            SetPlan();
            ViewBag.plan = _plan.ToJson();
            SetDataSet();
            ViewBag.ScheduleList = _ScheduleList.ToJson();
            ViewBag.y = today.Year;
            ViewBag.m = today.AddMonths(-1).Month;
            ViewBag.d = today.Day;

            return View();
        }
        public List<ScheduleModel> GetScheduleList() {
            var scheduleDb = new DBManger().ScheduleDatabase.GetCollection<ScheduleModel>(ScheduleCollectionName);

            var sort = Builders<ScheduleModel>.Sort.Descending("_id"); // Newest element top of the list
            return scheduleDb.Find(schedule => true).Sort(sort).ToList();
        }
        public string SchedulesJson()
        {
            SetDataSet();
            return _ScheduleList.ToJson();
        }
        public void SetDataSet() {
            _ScheduleList = new List<ScheduleViewModel>();
            foreach (var get in GetScheduleList())
            {
                DateTime sdate = DateTime.Parse(get.Date + " " + get.Time);
                Debug.WriteLine("start date : " + sdate);
                DateTime eDate = sdate.AddMinutes(Int32.Parse(get.Duration));
                string checkUser = "";
                if (get.User != null) {
                    checkUser = get.User;
                }

                var data = new ScheduleViewModel
                {
                    start_date = sdate.ToString("yyyy-MM-dd HH:mm"),
                    end_date = eDate.ToString("yyyy-MM-dd HH:mm"),
                    EventName = get.EventName,
                    EventType = get.EventType,
                    section_id = getSectionId(get.Location).ToString(),
                    UsagePlan = get.UsagePlan,
                    User = checkUser,
                    Room = get.Location
                };
                _ScheduleList.Add(data);
            }
        }
        public void SetSections() {
            _Sections = new List<ScheduleSectionsViewModel>();

            var roomList = new RoomsController().getRoomListFromDB();
            ViewData["RoomList"] = roomList;

            int x = 0;
            foreach (var get in roomList) {
                x++;
                var data = new ScheduleSectionsViewModel()
                {
                    key = x, label = get.roomId
                };
                _Sections.Add(data);

            }
        }

        public int getSectionId(string id)
        { int key = 0;
            SetSections();
            try { key = _Sections.Where(x => x.label.Equals(id)).First().key;
                if (key == 0) {
                    throw new System.ArgumentException("Parameter roomid cannot find", "original");
                }
            } catch (Exception e) {
                return key;
            }
            return key;
        }

        public void SetPlan()
        {
            List<MongoDefaulttUsagePlanModel> list = new List<MongoDefaulttUsagePlanModel>();
            _plan = new List<ScheduleSectionsViewModel>();
            int key = 0;
            try
            {
                list = new DBManger().DataBase.GetCollection<MongoDefaulttUsagePlanModel>("DEFAULT_USAGE_PLAN").Find(new BsonDocument()).ToList();
                foreach (var get in list)
                {
                    key++;
                    var data = new ScheduleSectionsViewModel()
                    {
                        key = key,
                        label = get.name
                    };
                    _plan.Add(data);

                }
            }
            catch (Exception e)
            {
            }
            //got df plan
            //start PERSONAL_USAGE_PLAN

            var PERSONAL = new DBManger().DataBase.GetCollection<MongoPersonalUsagePlanModel>("PERSONAL_USAGE_PLAN").Find(new BsonDocument()).ToList();
            foreach (var get in PERSONAL)
            {
                key++;
                var data = new ScheduleSectionsViewModel()
                {
                    key = key,
                    label = get.name
                };
                _plan.Add(data);

            }

        }

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
