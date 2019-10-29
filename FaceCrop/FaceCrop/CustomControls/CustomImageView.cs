using System.Collections.Generic;
using ViewModels.Models;
using Xamarin.Forms;

namespace FaceCrop.CustomControls
{
    public class CustomImageView : Image
    {
        public static readonly BindableProperty RectangleCollectionProperty = BindableProperty.Create(
                "RectangleCollection", 
                typeof(List<FaceRectangleModel>),
                typeof(CustomImageView));

        public List<FaceRectangleModel> RectangleCollection
        {
            get { return (List<FaceRectangleModel>)GetValue(RectangleCollectionProperty); }
            set { SetValue(RectangleCollectionProperty, value); }
        }
    }
}