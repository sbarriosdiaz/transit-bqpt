////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Bqpt.Infrastructure
{
    public interface IFilesConnectedServices
    {
        string SetFileName(string fileName);

        Task<IEnumerable<BlobEnvelop>> FilesValidation(IEnumerable<FileValidationDto> payload, CancellationToken cancellationToken);

        Task<BlobEnvelop> SaveFile(HttpPostedFileBase file, string folderName, string fileName, CancellationToken cancellationToken);

        Task<BlobFileEnvelop> GetFile(string domainKey, CancellationToken cancellationToken = default);

        Task<bool> DeleteFile(string domainKey, CancellationToken cancellationToken);

        Task<bool> CleanAllFilesInFolder(CleanFolderDto payload, CancellationToken cancellationToken);
    }
}