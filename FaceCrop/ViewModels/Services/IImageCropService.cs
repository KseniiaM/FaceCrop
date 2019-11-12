using Plugin.Media.Abstractions;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using ViewModels.Models;
using Xamarin.Forms;

namespace ViewModels.Services
{
    public interface IImageCropService
    {
        Task<ImageSource> DrawFaceRactangles(StreamImageSource image, List<FaceRectangleModel> faces);

        Task<ImageSource> CropImages(StreamImageSource imageSource, FaceRectangleModel faceRectangleModel);
    }
}
