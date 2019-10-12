using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using FaceCrop.Droid.Services;
using ViewModels.Serivces;
using Xamarin.Forms;


[assembly: Dependency(typeof(PhotoGalleryService))]

namespace FaceCrop.Droid.Services
{
    public class PhotoGalleryService : IPhotoGalleryService
    {
        //public PhotoGalleryService()
        //{

        //}

        public Task<Stream> GetImageStreamAsync()
        {
            // Define the Intent for getting images
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);

            var activityContext = Xamarin.Forms.Forms.Context as MainActivity;
            // Start the picture-picker activity (resumes in MainActivity.cs)
            activityContext.StartActivityForResult(
                Intent.CreateChooser(intent, "Select Picture"),
                MainActivity.PickImageId);

            // Save the TaskCompletionSource object as a MainActivity property
            activityContext.PickImageTaskCompletionSource = new TaskCompletionSource<Stream>();

            // Return Task object
            return activityContext.PickImageTaskCompletionSource.Task;
        }
    }
}
