using MvvmCross.ViewModels;
using ViewModels.Services;
using Xamarin.Forms;

namespace ViewModels.ViewModels
{
    class DisplayImagesViewModel : MvxViewModel<ImageSource>
    {
        private readonly IRestService restService;

        private ImageSource selectedImageSource;
        private bool isProcessingPicture;
        public ImageSource SelectedImageSource
        {
            get => selectedImageSource;
            set => SetProperty(ref selectedImageSource, value);
        }

        public bool IsProcessingPicture
        {
            get => isProcessingPicture;
            set => SetProperty(ref isProcessingPicture, value);
        }

        public DisplayImagesViewModel(IRestService restService)
        {
            this.restService = restService;
        }

        public override void Prepare(ImageSource parameter)
        {
            SelectedImageSource = parameter;
        }

        public async override void ViewAppearing()
        {
            base.ViewAppearing();
            IsProcessingPicture = true;
            //make a normal uri 
            var rez = await restService.GetImageDataFromUrl("dsadsa");
            var e = await rez.Content.ReadAsStringAsync();
            IsProcessingPicture = false;
         }
    }
}
