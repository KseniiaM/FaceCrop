using System.IO;
using System.Windows.Input;
using MvvmCross.ViewModels;
using ViewModels.Serivces;
using Xamarin.Forms;

namespace ViewModels.ViewModels
{
    public class StartViewModel : MvxViewModel
    {
        public ImageSource SelectedImageSource { get; private set; }

        public ICommand PickFromGalleryCommand => new Command(PickFromGalleryCommandExecute);

        public ICommand TakePhotoCommand => new Command(TakePhotoCommandExecute);

        private async void PickFromGalleryCommandExecute(object parameter)
        {
            Stream stream = await DependencyService.Get<IPhotoGalleryService>().GetImageStreamAsync();
            if (stream != null)
            {
                SelectedImageSource = ImageSource.FromStream(() => stream);
            }
        }

        private void TakePhotoCommandExecute(object parameter)
        {

        }
    }
}
