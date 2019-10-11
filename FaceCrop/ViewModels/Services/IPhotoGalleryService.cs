using System.IO;
using System.Threading.Tasks;

namespace ViewModels.Serivces
{
    public interface IPhotoGalleryService
    {
        Task<Stream> GetImageStreamAsync();
    }
}
