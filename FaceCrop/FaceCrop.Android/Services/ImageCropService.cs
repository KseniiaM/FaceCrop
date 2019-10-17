using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FaceCrop.Droid.Services;
using Plugin.Media.Abstractions;
using ViewModels.Models;
using ViewModels.Services;
using Xamarin.Forms;


[assembly: Dependency(typeof(ImageCropService))]
namespace FaceCrop.Droid.Services
{
    class ImageCropService : IImageCropService
    {
        public List<ImageSource> CropImage(MediaFile mediaFile, List<FaceRectangleModel> faces)
        {
            var bitmap = BitmapFactory.DecodeStream(mediaFile.GetStream());

            return faces.Select(faceModel =>
            {
                var cropped = Bitmap.CreateBitmap(bitmap,
                                               faceModel.X,
                                               faceModel.Y,
                                               faceModel.Width,
                                               faceModel.Height);

                using (MemoryStream stream = new MemoryStream())
                {
                    cropped.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                    return ImageSource.FromStream(() => new MemoryStream(stream.ToArray()));
                }
            }).ToList();
        }
    }
}