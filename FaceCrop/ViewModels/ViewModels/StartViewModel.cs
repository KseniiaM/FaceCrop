using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using ViewModels.Models;
using ViewModels.Services;
using Xamarin.Forms;

namespace ViewModels.ViewModels
{
    public class StartViewModel : MvxViewModel
    {
        private readonly IRestCallsService restService;
        private readonly IJsonParserService jsonParserService;
        private readonly IMvxNavigationService navigationService;

        private bool isMediaInitialized;

        public ICommand PickFromGalleryCommand => new Command(PickFromGalleryCommandExecute);
        public ICommand TakePhotoCommand => new Command(TakePhotoCommandExecute);

        public StartViewModel(IMvxNavigationService navigationService,
                              IRestCallsService restService,
                              IJsonParserService jsonParserService)
        {
            this.navigationService = navigationService;
            this.restService = restService;
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

            //user did not select anything so image = null
            if (selectedImage != null)
            {
                //TODO this one should be uncommented and work if you generate a new key
                //var json = await restService.PostImageForFaceDetection(selectedImage);

                var faces = jsonParserService.ReturnEmulatedFaceModels();

                var faceModel = new FaceRectangleCollectionModel()
                {
                    OriginalImage = ImageSource.FromStream(selectedImage.GetStream),
                    RectangleModels = faces
                };

                await navigationService.Navigate<DisplayImagesViewModel, FaceRectangleCollectionModel>(faceModel);
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
