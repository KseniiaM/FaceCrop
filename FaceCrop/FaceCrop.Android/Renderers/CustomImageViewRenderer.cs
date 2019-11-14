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
using Android.Views;
using static Android.Views.ScaleGestureDetector;
using FaceCrop.Droid.Utils;
using System.Threading.Tasks;

[assembly: ExportRenderer(typeof(CustomImageView), typeof(CustomImageViewRenderer))]
namespace FaceCrop.Droid.Renderers
{
    class CustomImageViewRenderer : ImageRenderer, IOnScaleGestureListener
    {
        private CustomImageView element;
        private double bitmapScaleRatio;
        private Bitmap currentBitmapSource;
        private FaceRectangleModel previousRectangle;

        private readonly DrawingService drawingService;
        private ScaleGestureDetector scaleGestureDetector;

        private FaceRectangleModel SelectedRectangle
        {
            get
            {
                return element.SelectedRectangle;
            }
            set
            {
                element.SelectedRectangle = value;
            }
        }

        public CustomImageViewRenderer(Context context) : base(context)
        {
            drawingService = new DrawingService();
            scaleGestureDetector = new ScaleGestureDetector(context, this);
        }
        
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && Control != null)
            {
                element = e.NewElement as CustomImageView;
                
                element.SelectedRectangleChanged += SelectedRectangleChangedHandler;
                Control.Touch += OnImageClicked;
            }
        }

        private async void SelectedRectangleChangedHandler()
        {
            await ResolveNewImageSource();

            if (SelectedRectangle != null)
            {
                DrawDarkenFrame();
            }
        }

        private async Task ResolveNewImageSource()
        {
            if (SelectedRectangle == null)
            {
                var newSource = element.ImageWithRectangles;
                var bitmap = await BitmapUtils.ConvertImageSourceToBitmap((StreamImageSource)newSource);
                Control.SetImageDrawable(new BitmapDrawable(bitmap));
            }
            if (SelectedRectangle != null && previousRectangle == null)
            {
                var newSource = element.ImageWithoutRectangles;
                previousRectangle = SelectedRectangle;
                currentBitmapSource = await BitmapUtils.ConvertImageSourceToBitmap((StreamImageSource)newSource);
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
                    element.SelectedRectangleChanged -= SelectedRectangleChangedHandler;
                }
            }

            base.Dispose(disposing);
        }

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            if(SelectedRectangle != null)
            {
                scaleGestureDetector.OnTouchEvent(e);
                HandleDragAction(e);
            }
            
            return base.DispatchTouchEvent(e);
        }

        private void OnImageClicked(object sender, TouchEventArgs e)
        {
            var point = GetClickPointAsBitmapCoordinates(e);

            //a valid point on screen was selected (not bounds)
            if (point != Xamarin.Forms.Point.Zero && SelectedRectangle == null)
            {
                SelectedRectangle = FindClickedRectangle(point);
            }
        }

        private void DrawDarkenFrame()
        {
            var rect = ConvertSelectedRectangleModelToRectF();
            var mask = drawingService.CreateHalfTrasparentBitmap(currentBitmapSource, rect);
            var result = drawingService.MergeBitmaps(currentBitmapSource, mask);
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

            bitmapScaleRatio = ResolveRatioOfBitmapScaling(bitmap, out isResolvedOnHeight);

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

        private double ResolveRatioOfBitmapScaling(Bitmap bitmap, out bool isResolvedOnHeight)
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
                return (double)Control.Width / (double)bitmap.Width;
            }
        }

        private Xamarin.Forms.Point HandleClickForHeightBoundsImage(Bitmap bitmap, double scale, TouchEventArgs args)
        {
            var bitmapHeight = bitmap.Height * scale;
            var offset = (Control.Height - bitmapHeight) / 2.0d;

            var yCoord = (double) args.Event.GetY();
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

        private Xamarin.Forms.Point HandleClickForWidthBoundsImage(Bitmap bitmap, double scale, TouchEventArgs args)
        {
            var bitmapWidth = bitmap.Width * scale;
            var offset = (Control.Width - bitmapWidth) / 2.0d;

            var xCoord = (double) args.Event.GetX();
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


        #region FrameMoving
        public bool OnScale(ScaleGestureDetector detector)
        {
            if (detector.ScaleFactor != 1)
            {
                var differenceX = CountScaleDifference(detector.CurrentSpanX, detector.PreviousSpanX);
                var differenceY = CountScaleDifference(detector.CurrentSpanY, detector.PreviousSpanY);

                var rect = new FaceRectangleModel()
                {
                    Left = SelectedRectangle.Left - differenceX / 2,
                    Top = SelectedRectangle.Top - differenceY / 2,
                    Width = SelectedRectangle.Width + differenceX,
                    Height = SelectedRectangle.Height + differenceY
                };

                SelectedRectangle = rect;
            }

            return true;
        }

        public bool OnScaleBegin(ScaleGestureDetector detector)
        {
            return true;
        }

        public void OnScaleEnd(ScaleGestureDetector detector)
        {
        }

        private void OnDrag(int oldX, int newX, int oldY, int newY)
        {
            var xDiff = newX - oldX;
            var yDiff = newY - oldY;

            var rect = new FaceRectangleModel()
            {
                Left = SelectedRectangle.Left + xDiff,
                Top = SelectedRectangle.Top + yDiff,
                Width = SelectedRectangle.Width,
                Height = SelectedRectangle.Height
            };

            SelectedRectangle = rect;
        }

        private int CountScaleDifference(float newDistance, float oldDistance)
        {
            return  (int) (newDistance / bitmapScaleRatio - oldDistance / bitmapScaleRatio);
        }

        int previousX = 0;
        int previousY = 0;

        public void HandleDragAction(MotionEvent e)
        {
            var newX = 0;
            var newY = 0;

            switch(e.Action)
            {
                case MotionEventActions.Down:
                {
                    previousX = (int) (e.GetX() / bitmapScaleRatio);
                    previousY = (int) (e.GetY() / bitmapScaleRatio);

                    break;
                }
                case MotionEventActions.Move:
                case MotionEventActions.Up:
                {
                    newX = (int)(e.GetX() / bitmapScaleRatio);
                    newY = (int)(e.GetY() / bitmapScaleRatio);

                    if ((previousX == newX && previousY == newY) || (previousX == 0 && previousY == 0))
                    {
                        return;
                    }

                    OnDrag(previousX, newX, previousY, newY);

                    previousX = newX;
                    previousY = newY;

                    break;
                }
            }
        }
        #endregion
    }
}