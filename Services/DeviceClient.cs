using System.Text.Json;

using BitrateCalculation.Models;
using BitrateCalculation.Models.Payloads;
using BitrateCalculation.Services.Core;

namespace BitrateCalculation.Services;

public class DeviceClient(IDevicePayloadProvider provider) : IDeviceClient
{
    public async Task<DevicePayload?> GetDevicePayloadAsync()
    {
        await using var stream = await provider.GetPayloadAsync();
        var output = await JsonSerializer.DeserializeAsync<DevicePayload>(stream);
        return output;
    }

    /// <summary>
    /// Start polling the device
    /// </summary>
    /// <param name="pollingRateHz">Polling rate in Hz</param>
    /// <param name="pollCount">Number of time to poll, if not set, poll indefinitely</param>
    /// <returns></returns>
    public async Task<(DevicePayload? Device, IAsyncEnumerable<IEnumerable<BitrateResult>> Bitrates)> BeginPollingAsync(double pollingRateHz, int? pollCount = null)
    {
        try
        {
            var initial = await GetDevicePayloadAsync() ?? throw new Exception("[DeviceClient] Initial payload is null");
            var poll = Poll(initial, pollingRateHz, pollCount);
            var output = (initial, poll);
            return output;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[DeviceClient] Failed to fetch initial device payload: {ex}");
            throw;
        }
    }

    protected async IAsyncEnumerable<IEnumerable<BitrateResult>> Poll(DevicePayload initial, double pollingRateHz, int? pollCount)
    {
        var calculator = new BitrateCalculator(pollingRateHz);
        var pollIntervalMs = (int)(1000.0 / pollingRateHz);

        var previous = initial;
        var consumed = previous == null ? 0 : 1;

        while (previous != null && (pollCount == null || consumed < pollCount))
        {
            await Task.Delay(pollIntervalMs);

            DevicePayload? current = null;
            try
            {
                current = await GetDevicePayloadAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[DeviceClient] Failed to fetch device payload during polling: {ex}");
            }

            if (current == null)
            {
                continue;
            }

            consumed++;

            var progress = calculator.Calculate(previous.NIC, current.NIC);

            yield return progress;

            previous = current;
        }
    }
}
