namespace Application.Interfaces;

public interface IQRCodeService
{
    Task<string> GenerateQRCodeAsync(string textOrUrl);
}