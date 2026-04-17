using BitrateCalculation.Services.Core;

namespace BitrateCalculation.Services;

public class SampleDevicePayloadProvider : IDevicePayloadProvider
{
    private static readonly string[] FileNames =
    [
        "payload_1.json",
        "payload_2.json",
        "payload_3.json",
    ];

    private int currentIndex = 0;

    public Task<Stream> GetPayloadAsync()
    {
        var i = Math.Min(currentIndex, FileNames.Length - 1);

        currentIndex++;

        var path = Path.Combine("data", FileNames[i]);
        var stream = File.OpenRead(path);
        return Task.FromResult<Stream>(stream);
    }
}
