using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Services
{
    public interface IRestCallsService
    {
        Task<string> PostImageForFaceDetection(MediaFile img);
    }
}
