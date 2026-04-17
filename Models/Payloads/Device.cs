namespace BitrateCalculation.Models.Payloads;

public class DevicePayload
{
    public string Device { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public List<NIC> NIC { get; set; } = [];
}
