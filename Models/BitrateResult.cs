using BitrateCalculation.Services;

namespace BitrateCalculation.Models;

public class BitrateResult
{
    public string NicDescription { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public long RxOctets { get; set; }
    public long TxOctets { get; set; }
    public double RxBitsPerSecond { get; set; }
    public double TxBitsPerSecond { get; set; }

    public double RxBits => RxOctets * BitrateCalculator.DataUnitSize;
    public double TxBits => TxOctets * BitrateCalculator.DataUnitSize;
}
