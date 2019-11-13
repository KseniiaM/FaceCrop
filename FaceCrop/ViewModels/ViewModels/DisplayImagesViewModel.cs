using MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModels.Models;
using ViewModels.Services;
using Xamarin.Forms;

namespace ViewModels.ViewModels
{
    class DisplayImagesViewModel : MvxViewModel<FaceRectangleCollectionModel>
    {
        private ImageSource selectedFaces;
        private ImageSource croppedImage;
        private List<FaceRectangleModel> rectangles;
        private FaceRectangleModel selectedRectangle;
        private ImageSource originalImage;

        public ICommand RefreshSelectionCommand { get; set; }

        public ICommand CropCommand { get; set; }

        public DisplayImagesViewModel()
        {
            RefreshSelectionCommand = new Command(RefreshSelectionCommandExecute, 
                                                  () => SelectedRectangle != null);

            CropCommand = new Command(async () => await CropCommandExecute(),
                                            () => SelectedRectangle != null);
        }

        public override void Prepare(FaceRectangleCollectionModel parameter)
        {
            originalImage = parameter.OriginalImage;
            SelectedFaces = parameter.OriginalImage;
            Rectangles = parameter.RectangleModels;
        }

        public ImageSource SelectedFaces
        {
            get => selectedFaces;
            set => SetProperty(ref selectedFaces, value);
        }

        public ImageSource CroppedImage
        {
            get => croppedImage;
            set => SetProperty(ref croppedImage, value);
        }

        public FaceRectangleModel SelectedRectangle
        {
            get => selectedRectangle;
            set
            {
                SetProperty(ref selectedRectangle, value);
                ((Command)CropCommand).ChangeCanExecute();
                ((Command)RefreshSelectionCommand).ChangeCanExecute();
            }
        }

        public List<FaceRectangleModel> Rectangles
        {
            get => rectangles;
            set => SetProperty(ref rectangles, value);
        }

        private void RefreshSelectionCommandExecute()
        {
            SelectedRectangle = null;
        }

        private async Task CropCommandExecute()
        {
            CroppedImage = await DependencyService.Get<IImageCropService>()
                                                  .CropImages(originalImage as StreamImageSource, SelectedRectangle);
            SelectedRectangle = null;
        }
    }
}
