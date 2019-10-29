using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ViewModels.Models
{
    public class FaceRectangleCollectionModel
    {
        public List<FaceRectangleModel> RectangleModels { get; set; }

        public ImageSource OriginalImage { get; set; }
    }
}
