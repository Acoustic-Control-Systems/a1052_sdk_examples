# A10x SDK examples

Examples for integrating A1052 and A1040 devices with the A10x SDK.

See https://acs-international.com/instruments/oem-ultrasonic-pulser-receiver-units/a1052-multisonic/ for more information about the A1052 device family.

## Repository Structure

### SDK reference manuals

- [a1052_sdk_reference.md](a1052_sdk_reference.md) covers `A1052SDK`
- [a1040_sdk_reference.md](a1040_sdk_reference.md) covers `A1040SDK`

### [PythonExample](PythonExample)

Python integration examples demonstrating how to use the SDK with Python through pythonnet.

- [simple_example.py](PythonExample/simple_example.py) shows A1052 usage
- [a1040_example.py](PythonExample/a1040_example.py) shows A1040 usage
- [python_example_readme.md](PythonExample/python_example_readme.md) contains setup and usage notes

### [CsharpExample](CsharpExample)

C# example showing native `A1052SDK` usage in a console application.

- [Program.cs](CsharpExample/Program.cs) demonstrates SDK initialization, connection, and A-scan handling

### [CsharpA1040Example](CsharpA1040Example)

C# example showing native `A1040SDK` usage in a console application.

- [Program.cs](CsharpA1040Example/Program.cs) demonstrates an `8x4` flow and includes commented `16x4` and `8x8` examples

## Getting Started

1. Choose an example:
   - For Python, go to [PythonExample](PythonExample) and follow [python_example_readme.md](PythonExample/python_example_readme.md)
   - For C# A1052, open [CsharpExample/CsharpExample.sln](CsharpExample/CsharpExample.sln)
   - For C# A1040, open [CsharpA1040Example/CsharpA1040Example.csproj](CsharpA1040Example/CsharpA1040Example.csproj)

2. Prerequisites:
   - An A1052 or A1040 device
   - SDK DLL files in the `SDK` directory
   - Device network connectivity

3. Update the device IP addresses in the examples to match your hardware.
