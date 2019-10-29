using MvvmCross.ViewModels;
using Plugin.Media.Abstractions;
using System;
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
        public ICommand FaceSelectedCommand => new Command(FaceSelectedCommandExecute);

        private void FaceSelectedCommandExecute(object obj)
        {
            
        }

        //private List<ImageSource> selectedImageSources;

        //public List<ImageSource> SelectedImageSources
        //{
        //    get => selectedImageSources;
        //    set => SetProperty(ref selectedImageSources, value);
        //}

        //public bool IsProcessingPicture
        //{
        //    get => isProcessingPicture;
        //    set => SetProperty(ref isProcessingPicture, value);
        //}

        private ImageSource selectedFaces;
        private List<FaceRectangleModel> rectangles;

        public ImageSource SelectedFaces
        {
            get => selectedFaces;
            set => SetProperty(ref selectedFaces, value);
        }

        public List<FaceRectangleModel> Rectangles
        {
            get => rectangles;
            set => SetProperty(ref rectangles, value);
        }

        public DisplayImagesViewModel()
        {
        }

        public override void Prepare(FaceRectangleCollectionModel parameter)
        {
            SelectedFaces = parameter.OriginalImage;
            Rectangles = parameter.RectangleModels;
        }
    }
}
