using System;
using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using FaceCrop.Droid.Services;
using FaceCrop.Droid.Utils;
using Plugin.Media.Abstractions;
using ViewModels.Models;
using ViewModels.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageCropService))]
namespace FaceCrop.Droid.Services
{
    class ImageCropService : IImageCropService
    {
        public List<ImageSource> CropImages(MediaFile mediaFile, List<FaceRectangleModel> faces)
        {
            var bitmap = BitmapFactory.DecodeStream(mediaFile.GetStream());
            
            return faces.Select(faceModel =>
            {
                var cropped = Bitmap.CreateBitmap(bitmap,
                                               faceModel.Top,
                                               faceModel.Left,
                                               faceModel.Width,
                                               faceModel.Height);

                return BitmapUtils.ConvertBitmapToImageSource(cropped);
            }).ToList();
        }

        public ImageSource DrawFaceRactangles(MediaFile mediaFile, List<FaceRectangleModel> faces)
        {
            var bitmap = BitmapUtils.ConvertToMutableBitmap(BitmapFactory.DecodeStream(mediaFile.GetStream()));
            bitmap.SetConfig(Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(bitmap);

            foreach (var face in faces)
            {
                DrawRectangle(face, canvas);
            }

            return BitmapUtils.ConvertBitmapToImageSource(bitmap);
        }

        private void DrawRectangle(FaceRectangleModel face, Canvas canvas)
        {
            RectF bounds = new RectF(face.Left,
                         face.Top,
                         face.Width + face.Left,
                         face.Height + face.Top);
            var paint = new Paint();
            paint.Color = new Android.Graphics.Color(255, 255, 255);
            paint.SetStyle(Paint.Style.Stroke);
            canvas.DrawRect(bounds, paint);
        }
    }
}