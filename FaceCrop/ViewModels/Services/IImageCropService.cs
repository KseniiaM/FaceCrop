using Plugin.Media.Abstractions;
using System.Collections.Generic;
using System.Drawing;
using ViewModels.Models;
using Xamarin.Forms;

namespace ViewModels.Services
{
    public interface IImageCropService
    {
        List<ImageSource> CropImage(MediaFile mediaFile, List<FaceRectangleModel> faceRectangleModel);
    }
}
