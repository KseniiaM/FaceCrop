using MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Windows.Input;
using ViewModels.Models;
using Xamarin.Forms;

namespace ViewModels.ViewModels
{
    class DisplayImagesViewModel : MvxViewModel<FaceRectangleCollectionModel>
    {
        public ICommand RefreshSelectionCommand => new Command(RefreshSelectionCommandExecute);
        public override void Prepare(FaceRectangleCollectionModel parameter)
        {
            SelectedFaces = parameter.OriginalImage;
            Rectangles = parameter.RectangleModels;
        }

        private ImageSource selectedFaces;
        private List<FaceRectangleModel> rectangles;
        private FaceRectangleModel selectedRectangle;

        public ImageSource SelectedFaces
        {
            get => selectedFaces;
            set => SetProperty(ref selectedFaces, value);
        }

        public FaceRectangleModel SelectedRectangle
        {
            get => selectedRectangle;
            set => SetProperty(ref selectedRectangle, value);
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
    }
}
