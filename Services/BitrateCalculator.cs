using BitrateCalculation.Models;
using BitrateCalculation.Models.Payloads;

namespace BitrateCalculation.Services;

public class BitrateCalculator(double pollingRateHz)
{
    /// <summary>
    /// For octet is consist of 8 bits
    /// </summary>
    public const int BitsPerOctet = 8;
    public const long DefaultWrapValue = long.MaxValue;
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

    public static long CalculateOctetDelta(long current, long previous, long wrapValue)
    {
        if (current >= previous)
        {
            return current - previous;
        }

        // Counter wrapped, add extra 1 for cross the 0
        return wrapValue - previous + current + 1;
    }

    public static double CalculateBitrate(long currentOctets, long previousOctets, double deltaSeconds, long wrapValue)
    {
        var deltaOctets = CalculateOctetDelta(currentOctets, previousOctets, wrapValue);
        return deltaOctets * BitsPerOctet / deltaSeconds;
    }

    public IEnumerable<BitrateResult> Calculate(IEnumerable<NIC> previousItems, IEnumerable<NIC> currentItems, long wrapValue = DefaultWrapValue)
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

            yield return new BitrateResult
            {
                NicDescription = current.Description,
                Timestamp = current.Timestamp,
                RxOctets = current.Rx,
                TxOctets = current.Tx,
                RxBitsPerSecond = CalculateBitrate(current.Rx, previous.Rx, deltaSeconds, wrapValue),
                TxBitsPerSecond = CalculateBitrate(current.Tx, previous.Tx, deltaSeconds, wrapValue),
            };
        }
    }
}
