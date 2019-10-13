using MvvmCross.ViewModels;
using Xamarin.Forms;

namespace ViewModels.ViewModels
{
    class DisplayImagesViewModel : MvxViewModel<ImageSource>
    {
        private ImageSource selectedImageSource;

        public ImageSource SelectedImageSource
        {
            get => selectedImageSource;
            set => SetProperty(ref selectedImageSource, value);
        }

        public override void Prepare(ImageSource parameter)
        {
            SelectedImageSource = parameter;
        }
    }
}
