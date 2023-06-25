namespace Application.Interfaces;

public interface IWebService
{
    bool CheckHeaderValue(string headerName, string valueCheck);
    string GetHeader(string headerName);
    string GetStringSession(string key);
    void SetStringSession(string key, string value);
    void ClearSession(string key);
    void ClearAllSession();
}