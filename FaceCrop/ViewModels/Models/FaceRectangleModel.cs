using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels.Models
{
    public class FaceRectangleModel
    {
        [JsonProperty("top")]
        public int Top { get; set; }

        [JsonProperty("left")]
        public int Left { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }
}
