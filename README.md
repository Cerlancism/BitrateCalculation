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
