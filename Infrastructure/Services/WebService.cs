using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class WebService : IWebService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
  
    public WebService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    public bool CheckHeaderValue(string headerName, string valueCheck)
    {
        var headers = _httpContextAccessor.HttpContext?.Request.Headers;
        return headers != null && headers[$"{headerName}"] == valueCheck;
    }

    public string GetHeader(string headerName)
    {
        var headers = _httpContextAccessor.HttpContext?.Request.Headers;
        if (headers != null)
        {
            return headers[$"{headerName}"];
        }

        return "";
    }
    
    public string GetStringSession(string key)
    {
        var data = _httpContextAccessor.HttpContext?.Session.GetString(key);

        return data ?? string.Empty;
    }
    
    public void SetStringSession(string key, string value)
    {
        _httpContextAccessor.HttpContext?.Session.SetString(key, value);
    }
    
    public void ClearSession(string key)
    {
        _httpContextAccessor.HttpContext?.Session.Remove(key);
    }
    
    public void ClearAllSession()
    {
        _httpContextAccessor.HttpContext?.Session.Clear();
    }
}