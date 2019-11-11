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
        private List<FaceRectangleModel> rectangles;
        private FaceRectangleModel selectedRectangle;

        public ICommand RefreshSelectionCommand => new Command(RefreshSelectionCommandExecute);

        public override void Prepare(FaceRectangleCollectionModel parameter)
        {
            SelectedFaces = parameter.OriginalImage;
            Rectangles = parameter.RectangleModels;
        }

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
