using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ViewModels.Models;
using ViewModels.Services;
using Xamarin.Forms;

namespace FaceCrop.CustomControls
{
    public class CustomImageView : Image
    {
        public ImageSource ImageWithoutRectangles { get; private set; }
        public ImageSource ImageWithRectangles { get; private set; }

        public event Action<FaceRectangleModel> SelectedRectangleChanged;

        public static readonly BindableProperty RectangleCollectionProperty = BindableProperty.Create(
                "RectangleCollection", 
                typeof(List<FaceRectangleModel>),
                typeof(CustomImageView));

        public static readonly BindableProperty SelectedRectangleProperty = BindableProperty.Create(
                "SelectedRectangle",
                typeof(FaceRectangleModel),
                typeof(CustomImageView));

        public List<FaceRectangleModel> RectangleCollection
        {
            get { return (List<FaceRectangleModel>)GetValue(RectangleCollectionProperty); }
            set { SetValue(RectangleCollectionProperty, value); }
        }

        public FaceRectangleModel SelectedRectangle
        {
            get { return (FaceRectangleModel)GetValue(SelectedRectangleProperty); }
            set { SetValue(SelectedRectangleProperty, value); }
        }

        protected async override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == SelectedRectangleProperty.PropertyName)
            {
                Source = SelectedRectangle == null ? ImageWithRectangles : ImageWithoutRectangles;
                SelectedRectangleChanged?.Invoke(this.SelectedRectangle);
            }

            if (propertyName == RectangleCollectionProperty.PropertyName)
            {
                await DrawFaceRectangles();
            }
        }

        private async Task DrawFaceRectangles()
        {
            if (Source != null && RectangleCollection != null)
            {
                ImageWithoutRectangles = Source;
                ImageWithRectangles = await DependencyService.Get<IImageCropService>()
                                                             .DrawFaceRactangles((StreamImageSource)this.Source, this.RectangleCollection);
                this.Source = ImageWithRectangles;
            }
        }
    }
}