using Plugin.Media.Abstractions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
namespace ViewModels.Services
{
    class RestCallsService : IRestCallsService
    {
        //const string KeyHeader = "Ocp-Apim-Subscription-Key";
        //const string SubscriptionKey = "5f4794c78f6f4326aa4ec9c35f1a13c3";
        //const string UriBase = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/detect";
        //const string ContentType = "application/octet-stream";
        //const string requestParameters = "returnFaceId=true&returnFaceLandmarks=false" +
        //                                 "&returnFaceAttributes=age," +
        //                                 "emotion";

        const string KeyHeader = "user_id";
        const string SubscriptionKey = "5dcc575f70edbe0a8acdeb8c";
        const string UriBase = "http://www.facexapi.com/";
        const string ContentType = "multipart/form-data";
        const string requestParameters = "returnFaceId=true&returnFaceLandmarks=false" +
                                         "&returnFaceAttributes=age," +
                                         "emotion";

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

        public async Task<string> PostImageForFaceDetection(MediaFile img)
        {
            HttpClient.DefaultRequestHeaders.Add(KeyHeader, SubscriptionKey);
            var imageConverted = ByteUtils.ConvertMediaFileToByteArray(img);
            //string uri = UriBase + "?" + requestParameters;
            string uri = "http://www.facexapi.com/get_image_attr";

            using (MultipartFormDataContent content = new MultipartFormDataContent())
            {
                content.Headers.ContentType = new MediaTypeHeaderValue(ContentType);
                content.Add(new ByteArrayContent(imageConverted), "img");
                var response = await HttpClient.PostAsync(uri, content);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                return string.Empty;
            }
        }

        public async Task<string> PostImageForFaceDetection(MediaFile img)
        {
            HttpClient.DefaultRequestHeaders.Add(KeyHeader, SubscriptionKey);
            var imageConverted = ByteUtils.ConvertMediaFileToByteArray(img);
            //string uri = UriBase + "?" + requestParameters;
            string uri = "http://www.facexapi.com/get_image_attr";

            using (MultipartFormDataContent content = new MultipartFormDataContent())
            {
                content.Headers.ContentType = new MediaTypeHeaderValue(ContentType);
                content.Add(new ByteArrayContent(imageConverted), "img");
                var response = await HttpClient.PostAsync(uri, content);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                return string.Empty;
            }
        }
    }
}
