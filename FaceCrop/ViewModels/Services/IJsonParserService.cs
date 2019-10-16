using System;
using System.Collections.Generic;
using ViewModels.Models;

namespace ViewModels.Services
{
    public interface IJsonParserService
    {
        List<FaceRectangleModel> ConvertJsonToFaceModels(string jsonString);
    }
}
