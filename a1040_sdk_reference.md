# A1040 SDK API Reference

This document provides an overview of the `A1040SDK` API.

## Overview

`A1040SDK` is the A1040-specific SDK wrapper built on top of `A10xSdk`.

Supported array configurations:

- `8x4`: single device, 32 channels
- `16x4`: paired master and slave devices, 64 channels
- `8x8`: paired master and slave devices, 64 channels

Use the connection method that matches the device layout:

- `Connect8x4(masterIp)`
- `Connect16x4(masterIp, slaveIp)`
- `Connect8x8(masterIp, slaveIp)`

## Constructor

```csharp
public A1040SDK(ILogger? logger = null)
```

Creates a new A1040 SDK instance.

## Properties

### MasterDeviceIP

```csharp
public string MasterDeviceIP { get; set; }
```

Stores the configured master device IP.

### SlaveDeviceIP

```csharp
public string SlaveDeviceIP { get; set; }
```

Stores the configured slave device IP for paired modes.

## A1040-Specific Events

### AscanDataReceived

```csharp
public event Action<short[], int, TimeSpan>? AscanDataReceived;
```

Raised when processed A-scan data arrives. The `int` argument is the number of samples, not bytes.

### SlaveBatteryInfoReceived

```csharp
public event Action<BatteryResult>? SlaveBatteryInfoReceived;
```

Raised when the slave battery response arrives in `16x4` or `8x8` mode.

## A1040-Specific Methods

### Connect8x4

```csharp
public void Connect8x4(string masterIp)
```

Starts communication with a single A1040 device in `8x4` mode.

### Connect16x4

```csharp
public void Connect16x4(string masterIp, string slaveIp)
```

Starts communication with a paired A1040 system in `16x4` mode.

### Connect8x8

```csharp
public void Connect8x8(string masterIp, string slaveIp)
```

Starts communication with a paired A1040 system in `8x8` mode.

### SetLaser

```csharp
public void SetLaser(bool enabled)
```

Controls the A1040 laser state.

### 16x4 Transmitter Selection

```csharp
public void SetSingle16x4Transmitter(int sensor)
public void SetQuadro16x4Transmitter(int sensor)
```

Selects the active transmitter for `16x4` acquisition.

### 8x8 Transmitter Selection

```csharp
public void SetSingle8x8Transmitter(int sensor)
public void SetQuadro8x8Transmitter(int sensor)
```

Selects the active transmitter for `8x8` acquisition.

### Acquisition Control

```csharp
public void StartAscanSingleTransmitter()
public void StartAscanQuadroTransmitter()
public void RequestSlaveBatteryInfo()
```

- `StartAscanSingleTransmitter()` starts A-scan acquisition using the current single-transmitter selection
- `StartAscanQuadroTransmitter()` starts A-scan acquisition using the current quadro-transmitter selection
- `RequestSlaveBatteryInfo()` requests battery status from the slave device in paired modes

## Common Inherited Members From A10xSdk

These inherited members are typically used together with `A1040SDK`:

```csharp
sdk.SetOperatingFrequency(50);
sdk.SetGain(10);
sdk.SetAscanAveraging(5);
sdk.SetPulseRepetitionRate(3);
sdk.SetPeriods(2.5f);

sdk.SetSingle8x4Transmitter(0);
sdk.SetQuadro8x4Transmitter(0);

sdk.RequestBatteryInfo();
sdk.StopAcquisition();
sdk.Disconnect();

bool connected = sdk.IsConnected;
bool cancelled = sdk.IsCommunicationCancelled;
```

Inherited events:

- `NetworkConnected`
- `NetworkDisconnected`
- `MasterBatteryInfoReceived`
- `DiagnosticInfoReceived`
- `ButtonPressed`

## Example Usage

### C#

```csharp
using A10x_SDK;

using var sdk = new A1040SDK();
sdk.Connect8x4("192.168.1.31");
sdk.SetGain(10);
sdk.SetAscanAveraging(5);
sdk.SetPulseRepetitionRate(3);
sdk.SetSingle8x4Transmitter(0);
sdk.StartAscanSingleTransmitter();
```

### Python

```python
from A10x_SDK import A1040SDK

sdk = A1040SDK()
sdk.Connect16x4("192.168.1.31", "192.168.1.32")
sdk.SetSingle16x4Transmitter(0)
sdk.StartAscanSingleTransmitter()
```

## Example Files In This Repository

- [`CsharpA1040Example/Program.cs`](/d:/Development/GIT/a1052_sdk_examples/CsharpA1040Example/Program.cs)
- [`PythonExample/a1040_example.py`](/d:/Development/GIT/a1052_sdk_examples/PythonExample/a1040_example.py)
