using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels.Models
{
    class FacesModel
    {
        public List<FaceRectangleModel> faceRectangleModels { get; set; }

        public FacesModel(List<FaceRectangleModel> faceRectangleModels)
        {
            this.faceRectangleModels = faceRectangleModels;
        }
    }


    public class Rootobject
    {
        public Class1[] Property1 { get; set; }
    }

    public class Class1
    {
        public string faceId { get; set; }
        public Facerectangle faceRectangle { get; set; }
        public Faceattributes faceAttributes { get; set; }
    }

    public class Facerectangle
    {
        public int top { get; set; }
        public int left { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Faceattributes
    {
        public float age { get; set; }
        public Emotion emotion { get; set; }
    }

    public class Emotion
    {
        public float anger { get; set; }
        public float contempt { get; set; }
        public float disgust { get; set; }
        public float fear { get; set; }
        public float happiness { get; set; }
        public float neutral { get; set; }
        public float sadness { get; set; }
        public float surprise { get; set; }
    }
}
