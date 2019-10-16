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
        private readonly IRestCallsService restService;
        private readonly IJsonParserService jsonParserService;

        private bool isMediaInitialized;

        public ICommand PickFromGalleryCommand => new Command(PickFromGalleryCommandExecute);

        public ICommand TakePhotoCommand => new Command(TakePhotoCommandExecute);

        public StartViewModel(IMvxNavigationService navigationService,
                              IRestCallsService restService,
                              IJsonParserService jsonParserService)
        {
            this.restService = restService;
            this.navigationService = navigationService;
            this.jsonParserService = jsonParserService;
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

            var json = await restService.PostImageForFaceDetection(selectedImage);
            var faces = jsonParserService.ConvertJsonToFaceModels(json);
            //user did not select anything
            //if (selectedImage != null)
            //{
            //    var selectedImageSource = ImageSource.FromStream(() => selectedImage.GetStream());
            //    await navigationService.Navigate<DisplayImagesViewModel, ImageSource>(selectedImageSource);
            //}
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
