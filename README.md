# A1052 SDK examples
Examples for integration of A1052 Multisonic with its SDK

See https://acs-international.com/instruments/oem-ultrasonic-pulser-receiver-units/a1052-multisonic/ for more information about the device.

## Repository Structure

This repository contains examples and resources organized into the following directories:

### `PythonExample`
Python integration example demonstrating how to use the A1052 SDK with Python through .NET interop using pythonnet. Includes:
- `simple_example.py` - Complete working example with device connection, configuration, and data acquisition
- `python_example_readme.md` - Detailed setup and usage instructions

### `CsharpExample`
C# .NET example showing native SDK usage in a console application. Contains:
- `Program.cs` - Main example demonstrating SDK initialization, device connection, and data handling

## Getting Started

1. **Choose your preferred language example:**
   - For Python: Navigate to `/PythonExample` and follow the setup in `python_example_readme.md`
   - For C#: Open `/CsharpExample/CsharpExample.sln` in Visual Studio

2. **Prerequisites:**
   - A1052 Multisonic device
   - SDK DLL files in the `/SDK` directory
   - Device network connection (via device's WiFi access point)

3. **Configuration:**
   - Update the device IP address in the example code to match your device
   - Ensure SDK DLLs are properly referenced

