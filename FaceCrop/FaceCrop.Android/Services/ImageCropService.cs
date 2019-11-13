using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
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
        public async Task<ImageSource> CropImages(StreamImageSource imageSource, FaceRectangleModel face)
        {
            
            var bitmap = await BitmapUtils.ConvertImageSourceToBitmap(imageSource);

            var cropped = Bitmap.CreateBitmap(bitmap,
                                              ValidateRectStartPointCoordinate(face.Left),
                                              ValidateRectStartPointCoordinate(face.Top),
                                              ValidateReccToLengthPointCoordinate(face.Left, face.Width, bitmap.Width),
                                              ValidateReccToLengthPointCoordinate(face.Top, face.Height, bitmap.Height));

            return BitmapUtils.ConvertBitmapToImageSource(cropped);
        }

        public async Task<ImageSource> DrawFaceRactangles(StreamImageSource image, List<FaceRectangleModel> faces)
        {
            var bitmap = await BitmapUtils.ConvertImageSourceToBitmap(image);
            var bitmapMutable = BitmapUtils.ConvertToMutableBitmap(bitmap);

            bitmapMutable.SetConfig(Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(bitmapMutable);

            foreach (var face in faces)
            {
                DrawRectangle(face, canvas);
            }

            return BitmapUtils.ConvertBitmapToImageSource(bitmapMutable);
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

        private int ValidateRectStartPointCoordinate(int coordinateValue)
        {
            return coordinateValue > 0 ? coordinateValue : 0;
        }

        private int ValidateReccToLengthPointCoordinate(int startCoordinate, int length, int biggestAllowedValue)
        {
            if(startCoordinate <= 0)
            {
                if ((length + startCoordinate) > biggestAllowedValue)
                {
                    return biggestAllowedValue - 1;
                }

                return length + startCoordinate;
            }

            return (startCoordinate + length) >= biggestAllowedValue ? (biggestAllowedValue - startCoordinate - 1) : length;
        }
    }
}