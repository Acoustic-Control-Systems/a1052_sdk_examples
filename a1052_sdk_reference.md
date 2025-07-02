# A10x SDK API Reference

This document provides comprehensive API reference for the A10x SDK .NET library.

## Overview

The A10x SDK provides .NET wrapper access to MiraNeo device communication. The main classes are:
- `A10xSdk` - Abstract base class for all A10x series devices
- `A1052SDK` - Concrete implementation for A1052 devices (32-channel, 8x4 configuration)

---

## A1052SDK Class

The `A1052SDK` class provides functionality for interacting with the A1052 device, including connection management, data acquisition, and diagnostic information retrieval.

### Constructor

```csharp
public A1052SDK(ILogger? logger = null)
```

Creates a new instance of the A1052 SDK.

**Parameters:**
- `logger` (optional): ILogger instance for logging SDK operations

**Example:**
```csharp
var sdk = new A1052SDK();
// or with logging
var sdk = new A1052SDK(myLogger);
```

### Properties

#### DeviceIP
```csharp
public string DeviceIP { get; set; }
```
Gets or sets the IP address of the A1052 device to connect to.

**Example:**
```csharp
sdk.DeviceIP = "192.168.1.31";
```

### A1052-Specific Events

#### AscanDataReceived
```csharp
public event Action<short[], int, TimeSpan>? AscanDataReceived;
```
Fired when A-scan data is received from the A1052 device.

**Parameters:**
- `data`: Array of A-scan data samples (short values)
- `size`: Size of the data in samples
- `time`: Time of data acquisition

**Example:**
```csharp
sdk.AscanDataReceived += (data, size, timestamp) => {
    Console.WriteLine($"Received {size} samples at {timestamp}");
    // Process A-scan data
    for (int i = 0; i < size; i++) {
        short sample = data[i];
        // Process each sample
    }
};
```

### A1052-Specific Methods

#### Connect()
```csharp
public void Connect()
```
Connect to the A1052 device using the address specified in `DeviceIP` property.

**Throws:**
- `InvalidOperationException`: If DeviceIP is not set

**Example:**
```csharp
sdk.DeviceIP = "192.168.1.31";
sdk.Connect();
```

#### Connect(string deviceIp)
```csharp
public void Connect(string deviceIp)
```
Connect to the A1052 device using the specified IP address.

**Parameters:**
- `deviceIp`: IP address of the A1052 device

**Throws:**
- `InvalidOperationException`: If deviceIp is null or empty

**Example:**
```csharp
sdk.Connect("192.168.1.31");
```

#### StartAscanSingleTransmitter()
```csharp
public void StartAscanSingleTransmitter()
```
Start A-scan acquisition for a single transmitter on the A1052 device.

**Note:** Set transmitter index using `SetSingle8x4Transmitter(sensor)` before calling this method.

**Example:**
```csharp
sdk.SetSingle8x4Transmitter(0);  // Select transmitter (0-based)
sdk.StartAscanSingleTransmitter();
```

#### StartAscanQuadroTransmitter()
```csharp
public void StartAscanQuadroTransmitter()
```
Start A-scan acquisition for a quadro transmitter on the A1052 device.

**Note:** Set transmitter index using `SetQuadro8x4Transmitter(sensor)` before calling this method.

**Example:**
```csharp
sdk.SetQuadro8x4Transmitter(0);  // Select quadro transmitter (0-based)
sdk.StartAscanQuadroTransmitter();
```

#### RequestDiagnosticInfo()
```csharp
public void RequestDiagnosticInfo()
```
Request diagnostic information from the A1052 device.

**Note:** The `DiagnosticInfoReceived` event will be triggered when the information is received.

**Example:**
```csharp
sdk.DiagnosticInfoReceived += (diagnosticInfo) => {
    Console.WriteLine($"Diagnostic info received: {diagnosticInfo}");
};
sdk.RequestDiagnosticInfo();
```

### A1052 Configuration Methods

The A1052SDK inherits all configuration methods from A10xSdk. Here are the most commonly used ones for A1052:

#### Transmitter Selection

```csharp
// Single transmitter mode (0-31)
sdk.SetSingle8x4Transmitter(0);

// Quadro transmitter mode (0-7)  
sdk.SetQuadro8x4Transmitter(0);
```

#### Signal Parameters

```csharp
// Set operating frequency (10-100 KHz)
sdk.SetOperatingFrequency(50); // 50 KHz

// Set gain (0-36 dB)
sdk.SetGain(30); // 30 dB

// Set A-scan averaging (1-16)
sdk.SetAscanAveraging(8); // 8 averages

// Set maximum pulse repetition rate (0-100 Hz, 0 = as fast as possible)
sdk.SetPulseRepetitionRate(10.0); // 10 Hz

// Set periods number on burst (0.5-10.0)
sdk.SetPeriods(2.5); // 2.5 periods
```

### A1052 Usage Examples

#### Basic A1052 Connection and Data Acquisition

```csharp
using A10x_SDK;
using Microsoft.Extensions.Logging;

// Create logger (optional)
using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
var logger = loggerFactory.CreateLogger<A1052SDK>();

// Create SDK instance
var sdk = new A1052SDK(logger);

try
{
    // Subscribe to events
    sdk.NetworkConnected += (connected) => {
        if (connected) {
            Console.WriteLine("A1052 device connected");
            
            // Configure device for A-scan acquisition
            sdk.SetOperatingFrequency(50);    // 50 KHz
            sdk.SetGain(30);                  // 30 dB gain
            sdk.SetAscanAveraging(8);         // 8 averages
            sdk.SetPulseRepetitionRate(10.0); // 10 Hz PRR
            sdk.SetPeriods(2.5);              // 2.5 periods
            
            // Select transmitter and start acquisition
            sdk.SetSingle8x4Transmitter(0);   // Select transmitter 0
            sdk.StartAscanSingleTransmitter(); // Start single transmitter A-scan
        }
    };

    sdk.AscanDataReceived += (data, size, timestamp) => {
        Console.WriteLine($"A-scan data: {size} samples at {timestamp}");
        
        // Find peak amplitude
        short maxAmplitude = 0;
        int peakIndex = 0;
        for (int i = 0; i < size; i++) {
            if (Math.Abs(data[i]) > Math.Abs(maxAmplitude)) {
                maxAmplitude = data[i];
                peakIndex = i;
            }
        }
        Console.WriteLine($"Peak amplitude: {maxAmplitude} at index {peakIndex}");
    };

    sdk.NetworkDisconnected += () => {
        Console.WriteLine("A1052 device disconnected");
    };

    // Connect to A1052 device
    sdk.DeviceIP = "192.168.1.31";
    sdk.Connect();

    Console.WriteLine("Press any key to stop...");
    Console.ReadKey();

} finally {
    sdk.Dispose();
}
```

#### Quadro Transmitter Mode

```csharp
var sdk = new A1052SDK();

sdk.NetworkConnected += (connected) => {
    if (connected) {
        // Configure for quadro transmitter mode
        sdk.SetOperatingFrequency(75);     // 75 KHz
        sdk.SetGain(25);                   // 25 dB gain
        sdk.SetAscanAveraging(4);          // 4 averages
        
        // Select quadro transmitter group 0 (transmitters 0,1,2,3)
        sdk.SetQuadro8x4Transmitter(0);
        sdk.StartAscanQuadroTransmitter();
    }
};

sdk.Connect("192.168.1.31");
```

#### Scanning Multiple Transmitters

```csharp
var sdk = new A1052SDK();
int currentTransmitter = 0;
const int maxTransmitters = 32; // A1052 has 32 transmitters (0-31)

sdk.NetworkConnected += (connected) => {
    if (connected) {
        // Configure device
        sdk.SetOperatingFrequency(60);
        sdk.SetGain(28);
        sdk.SetAscanAveraging(8);
        
        // Start with first transmitter
        ScanNextTransmitter();
    }
};

sdk.AscanDataReceived += (data, size, timestamp) => {
    Console.WriteLine($"Transmitter {currentTransmitter}: {size} samples");
    
    // Process data for current transmitter
    ProcessAscanData(data, size, currentTransmitter);
    
    // Move to next transmitter
    currentTransmitter++;
    if (currentTransmitter < maxTransmitters) {
        ScanNextTransmitter();
    } else {
        Console.WriteLine("Scan complete");
        sdk.StopAcquisition();
    }
};

void ScanNextTransmitter() {
    sdk.SetSingle8x4Transmitter(currentTransmitter);
    sdk.StartAscanSingleTransmitter();
}

void ProcessAscanData(short[] data, int size, int transmitterIndex) {
    // Your data processing logic here
    // For example, calculate thickness, detect flaws, etc.
}

sdk.Connect("192.168.1.31");
```

#### Battery and Diagnostic Monitoring

```csharp
var sdk = new A1052SDK();

// Subscribe to diagnostic events
sdk.MasterBatteryInfoReceived += (batteryInfo) => {
    Console.WriteLine($"A1052 Battery Info: {batteryInfo}");
};

sdk.DiagnosticInfoReceived += (diagnosticInfo) => {
    Console.WriteLine($"A1052 Diagnostic Info: {diagnosticInfo}");
};

sdk.ButtonPressed += () => {
    Console.WriteLine("A1052 button was pressed");
    // Request diagnostic info when button is pressed
    sdk.RequestDiagnosticInfo();
};

sdk.NetworkConnected += (connected) => {
    if (connected) {
        // Request initial battery info
        sdk.RequestBatteryInfo();
        
        // Start periodic diagnostic requests
        var timer = new System.Timers.Timer(30000); // Every 30 seconds
        timer.Elapsed += (s, e) => sdk.RequestDiagnosticInfo();
        timer.Start();
    }
};

sdk.Connect("192.168.1.31");
```

#### LED Control and Device Identification

```csharp
var sdk = new A1052SDK();

sdk.NetworkConnected += (connected) => {
    if (connected) {
        // Blink LEDs in sequence for identification
        BlinkLEDsSequence();
    }
};

async void BlinkLEDsSequence() {
    // Turn on LEDs one by one
    for (int i = 0; i < 8; i++) {
        sdk.SetLedMask(1 << i); // Turn on LED i
        await Task.Delay(200);
    }
    
    // Turn off all LEDs
    sdk.SetLedMask(0);
    
    // Flash all LEDs
    for (int i = 0; i < 5; i++) {
        sdk.SetLedMask(0xFF); // All LEDs on
        await Task.Delay(100);
        sdk.SetLedMask(0);    // All LEDs off
        await Task.Delay(100);
    }
}

sdk.Connect("192.168.1.31");
```

---

## Base Class: A10xSdk

The A1052SDK inherits from the abstract `A10xSdk` class. Below are the inherited methods and events available to A1052SDK.

### Inherited Events

- `NetworkConnected`: Fired when network connection status changes
- `NetworkDisconnected`: Fired when network disconnects
- `DiagnosticInfoReceived`: Fired when diagnostic information is received

### Inherited Methods

- `StopCommunication()`: Stop communication with all devices
- `StopAcquisition()`: Set device to idle state
- `SetOperatingFrequency(int frequency)`: Set operating frequency
- `SetGain(int gain)`: Set gain value
- `SetAscanAveraging(int averaging)`: Set A-scan averaging
- `SetPulseRepetitionRate(double rate)`: Set pulse repetition rate
- `RequestBatteryInfo()`: Request battery information from both devices
- `SetLedMask(int mask)`: Set LED mask (bitmask or bool array)
- `SetLedMask(bool[] maskArray)`: Set LED mask (from bool array)
- `RequestIdent()`: Request device identification by blinking LEDs
- `SetPeriods(float periods)`: Set periods number in one burst (float).
- `SetSingle8x4Transmitter(int sensor)`: Set transmitter index for 32-channels device in single mode
- `SetQuadro8x4Transmitter(int sensor)`: Set transmitter index for 32-channels device in quadro mode

## Parameter Limits

The following parameter limits apply to A1052 devices:

```csharp
public class ParametersLimits
{
    public const int AVERAGING_MIN = 1;
    public const int AVERAGING_MAX = 16;

    public const int ANALOG_GAIN_MIN = 0;
    public const int ANALOG_GAIN_MAX = 36;

    public const float BURST_PERIODS_MIN = 0.5f;
    public const float BURST_PERIODS_MAX = 10f;

    public const int OPERATING_FREQUENCY_MIN = 10;    // 10 KHz
    public const int OPERATING_FREQUENCY_MAX = 100;   // 100 KHz

    public const int PULSE_REPETITION_RATE_MIN = 0;   // 0 = as fast as possible
    public const int PULSE_REPETITION_RATE_MAX = 100; // 100 Hz maximum
}
```

## A1052 Device Specifications

- **Channels**: 32 (8x4 configuration)
- **Transmitter modes**: Single (0-31) or Quadro (0-7 groups)
- **Data format**: 16-bit signed integers (short)
- **Sample rate**: Fixed at 4096 samples per A-scan
- **Operating frequency**: 10-200 KHz
- **Gain range**: 0-36 dB
- **Connection**: Ethernet (TCP/IP)

## Best Practices for A1052

1. **Always dispose**: Use `using` statement or call `Dispose()` when finished
2. **Handle events**: Subscribe to `NetworkDisconnected` for robust error handling
3. **Process data efficiently**: A-scan data arrives frequently, process quickly
4. **Monitor battery**: Request battery info periodically for battery-powered devices

## Troubleshooting A1052

### Connection Issues
- Verify device IP address is correct
- Ensure device is powered on and initialized
- Check network connectivity to device
- Verify firewall settings allow TCP communication

### Data Quality Issues
- Adjust gain if signal is too weak or saturated
- Increase averaging for better signal-to-noise ratio
- Check operating frequency matches transducer specifications
- Verify transmitter selection is correct

### Performance Issues  
- Reduce pulse repetition rate if data processing can't keep up
- Use appropriate averaging settings
- Process A-scan data in background thread if needed