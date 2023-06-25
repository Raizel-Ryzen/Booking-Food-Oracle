using Application.Interfaces;
using Domain.Enums;
using Domain.Helpers;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace Infrastructure.Services;

public class QRCodeService : IQRCodeService
{
    private readonly IConfiguration _configuration;
    private readonly IGitService _gitService;

    public QRCodeService(IConfiguration configuration, IGitService gitService)
    {
        _configuration = configuration;
        _gitService = gitService;
    }

    public async Task<string> GenerateQRCodeAsync(string textOrUrl)
    {
        var baseUrl = _configuration["QRCodeAPI:BaseURL"] +
                      $"/?data={textOrUrl}&size={_configuration["QRCodeAPI:DefaultSize"]}";
        var client = new RestClient(baseUrl);

        var request = new RestRequest()
        {
            Method = Method.Post
        };

        var bytes = await client.DownloadDataAsync(request);

        if (!(bytes?.Length > 0))
        {
            return "";
        }

        MemoryStream mem = new(bytes);
        var res = await _gitService.UploadImageAsync(mem, GitFolder.BookingFood.ReadDescription());
            
        return res.Content?.DownloadUrl ?? string.Empty;
    }
}