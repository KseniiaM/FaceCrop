using Plugin.Media.Abstractions;
using System.Collections.Generic;
using System.Drawing;
using ViewModels.Models;
using Xamarin.Forms;

namespace ViewModels.Services
{
    public interface IImageCropService
    {
        ImageSource DrawFaceRactangles(MediaFile mediaFile, List<FaceRectangleModel> faces);

        List<ImageSource> CropImages(MediaFile mediaFile, List<FaceRectangleModel> faceRectangleModel);
    }
}
