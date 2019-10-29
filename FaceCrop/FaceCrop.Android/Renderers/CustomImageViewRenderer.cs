using Android.Content;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using FaceCrop.Droid.Renderers;
using System.ComponentModel;
using Android.Graphics;
using FaceCrop.CustomControls;
using Android.Graphics.Drawables;
using ViewModels.Models;
using System.Collections.Generic;

[assembly: ExportRenderer(typeof(CustomImageView), typeof(CustomImageViewRenderer))]
namespace FaceCrop.Droid.Renderers
{
    class CustomImageViewRenderer : ImageRenderer
    {
        private List<FaceRectangleModel> rectangles;

        public CustomImageViewRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && Control != null)
            {
                Control.Touch += OnImageClicked;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Control != null)
                {
                    Control.Touch -= OnImageClicked;
                }
            }

            base.Dispose(disposing);
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            //TODO it is never set, find out why and fix
            if (e.PropertyName == CustomImageView.RectangleCollectionProperty.PropertyName)
            {
                rectangles = ((CustomImageView)sender).RectangleCollection;
            }
        }

        private void OnImageClicked(object sender, TouchEventArgs e)
        {
            var point = GetClickPointAsBitmapCoordinates(e);
        }

        private Xamarin.Forms.Point GetClickPointAsBitmapCoordinates(TouchEventArgs e)
        {
            Bitmap bitmap = ((BitmapDrawable)Control.Drawable).Bitmap;
            bool isResolvedOnHeight;
            int bitmapScaleRatio = ResolveRatioOfBitmapScaling(bitmap, out isResolvedOnHeight);

            if (isResolvedOnHeight)
            {
                return HandleClickForWidthBoundsImage(bitmap, bitmapScaleRatio, e);
            }
            else
            {
                return HandleClickForHeightBoundsImage(bitmap, bitmapScaleRatio, e);
            }
        }

        private float CalculateRatio(int width, int height)
        {
            return height / width;
        }

        private int ResolveRatioOfBitmapScaling(Bitmap bitmap, out bool isResolvedOnHeight)
        {
            var bitmapRatio = CalculateRatio(bitmap.Width, bitmap.Height);
            var controlRatio = CalculateRatio(Control.Width, Control.Height);

            //width has bounds added, then count using height
            if (bitmapRatio > controlRatio)
            {
                isResolvedOnHeight = true;
                return Control.Height / bitmap.Height;
            }
            else
            {
                isResolvedOnHeight = false;
                return Control.Width / bitmap.Width;
            }
        }

        private Xamarin.Forms.Point HandleClickForHeightBoundsImage(Bitmap bitmap, int scale, TouchEventArgs args)
        {
            var bitmapHeight = bitmap.Height * scale;
            var offset = (Control.Height - bitmapHeight) / 2.0f;

            var yCoord = args.Event.GetY();
            if (yCoord < offset || yCoord > (bitmapHeight + offset))
            {
                //Click out of bitmap bounds
                return Xamarin.Forms.Point.Zero;
            }

            yCoord -= offset;
            var xCoord = args.Event.GetX();

            //now convert to bitmap coord
            return new Xamarin.Forms.Point(xCoord / scale, yCoord / scale);
        }

        private Xamarin.Forms.Point HandleClickForWidthBoundsImage(Bitmap bitmap, int scale, TouchEventArgs args)
        {
            var bitmapWidth = bitmap.Width * scale;
            var offset = (Control.Width - bitmapWidth) / 2.0f;

            var xCoord = args.Event.GetX();
            if (xCoord < offset || xCoord > (bitmapWidth + offset))
            {
                //Click out of bitmap bounds
                return Xamarin.Forms.Point.Zero;
            }

            xCoord -= offset;
            var yCoord = args.Event.GetY();

            //now convert to bitmap coord
            return new Xamarin.Forms.Point(xCoord / scale, yCoord / scale);
        }
    }
}