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
                'Top': '30',
                'Left': '30',
                'Width': '100',
                'Height': '130'
            },
            {
                'Top': '80',
                'Left': '70',
                'Width': '100',
                'Height': '100'
            }
        ]";

        public List<FaceRectangleModel> ConvertJsonToFaceModels(string jsonString)
        {
            var json = JsonConvert.DeserializeObject(jsonString) as JArray;
            return json.Select(item => CreateFaceModel(item)).ToList();
        }

        public List<FaceRectangleModel> ReturnEmulatedFaceModels()
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.MissingMemberHandling = MissingMemberHandling.Error;

            var json = JsonConvert.DeserializeObject<List<JToken>>(faceJson, serializerSettings);
            return json.Select(item => CreateFaceModel(item)).ToList();
        }

        private FaceRectangleModel CreateFaceModel(JToken faceRect)
        {
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
