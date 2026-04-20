using System.Text.Json.Serialization;

using BitrateCalculation.Services;

namespace BitrateCalculation.Models.Payloads;

public class NIC
{
    public string Description { get; set; } = string.Empty;

    public string MAC { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Received octets (1 octet = 8 bits)
    /// </summary>
    [JsonConverter(typeof(StringToLongConverter))]
    public long Rx { get; set; }

    /// <summary>
    /// Transmitted octets (1 octet = 8 bits)
    /// </summary>
    [JsonConverter(typeof(StringToLongConverter))]
    public long Tx { get; set; }

    [JsonIgnore]
    public double RxBits => Rx * BitrateCalculator.BitsPerOctet;

    [JsonIgnore]
    public double TxBits => Tx * BitrateCalculator.BitsPerOctet;
}
