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
        public List<FaceRectangleModel> ConvertJsonToFaceModels(string jsonString)
        {
            var json = JsonConvert.DeserializeObject(jsonString) as JArray;
            return json.Select(item => CreateFaceModel(item)).ToList();
        }

        private FaceRectangleModel CreateFaceModel(JToken item)
        {
            var faceRect = item["faceRectangle"];
            return new FaceRectangleModel()
            {
                X = (int)faceRect["top"],
                Y = (int)faceRect["left"],
                Width = (int)faceRect["width"],
                Height = (int)faceRect["height"]
            };
        }
    }
}
