using BitrateCalculation.Models;
using BitrateCalculation.Models.Payloads;

namespace BitrateCalculation.Services;

public class BitrateCalculator(double pollingRateHz)
{
    /// <summary>
    /// For octet is consist of 8 bits
    /// </summary>
    public const int DataUnitSize = 8;
    private readonly double pollIntervalSeconds = 1.0 / pollingRateHz;

    private static string Format(double value, string unit) => value switch
    {
        >= 1_000_000_000 => $"{value / 1_000_000_000:F3} G{unit}",
        >= 1_000_000 => $"{value / 1_000_000:F3} M{unit}",
        >= 1_000 => $"{value / 1_000:F3} K{unit}",
        _ => $"{value:F0} {unit}"
    };

    public static string FormatBitrate(double bps) => Format(bps, "bps");

    public static string FormatBits(double bits) => Format(bits, "b");

    public IEnumerable<BitrateResult> Calculate(IEnumerable<NIC> previousItems, IEnumerable<NIC> currentItems)
    {
        var currentByMac = currentItems.ToDictionary(n => n.MAC);

        foreach (var previous in previousItems)
        {
            if (!currentByMac.TryGetValue(previous.MAC, out var current))
            {
                Console.Error.WriteLine($"[BitrateCalculator] Warning: NIC with MAC {previous.MAC} ({previous.Description}) missing from current poll, skipping.");
                continue;
            }

            var deltaTime = current.Timestamp - previous.Timestamp;
            var deltaSeconds = deltaTime.TotalSeconds;
            if (deltaSeconds <= 0)
            {
                deltaSeconds = pollIntervalSeconds;
            }

            // If current < previous, assume the device reset its counter and treat current as the estimated delta since reset.
            var rxDelta = current.Rx < previous.Rx ? current.Rx : current.Rx - previous.Rx;
            var txDelta = current.Tx < previous.Tx ? current.Tx : current.Tx - previous.Tx;

            var output = new BitrateResult
            {
                NicDescription = current.Description,
                Timestamp = current.Timestamp,
                RxOctets = current.Rx,
                TxOctets = current.Tx,
                RxBitsPerSecond = rxDelta * DataUnitSize / deltaSeconds,
                TxBitsPerSecond = txDelta * DataUnitSize / deltaSeconds,
            };

            yield return output;
        }
    }
}
