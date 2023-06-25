using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace Infrastructure.Services;

public class GitService : IGitService
{
    private readonly IConfiguration _configuration;
    private readonly IIdentityService _identityService;

    public GitService(IConfiguration configuration, IIdentityService identityService)
    {
        _configuration = configuration;
        _identityService = identityService;
    }

    public async Task<List<GitResponseModel>> UploadImagesAsync(List<IFormFile> files, string folderSource)
    {
        var userName = _identityService.GetCurrentUserLogin().UserName ?? "code-mega.com";
        List<GitResponseModel> listResult = new();

        foreach (var file in files.Where(file => file.Length > 0))
        {
            await using MemoryStream ms = new();
            await file.CopyToAsync(ms);

            var fileBytes = ms.ToArray();
            var uniqueFileName = StringHelper.ImageFileName(file, userName);
            RestRequest request = new() { Method = Method.Put };

            request.AddHeader("Authorization", "Token " + _configuration["Github:AccessToken"]);
            request.AddHeader("accept", "application/vnd.github.v3+json");
            request.AddJsonBody(
                new
                {
                    message = StringHelper.GitMessageCommit(userName),
                    content = Convert.ToBase64String(fileBytes),
                    branch = _configuration["Github:Branch"]
                });

            RestClient client =
                new(
                    $"{_configuration["Github:BaseUrl"]}/{folderSource}/{uniqueFileName}");

            var response = await client.ExecuteAsync(request);
            var jsonGit = response.Content?.Replace("download_url", "downloadUrl");

            if (jsonGit == null)
            {
                continue;
            }

            var resGit = JsonConvert.DeserializeObject<GitResponseModel>(jsonGit);

            if (resGit != null)
            {
                listResult.Add(resGit);
            }
        }

        return listResult;
    }

    public async Task<GitResponseModel> UploadImageAsync(IFormFile file, string folderSource)
    {
        var userName = _identityService.GetCurrentUserLogin().UserName ?? "code-mega.com";

        await using MemoryStream ms = new();
        await file.CopyToAsync(ms);

        var fileBytes = ms.ToArray();
        var uniqueFileName = StringHelper.ImageFileName(file, userName);
        RestRequest request = new() { Method = Method.Put };

        request.AddHeader("Authorization", "Token " + _configuration["Github:AccessToken"]);
        request.AddHeader("accept", "application/vnd.github.v3+json");
        request.AddJsonBody(
            new
            {
                message = StringHelper.GitMessageCommit(userName),
                content = Convert.ToBase64String(fileBytes),
                branch = _configuration["Github:Branch"]
            });

        RestClient client =
            new(
                $"{_configuration["Github:BaseUrl"]}/{folderSource}/{uniqueFileName}");

        var response = await client.ExecuteAsync(request);
        var jsonGit = response.Content?.Replace("download_url", "downloadUrl");

        if (jsonGit == null)
        {
            return new GitResponseModel { Success = false };
        }

        var resGit = JsonConvert.DeserializeObject<GitResponseModel>(jsonGit);
        if (resGit == null)
        {
            return new GitResponseModel { Success = false };
        }

        resGit.Success = true;

        return resGit;
    }
    
    public async Task<GitResponseModel> UploadImageAsync(MemoryStream ms, string folderSource)
    {
        var userName = _identityService.GetCurrentUserLogin().UserName ?? "code-mega.com";

        var fileBytes = ms.ToArray();
        var uniqueFileName = userName + Guid.NewGuid().ToString().Split('-').First() + ".png";
        RestRequest request = new() { Method = Method.Put };

        request.AddHeader("Authorization", "Token " + _configuration["Github:AccessToken"]);
        request.AddHeader("accept", "application/vnd.github.v3+json");
        request.AddJsonBody(
            new
            {
                message = StringHelper.GitMessageCommit(userName),
                content = Convert.ToBase64String(fileBytes),
                branch = _configuration["Github:Branch"]
            });

        RestClient client =
            new(
                $"{_configuration["Github:BaseUrl"]}/{folderSource}/{uniqueFileName}");

        var response = await client.ExecuteAsync(request);
        var jsonGit = response.Content?.Replace("download_url", "downloadUrl");

        if (jsonGit == null)
        {
            return new GitResponseModel { Success = false };
        }

        var resGit = JsonConvert.DeserializeObject<GitResponseModel>(jsonGit);
        if (resGit == null)
        {
            return new GitResponseModel { Success = false };
        }

        resGit.Success = true;

        return resGit;
    }
}