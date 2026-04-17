using BitrateCalculation.Models;
using BitrateCalculation.Models.Payloads;

namespace BitrateCalculation.Services.Core;

public interface IDeviceClient
{
    Task<DevicePayload?> GetDevicePayloadAsync();
    Task<(DevicePayload? Device, IAsyncEnumerable<IEnumerable<BitrateResult>> Bitrates)> BeginPollingAsync(double pollingRateHz, int? pollCount = null);
}
