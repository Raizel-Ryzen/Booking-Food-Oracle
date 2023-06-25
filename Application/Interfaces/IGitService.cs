using Application.Common.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IGitService
{
    Task<List<GitResponseModel>> UploadImagesAsync(List<IFormFile> files, string folderSource);

    Task<GitResponseModel> UploadImageAsync(IFormFile file, string folderSource);
    Task<GitResponseModel> UploadImageAsync(MemoryStream ms, string folderSource);
}