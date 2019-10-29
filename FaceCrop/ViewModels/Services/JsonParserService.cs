using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ViewModels.Models;

namespace ViewModels.Services
{
    public class JsonParserService : IJsonParserService
    {
        public const string faceJson = @"[
            {
            'faceRectangle':{
                'Top': '30',
                'Left': '30',
                'Width': '130',
                'Height': '150'
            }
        }]";

        public List<FaceRectangleModel> ConvertJsonToFaceModels(string jsonString)
        {
            var json = JsonConvert.DeserializeObject(jsonString) as JArray;
            return json.Select(item => CreateFaceModel(item)).ToList();
        }

        public List<FaceRectangleModel> ReturnEmulatedFaceModels()
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.MissingMemberHandling = MissingMemberHandling.Error;

            var json = JsonConvert.DeserializeObject<JToken>(faceJson, serializerSettings);
            return json.Children().Select(item => CreateFaceModel(item)).ToList();
        }

        private FaceRectangleModel CreateFaceModel(JToken item)
        {
            var faceRect = item["faceRectangle"];
            return new FaceRectangleModel()
            {
                Top = (int)faceRect["Top"],
                Left = (int)faceRect["Left"],
                Width = (int)faceRect["Width"],
                Height = (int)faceRect["Height"]
            };
        }


    }
}
