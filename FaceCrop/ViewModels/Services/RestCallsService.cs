using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Services
{
    class RestCallsService : IRestCallsService
    {
        const string subscriptionKey = "5f4794c78f6f4326aa4ec9c35f1a13c3";
        const string uriBase = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/detect";

        private HttpClient httpClient;

        private HttpClient HttpClient
        {
            get
            {
                if (httpClient == null)
                {
                    httpClient = new HttpClient();
                }

                return httpClient;
            }
        }

        public async Task<string> AnalyzeFace(MediaFile img)
        {
            HttpClient.DefaultRequestHeaders.Add(
                "Ocp-Apim-Subscription-Key", subscriptionKey);
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false" +
                "&returnFaceAttributes=age,"+
                "emotion";
            byte[] coded;
            using (var memoryStream = new MemoryStream())
            {
                img.GetStream().CopyTo(memoryStream);
                img.Dispose();
                coded = memoryStream.ToArray();
            }

            string uri = uriBase + "?" + requestParameters;

            using (ByteArrayContent content = new ByteArrayContent(coded))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json"
                // and "multipart/form-data".
                content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                var response = await HttpClient.PostAsync(uri, content);

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();
                var rez = JsonConvert.DeserializeObject(contentString);
                var jarr = rez as JArray;
                var rect = jarr["faceRectangle"];
                return contentString;
            }
        }
    }
}
