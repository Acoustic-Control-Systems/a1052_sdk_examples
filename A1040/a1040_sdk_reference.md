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

### IsConnected

```csharp
public bool IsConnected { get; }
```

Inherited from `A10xSdk`. Returns `true` while the communication task is running.

### IsCommunicationCancelled

```csharp
public bool IsCommunicationCancelled { get; }
```

Inherited from `A10xSdk`. Returns `true` when the SDK cancellation token has been triggered.

## A1040-Specific Events

### AscanDataReceived

```csharp
public event Action<short[], int, TimeSpan>? AscanDataReceived;
```

Raised when processed A-scan data arrives. The `int` argument is the number of samples, not bytes. One array contains samples from all device channels sequentially (e.g. for MASTER-only version of A1040 32 channels x 4096 samples = 131072 array length, for MASTER-SLAVE version 64 channels x 4096 samples = 262144 array length).

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

Controls the A1040 laser state. Note that during acquisition of A-Scans the laser is automatically turned off, so this method is typically used to turn on the laser before acquisition. 

### 8x4 Transmitter Selection

```csharp
public void SetSingle8x4Transmitter(int sensor)
public void SetQuadro8x4Transmitter(int sensor)
```

Selects the active transmitter for `8x4` acquisition. In QUADRO mode groups of 4 channels are activated together, so valid sensor numbers are 0-7 (e.g. `SetQuadro8x4Transmitter(0)` activates channels 0-3, `SetQuadro8x4Transmitter(1)` activates channels 4-7, etc.).

### 16x4 Transmitter Selection

```csharp
public void SetSingle16x4Transmitter(int sensor)
public void SetQuadro16x4Transmitter(int sensor)
```

Selects the active transmitter for `16x4` acquisition. In QUADRO mode groups of 4 channels are activated together, so valid sensor numbers are 0-7 (e.g. `SetQuadro16x4Transmitter(0)` activates channels 0-3, `SetQuadro16x4Transmitter(1)` activates channels 4-7, etc.).

### 8x8 Transmitter Selection

```csharp
public void SetSingle8x8Transmitter(int sensor)
public void SetQuadro8x8Transmitter(int sensor)
```

Selects the active transmitter for `8x8` acquisition. In QUADRO mode groups of 4 channels are activated together, so valid sensor numbers are 0-3 (e.g. `SetQuadro8x8Transmitter(0)` activates channels 0-3, `SetQuadro8x8Transmitter(1)` activates channels 4-7, etc.).

### Acquisition Control

```csharp
public void StartAscanSingleTransmitter()
public void StartAscanQuadroTransmitter()
```

- `StartAscanSingleTransmitter()` starts A-scan acquisition using the current single-transmitter selection
- `StartAscanQuadroTransmitter()` starts A-scan acquisition using the current quadro-transmitter selection

```csharp
public void StopAcquisition()
```

Stops any ongoing acquisition.

### Battery Info Request

```csharp
public void RequestBatteryInfo()
public void RequestSlaveBatteryInfo()
```

Requests battery status from the master and slave devices. The master battery info is returned through the `MasterBatteryInfoReceived` event inherited from `A10xSdk`, while the slave battery info is returned through the `SlaveBatteryInfoReceived` event.

## Inherited Events From A10xSdk

### NetworkConnected

```csharp
public event Action<bool>? NetworkConnected;
```

Raised when the SDK connection state changes.

### NetworkDisconnected

```csharp
public event Action? NetworkDisconnected;
```

Raised when the device communication layer disconnects and cleanup has completed.

### MasterBatteryInfoReceived

```csharp
public event Action<BatteryResult>? MasterBatteryInfoReceived;
```

Raised when battery information is received from the master device.

### DiagnosticInfoReceived

```csharp
public event Action<DiagnosticInfo>? DiagnosticInfoReceived;
```

Raised when diagnostic information is received from the device.

### ButtonPressed

```csharp
public event Action? ButtonPressed;
```

Raised when the device button short-press event is received.

## Inherited Methods From A10xSdk

### Connection And Lifecycle

```csharp
public void StopCommunication()
public void Disconnect()
public void Dispose()
```

- `StopCommunication()` sends a finish-communication message to the device layer
- `Disconnect()` disposes the communication engine and triggers the normal disconnection flow
- `Dispose()` releases SDK resources and waits for disconnect handling when needed

### Signal And Acquisition Configuration

```csharp
public void StopAcquisition()
public void SetOperatingFrequency(int frequency)
public void SetGain(int gain)
public void SetAscanAveraging(int averaging)
public void SetPulseRepetitionRate(double rate)
public void SetPeriods(float periods)
```

- `StopAcquisition()` returns the device to idle state
- `SetOperatingFrequency(int)` accepts values from `ParametersLimits.OPERATING_FREQUENCY_MIN` to `ParametersLimits.OPERATING_FREQUENCY_MAX`
- `SetGain(int)` accepts values from `ParametersLimits.ANALOG_GAIN_MIN` to `ParametersLimits.ANALOG_GAIN_MAX`
- `SetAscanAveraging(int)` accepts values from `ParametersLimits.AVERAGING_MIN` to `ParametersLimits.AVERAGING_MAX`
- `SetPulseRepetitionRate(double)` accepts values from `ParametersLimits.PULSE_REPETITION_RATE_MIN` to `ParametersLimits.PULSE_REPETITION_RATE_MAX`
- `SetPeriods(float)` accepts values from `ParametersLimits.BURST_PERIODS_MIN` to `ParametersLimits.BURST_PERIODS_MAX`

### 8x4 Shared Transmitter Methods

```csharp
public void SetSingle8x4Transmitter(int sensor)
public void SetQuadro8x4Transmitter(int sensor)
```

These methods are inherited from `A10xSdk` and are used by A1040 in `8x4` mode.

### Battery, LEDs, And Device Identification

```csharp
public void RequestBatteryInfo()
public void SetLedMask(int mask)
public void SetLedMask(bool[] maskArray)
public void RequestIdent()
```

- `RequestBatteryInfo()` requests battery status from the master device, and from both devices where supported by the communication layer
- `SetLedMask(int)` sets the device LEDs from a bitmask
- `SetLedMask(bool[])` sets the device LEDs from an array of boolean states
- `RequestIdent()` asks the device to identify itself by blinking LEDs

## Typical Combined Usage

These inherited and A1040-specific members are commonly used together:

```csharp
sdk.Connect8x4("192.168.1.31");

sdk.SetOperatingFrequency(50);
sdk.SetGain(10);
sdk.SetAscanAveraging(5);
sdk.SetPulseRepetitionRate(3);
sdk.SetPeriods(2.5f);

sdk.SetSingle8x4Transmitter(0);
sdk.StartAscanSingleTransmitter();

sdk.RequestBatteryInfo();
sdk.StopAcquisition();
sdk.Disconnect();

bool connected = sdk.IsConnected;
bool cancelled = sdk.IsCommunicationCancelled;
```

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

- [CsharpExample/Program.cs](CsharpExample/Program.cs)
- [PythonExample/a1040_example.py](PythonExample/a1040_example.py)
