using System;
using System.Diagnostics;
using System.Linq;
using FYP_WEB_APP.Models.MongoModels;

using MongoDB.Driver;

namespace FYP_WEB_APP.Models.LogicModels
{
    public class PiCamUtil
    {
        private static DBManger dBManger = new DBManger();
        private readonly IMongoCollection<MongoPICamModel> piCamlCollection = dBManger.DataBase.GetCollection<MongoPICamModel>("PI_CAM");

        public string getCamBase64_img(string deviceID) {

            var camls = piCamlCollection.Find(x => x.deviceId == deviceID).ToList();
            MongoPICamModel pICamModel = camls.Last();
            Debug.WriteLine("!pICamModel img:" + pICamModel.base_cam_img);
            return pICamModel.base_cam_img;
        }
    }
}
