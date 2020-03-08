using System.IO;
using System.Threading.Tasks;

namespace WebAdvert.Web.Interfaces
{
    public interface IFileUploader
    {
        Task<bool> UploadFileAsync(string fileName, Stream storageStream);
    }
}
