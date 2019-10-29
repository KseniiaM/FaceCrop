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
using static Android.Graphics.Bitmap;

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

                return ConvertBitmapToImageSource(cropped);
            }).ToList();
        }

        public ImageSource DrawFaceRactangles(MediaFile mediaFile, List<FaceRectangleModel> faces)
        {
            var bitmap = ConvertToMutableBitmap(BitmapFactory.DecodeStream(mediaFile.GetStream()));
            Canvas canvas = new Canvas(bitmap);

            foreach (var face in faces)
            {
                DrawRectangle(face, canvas);
            }

            return ConvertBitmapToImageSource(bitmap);
        }

        private Bitmap ConvertToMutableBitmap(Bitmap bitmap)
        {
            Config bitmapConfig = bitmap.GetConfig();

            if (bitmapConfig == null)
            {
                bitmapConfig = Config.Argb8888;
            }

            return bitmap.Copy(bitmapConfig, true);
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

        private ImageSource ConvertBitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                return ImageSource.FromStream(() => new MemoryStream(stream.ToArray()));
            }
        }
    }
}