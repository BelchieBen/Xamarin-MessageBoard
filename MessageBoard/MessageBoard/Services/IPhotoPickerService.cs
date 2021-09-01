using System.IO;
using System.Threading.Tasks;

namespace MessageBoard.Services
{
    public interface IPhotoPickerService
    {
        Task<Stream> GetImageStreamAsync();
    }
}
