using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Json.Net;
using Plugin.Media.Abstractions;
using ViewModels.Models;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace ViewModels.Services
{
    class RestService : IRestService
    {
        private const string BaseUrl = "https://apicloud-facerect.p.rapidapi.com/process-url.json?url=";

        private HttpClient httpClient;

        private HttpClient HttpClient
        {
            get
            {
                if(httpClient == null)
                {
                    httpClient = new HttpClient();
                }

                return httpClient;
            }
        }

        public async Task<HttpResponseMessage> GetImageDataFromUrl(string url)
        {
            //HttpClient.DefaultRequestHeaders.Add("x-rapidapi-host", "apicloud-facerect.p.rapidapi.com");
            //HttpClient.DefaultRequestHeaders.Add("x-rapidapi-key", "53059c6510msh349a754b5b95dcbp1b10c8jsn3a2a57394663");
            //HttpClient.BaseAddress = new Uri("https://apicloud-facerect.p.rapidapi.com/process-url.json");
            //var d = HttpClient.DefaultRequestHeaders;
           
            HttpClient.DefaultRequestHeaders.Add("x-rapidapi-host", "apicloud-facerect.p.rapidapi.com");
            HttpClient.DefaultRequestHeaders.Add("x-rapidapi-key", "53059c6510msh349a754b5b95dcbp1b10c8jsn3a2a57394663");
            HttpClient.DefaultRequestHeaders.Add("content-type", "multipart/form-data");

            var response = await httpClient.GetAsync("BaseUrl");
            var byteArray = await response.Content.ReadAsStringAsync();

            ////var byteArray = buffer.ToArray();
            //var responseString = Encoding.UTF7.GetString(byteArray, 0, byteArray.Length);
            //var strings = await response.Content.ReadAsStringAsync();
            //var fff = Convert.ToBase64String(strings);
            //var re = JsonNet.Deserialize<FacesModel>(fff);
            return response;
        }

        public async Task<HttpResponseMessage> GetImageDataFromMediaFile1(MediaFile img)
        {
            HttpClient.DefaultRequestHeaders.Add("x-rapidapi-host", "apicloud-facerect.p.rapidapi.com");
            HttpClient.DefaultRequestHeaders.Add("x-rapidapi-key", "53059c6510msh349a754b5b95dcbp1b10c8jsn3a2a57394663");
            //HttpClient.DefaultRequestHeaders.Add("content-type", "multipart/form-data");
            var d = img.GetStream();//GetStreamWithImageRotatedForExternalStorage();
            d.Flush();
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(d), "batat");

            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data");
            HttpClient.BaseAddress = new Uri("https://apicloud-facerect.p.rapidapi.com/process-file.json");
            var response = await httpClient.PostAsync("https://apicloud-facerect.p.rapidapi.com/process-file.json", content);
            var byteArray = await response.Content.ReadAsStringAsync();
            return response;
        }

        public async Task<HttpResponseMessage> GetImageDataFromMediaFile(MediaFile img)
        {
            HttpClient.DefaultRequestHeaders.Add("x-rapidapi-host", "apicloud-facerect.p.rapidapi.com");
            HttpClient.DefaultRequestHeaders.Add("x-rapidapi-key", "53059c6510msh349a754b5b95dcbp1b10c8jsn3a2a57394663");
            //HttpClient.DefaultRequestHeaders.Add("content-type", "multipart/form-data");
            //var d = img.GetStream();//GetStreamWithImageRotatedForExternalStorage();
            //d.Flush();
            //var content = new MultipartFormDataContent();
            //content.Add(new StreamContent(d), "batat");

            //content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data");
            //HttpClient.BaseAddress = new Uri("https://apicloud-facemark.p.rapidapi.com/");

            byte[] coded;
            using (var memoryStream = new MemoryStream())
            {
                img.GetStream().CopyTo(memoryStream);
                img.Dispose();
                coded = memoryStream.ToArray();
            }

            using (ByteArrayContent content = new ByteArrayContent(coded))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json"
                // and "multipart/form-data".
                content.Headers.ContentType =
                    new MediaTypeHeaderValue("multipart/form-data");

                var response = await httpClient.PostAsync("https://apicloud-facerect.p.rapidapi.com/process-file.json", content);
                var byteArray = await response.Content.ReadAsStringAsync();
                return response;
            }
        }
    }
}
