namespace BitrateCalculation.Services.Core;

public interface IDevicePayloadProvider
{
    Task<Stream> GetPayloadAsync();
}
