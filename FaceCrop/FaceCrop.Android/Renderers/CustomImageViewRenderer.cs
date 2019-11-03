using Android.Content;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using FaceCrop.Droid.Renderers;
using Android.Graphics;
using FaceCrop.CustomControls;
using Android.Graphics.Drawables;
using ViewModels.Models;
using System.Linq;
using FaceCrop.Droid.Services;
using System.ComponentModel;
using Android.Views;

[assembly: ExportRenderer(typeof(CustomImageView), typeof(CustomImageViewRenderer))]
namespace FaceCrop.Droid.Renderers
{
    class CustomImageViewRenderer : ImageRenderer
    {
        private CustomImageView element;
        private FaceRectangleModel selectedRectangle;
        private Bitmap originalImage;

        private FaceRectangleModel SelectedRectangle
        {
            get
            {
                return selectedRectangle;
            }
            set
            {
                selectedRectangle = value;

                if(selectedRectangle != null)
                {
                    DrawDarkenFrame();
                }
            }
        }

        private Bitmap OriginalImage
        {
            get
            {
                if (originalImage == null)
                {
                    originalImage = ((BitmapDrawable)Control.Drawable).Bitmap;
                }

                return originalImage;
            }
        }

        private readonly DrawingService drawingService;

        public CustomImageViewRenderer(Context context) : base(context) 
        {
            drawingService = new DrawingService();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && Control != null)
            {
                element = e.NewElement as CustomImageView;
                Control.Touch += OnImageClicked;
            }
        }

        private RectF ConvertSelectedRectangleModelToRectF()
        {
            return new RectF(SelectedRectangle.Left, 
                             SelectedRectangle.Top, 
                             SelectedRectangle.Left + SelectedRectangle.Width, 
                             SelectedRectangle.Top + SelectedRectangle.Height);
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

        private void OnImageClicked(object sender, TouchEventArgs e)
        {
            var point = GetClickPointAsBitmapCoordinates(e);

            //a valid point on screen was selected (not bounds)
            if (point != Xamarin.Forms.Point.Zero)
            {
                SelectedRectangle = FindClickedRectangle(point);
            }
        }

        private void DrawDarkenFrame()
        {
            var rect = ConvertSelectedRectangleModelToRectF();
            var mask = drawingService.CreateHalfTrasparentBitmap(OriginalImage, rect);
            var result = drawingService.MergeBitmaps(OriginalImage, mask);
            Control.SetImageDrawable(new BitmapDrawable(result));
        }

        private FaceRectangleModel FindClickedRectangle(Xamarin.Forms.Point point)
        {
            return element.RectangleCollection.FirstOrDefault(rect =>
                        IsWithinSelectedRange(point.X, rect.Left, rect.Left + rect.Width) &&
                        IsWithinSelectedRange(point.Y, rect.Top, rect.Top + rect.Height));
        }

        #region clickParser
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

        private bool IsWithinSelectedRange(double value, int lowerRange, int upperRange)
        {
            return value >= lowerRange && value <= upperRange;
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
        #endregion
    }
}