
using Android.Content;
using Android.Graphics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.Graphics.Bitmap;

namespace FaceCrop.Droid.Utils
{
    public static class BitmapUtils
    {
        public static Bitmap ConvertToMutableBitmap(Bitmap bitmap)
        {
            Config bitmapConfig = bitmap.GetConfig();

            if (bitmapConfig == null)
            {
                bitmapConfig = Config.Argb8888;
            }

            return bitmap.Copy(bitmapConfig, true);
        }

        public static ImageSource ConvertBitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                return ImageSource.FromStream(() => new MemoryStream(stream.ToArray()));
            }
        }

        public static async Task<Bitmap> ConvertImageSourceToBitmap(StreamImageSource image)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                var stream = await image.Stream(CancellationToken.None);
                var bitmap = BitmapFactory.DecodeStream(stream);
                return bitmap;
            }
        }
    }
}