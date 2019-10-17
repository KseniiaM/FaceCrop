using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using ViewModels.Services;
using Xamarin.Forms;

namespace ViewModels.ViewModels
{
    public class StartViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService navigationService;

        private bool isMediaInitialized;

        public ICommand PickFromGalleryCommand => new Command(PickFromGalleryCommandExecute);
        public ICommand TakePhotoCommand => new Command(TakePhotoCommandExecute);

        public StartViewModel(IMvxNavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        private async void PickFromGalleryCommandExecute(object parameter)
        {
            await DisplayImageOnNewPage(true);
        }

        private async void TakePhotoCommandExecute(object parameter)
        {
            await DisplayImageOnNewPage(false);
        }

        private async Task DisplayImageOnNewPage(bool getFromGallery)
        {
            await InitMedia();
            var selectedImage = getFromGallery ? await CrossMedia.Current.PickPhotoAsync()
                                               : await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());

            //user did not select anything so image = null
            if (selectedImage != null)
            {
                await navigationService.Navigate<DisplayImagesViewModel, MediaFile>(selectedImage);
            }
        }

        private async Task InitMedia()
        {
            if (!isMediaInitialized)
            {
                await CrossMedia.Current.Initialize();
                isMediaInitialized = true;
            }
        }
    }
}
