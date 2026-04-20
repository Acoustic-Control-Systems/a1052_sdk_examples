# A10x SDK examples

Examples for integrating A1052 and A1040 devices with the A10x SDK.

The repository is structured by device type. Each device has its own C# example, Python example, and reference document.

## Repository Structure

### [A1052](A1052)

- [A1052/a1052_sdk_reference.md](A1052/a1052_sdk_reference.md)
- [A1052/CsharpExample/CsharpExample.sln](A1052/CsharpExample/CsharpExample.sln)
- [A1052/CsharpExample/Program.cs](A1052/CsharpExample/Program.cs)
- [A1052/PythonExample/a1052_example.py](A1052/PythonExample/a1052_example.py)
- [A1052/PythonExample/python_example_readme.md](A1052/PythonExample/python_example_readme.md)

### [A1040](A1040)

- [A1040/a1040_sdk_reference.md](A1040/a1040_sdk_reference.md)
- [A1040/CsharpExample/CsharpA1040Example.csproj](A1040/CsharpExample/CsharpA1040Example.csproj)
- [A1040/CsharpExample/Program.cs](A1040/CsharpExample/Program.cs)
- [A1040/PythonExample/a1040_example.py](A1040/PythonExample/a1040_example.py)
- [A1040/PythonExample/python_example_readme.md](A1040/PythonExample/python_example_readme.md)


## Getting Started

1. Choose the device family you want to work with: `A1052` or `A1040`.
2. Use the device-specific C# or Python example under that folder.
3. Update the device IP addresses in the example code to match your hardware.
4. Ensure the shared SDK DLLs are present in [`SDK`](SDK).
