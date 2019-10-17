using MvvmCross.ViewModels;
using Plugin.Media.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModels.Services;
using Xamarin.Forms;

namespace ViewModels.ViewModels
{
    class DisplayImagesViewModel : MvxViewModel<MediaFile>
    {
        private readonly IRestCallsService restService;
        private readonly IJsonParserService jsonParserService;

        private List<ImageSource> selectedImageSources;
        private bool isProcessingPicture;
        private MediaFile sourceMediaFile;

        public List<ImageSource> SelectedImageSources
        {
            get => selectedImageSources;
            set => SetProperty(ref selectedImageSources, value);
        }

        public bool IsProcessingPicture
        {
            get => isProcessingPicture;
            set => SetProperty(ref isProcessingPicture, value);
        }

        public DisplayImagesViewModel(IRestCallsService restService,
                                      IJsonParserService jsonParserService)
        {
            this.restService = restService;
            this.jsonParserService = jsonParserService;
        }

        public override void Prepare(MediaFile parameter)
        {
            sourceMediaFile = parameter;
        }

        public async override Task Initialize()
        {
            //TODO move logic for getting json to previous screen
            await base.Initialize();
            IsProcessingPicture = true;
            var json = await restService.PostImageForFaceDetection(sourceMediaFile);
            var faces = jsonParserService.ConvertJsonToFaceModels(json);
            var croppedFaces = DependencyService.Get<IImageCropService>().CropImage(sourceMediaFile, faces);
            SelectedImageSources = croppedFaces;
            SelectedImageSources.AddRange(croppedFaces);
            IsProcessingPicture = false;
        }
    }
}
