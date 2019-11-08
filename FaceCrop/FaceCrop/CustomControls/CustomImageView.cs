using System;
using System.Collections.Generic;
using ViewModels.Models;
using ViewModels.Services;
using Xamarin.Forms;

namespace FaceCrop.CustomControls
{
    public class CustomImageView : Image
    {
        public StreamImageSource ImageWithNoRectangles;

        public static readonly BindableProperty RectangleCollectionProperty = BindableProperty.Create(
                "RectangleCollection", 
                typeof(List<FaceRectangleModel>),
                typeof(CustomImageView));

        public List<FaceRectangleModel> RectangleCollection
        {
            get { return (List<FaceRectangleModel>)GetValue(RectangleCollectionProperty); }
            set { SetValue(RectangleCollectionProperty, value); }
        }

        protected override async void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var rectangles = await DependencyService.Get<IImageCropService>()
                                                    .DrawFaceRactangles((StreamImageSource) this.Source, this.RectangleCollection);
            this.Source = rectangles;
        }
    }
}