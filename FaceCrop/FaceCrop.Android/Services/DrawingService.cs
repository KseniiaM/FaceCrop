using Android.Graphics;
using FaceCrop.Droid.Services;
using System;
using Xamarin.Forms;
using static Android.Graphics.Bitmap;

[assembly: Dependency(typeof(DrawingService))]
namespace FaceCrop.Droid.Services
{
    public class DrawingService
    {
        private Paint darkenPaint;
        private Paint eraserPaint;
        private Canvas halfTransparentFrameCanvas;
        private Canvas mergeCanvas;

        private Canvas HalfTransparentFrameCanvas
        {
            get 
            { 
                if(halfTransparentFrameCanvas == null)
                {
                    halfTransparentFrameCanvas = new Canvas();
                }

                return halfTransparentFrameCanvas; 
            }
        }

        private Canvas MergeCanvas
        {
            get
            {
                if (mergeCanvas == null)
                {
                    mergeCanvas = new Canvas();
                }

                return mergeCanvas;
            }
        }

        public DrawingService()
        {
            darkenPaint = new Paint(PaintFlags.AntiAlias);
            darkenPaint.Color = new Android.Graphics.Color(0, 0, 0, 50);

            eraserPaint = new Paint(PaintFlags.AntiAlias);
            eraserPaint.Color = new Android.Graphics.Color(0, 0, 0, 0);
            eraserPaint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.Clear));
        }

        public Bitmap CreateHalfTrasparentBitmap(Bitmap bitmap, RectF selectedRectangle)
        {
            var destinationBitmap = Bitmap.CreateBitmap(bitmap.Width, bitmap.Height, Config.Argb8888);
            HalfTransparentFrameCanvas.SetBitmap(destinationBitmap);
            HalfTransparentFrameCanvas.DrawPaint(darkenPaint);
            HalfTransparentFrameCanvas.DrawRect(selectedRectangle, eraserPaint);
            return destinationBitmap;
        }

        public Bitmap MergeBitmaps(Bitmap bitmapSource, Bitmap mask)
        {
            var mergeBitmap = Bitmap.CreateBitmap(bitmapSource.Width, bitmapSource.Height, bitmapSource.GetConfig());
            MergeCanvas.SetBitmap(mergeBitmap);
            Paint paint = new Paint(PaintFlags.FilterBitmap);
            MergeCanvas.DrawBitmap(bitmapSource, 0, 0, paint);
            MergeCanvas.DrawBitmap(mask, 0, 0, paint);
            return mergeBitmap;
        }
    }
}