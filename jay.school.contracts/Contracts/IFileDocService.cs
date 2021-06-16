using jay.school.contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace jay.school.contracts.Contracts
{
    public interface IFileDocService
    {
        Task<CustomResponse<FileDoc>> UploadFile(FileDoc fileDoc);
        Task<CustomResponse<MultipleFileDoc>> multipleFiles(MultipleFileDoc multipleFileDoc);
        Task<CustomResponse<SingleFileDoc>> SingleFiles(SingleFileDoc singleFileDoc)
    }
}
