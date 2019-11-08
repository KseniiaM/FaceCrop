using System;
using System.Collections.Generic;
using ViewModels.Models;
using ViewModels.Services;
using Xamarin.Forms;

namespace FaceCrop.CustomControls
{
    public class CustomImageView : Image
    {
        public event Action RectangleCollectionChangedHandler;

        public static readonly BindableProperty RectangleCollectionProperty = BindableProperty.Create(
                "RectangleCollection", 
                typeof(List<FaceRectangleModel>),
                typeof(CustomImageView),
                propertyChanged: RectangleCollectionChanged);

        public List<FaceRectangleModel> RectangleCollection
        {
            get { return (List<FaceRectangleModel>)GetValue(RectangleCollectionProperty); }
            set { SetValue(RectangleCollectionProperty, value); }
        }

        private static void RectangleCollectionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CustomImageView view)
            {
                view.RectangleCollectionChangedHandler?.Invoke();
            }
        }
    }
}