using BitrateCalculation.Services;

Console.WriteLine("==== Bitrate Calculation ====");

// Create sample data provider, for this exercise it reads JSON files
var payloadProvider = new SampleDevicePayloadProvider();
var client = new DeviceClient(payloadProvider);

// Poll for the sample data size count
// In the exercise only 1 sample datapoint given for the time snapshot, 
// extra payloads are created to simulate and demonstrate the polling frequency and the bitrate changes
var pollingRateHz = 2.0;
var pollCount = 3;
var (device, bitrates) = await client.BeginPollingAsync(pollingRateHz, pollCount);

Console.WriteLine($"[Started] Device: {device?.Device} | " +
    $"Model: {device?.Model} | " +
    $"Polling: {pollingRateHz} Hz ({1000.0 / pollingRateHz:N0} ms interval), {pollCount} polls"
);

foreach (var nic in device?.NIC ?? [])
{
    var rxFormatted = BitrateCalculator.FormatBits(nic.RxBits);
    var txFromatted = BitrateCalculator.FormatBits(nic.TxBits);

    Console.WriteLine($"[Initial] {nic.Timestamp:O} " +
        $"NIC: {nic.Description} | " +
        $"Rx: {nic.Rx:N0} octets ({nic.RxBits:N0} b, {rxFormatted}) | " +
        $"Tx: {nic.Tx:N0} octets ({nic.TxBits:N0} b, {txFromatted})"
    );
}

// Iterate through the simulated polling results for to show the bitrate evolution over time
await foreach (var results in bitrates)
{
    foreach (var result in results)
    {
        var rxBitsFormatted = BitrateCalculator.FormatBits(result.RxBits);
        var txBitsFormatted = BitrateCalculator.FormatBits(result.TxBits);
        var rxRateFormatted = BitrateCalculator.FormatBitrate(result.RxBitsPerSecond);
        var txRateFormatted = BitrateCalculator.FormatBitrate(result.TxBitsPerSecond);

        Console.WriteLine($"[Polling] {result.Timestamp:O} " +
            $"NIC: {result.NicDescription} | " +
            $"Rx: {result.RxOctets:N0} octets ({result.RxBits:N0} b " +
            $"{rxBitsFormatted}) Rate: {result.RxBitsPerSecond:N0} bps, {rxRateFormatted} | " +
            $"Tx: {result.TxOctets:N0} octets ({result.TxBits:N0} b, " +
            $"{txBitsFormatted}) Rate: {result.TxBitsPerSecond:N0} bps, {txRateFormatted}"
        );
    }
}

Console.WriteLine("==== Program End ====");
