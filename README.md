# BitrateCalculation

C# .NET 10 console app that calculates network interface bitrates (Rx/Tx) from polled device payloads.

## What it does

- Reads sample device payloads (extra payloads are created to simulate and demonstrate the polling frequency and the bitrate changes)
- Polls them at a rate over time.
- Computes Rx/Tx bits per second per NIC between snapshots and prints a human-readable summary (bps, Kbps, Mbps, etc...).

## Run

```bash
dotnet run
```

Expected output:  

```log
==== Bitrate Calculation ====
[Started] Device: Arista | Model: X-Video | Polling: 2 Hz (500 ms interval), 3 polls
[Initial] 2020-03-23T18:25:43.5110000Z NIC: Linksys ABR | Rx: 3,698,574,500 octets (29,588,596,000 b, 29.589 Gb) | Tx: 122,558,800 octets (980,470,400 b, 980.470 Mb)
[Polling] 2020-03-23T18:25:44.0110000Z NIC: Linksys ABR | Rx: 3,699,199,500 octets (29,593,596,000 b 29.594 Gb) Rate: 10,000,000 bps, 10.000 Mbps | Tx: 122,871,300 octets (982,970,400 b, 982.970 Mb) Rate: 5,000,000 bps, 5.000 Mbps
[Polling] 2020-03-23T18:25:44.5110000Z NIC: Linksys ABR | Rx: 3,699,949,500 octets (29,599,596,000 b 29.600 Gb) Rate: 12,000,000 bps, 12.000 Mbps | Tx: 123,105,675 octets (984,845,400 b, 984.845 Mb) Rate: 3,750,000 bps, 3.750 Mbps
==== Program End ====
```
