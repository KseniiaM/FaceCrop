using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Services
{
    public interface IRestService
    {
        Task<HttpResponseMessage> GetImageDataFromUrl(string url);

        Task<HttpResponseMessage> GetImageDataFromMediaFile(MediaFile img);
    }
}
