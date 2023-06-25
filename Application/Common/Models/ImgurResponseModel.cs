using System.Text.Json.Serialization;

namespace Application.Common.Models;

public class ImgurDetailModel
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("title")] public string Title { get; set; }

    [JsonPropertyName("description")] public string Description { get; set; }

    [JsonPropertyName("datetime")] public int Datetime { get; set; }

    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("deletehash")] public string DeleteHash { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("link")] public string Link { get; set; }
}

public class ImgurResponseModel
{
    [JsonPropertyName("data")] public ImgurDetailModel Data { get; set; }

    [JsonPropertyName("success")] public bool Success { get; set; }

    [JsonPropertyName("status")] public int Status { get; set; }
}