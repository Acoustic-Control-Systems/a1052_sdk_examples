# A10x SDK Python Examples

This directory contains Python examples showing how to use the A10x SDK through pythonnet.

## Device Coverage

- `simple_example.py`: A1052 example using `A10x_SDK.A1052SDK`
- `a1040_example.py`: A1040 example using `A10x_SDK.A1040SDK`

## Prerequisites

1. Python 3.7 or higher
2. A10x SDK .NET DLLs in the repository `SDK` directory
3. `pythonnet`
4. .NET 8 or higher

## Setup

1. Install Python dependencies:

   ```powershell
   pip install -r requirements.txt
   ```

2. Make sure the SDK DLLs in `..\SDK` include the device class you want to use.

3. Connect to the device access point and confirm the correct IP address.

## Project Structure

```text
PythonExample/
|-- a1040_example.py
|-- python_example_readme.md
|-- requirements.txt
`-- simple_example.py
```

## Usage

### A1052 Example

```powershell
python simple_example.py
```

### A1040 Example

```powershell
python a1040_example.py
```

Update the IP addresses in the script before running it.

## What The A1040 Example Covers

- Loading the packaged SDK DLLs from `..\SDK`
- Device identification with `A10xIdentity`
- `Connect8x4`
- Laser toggling through `SetLaser(True)` and `SetLaser(False)`
- Single and quadro transmitter selection
- A-scan acquisition and cleanup through `StopAcquisition()` and `Disconnect()`
- Commented `16x4` and `8x8` examples for paired setups

## Troubleshooting

### DLL Loading Errors

If pythonnet cannot load the SDK:

1. Verify that the `SDK` directory contains `A10x_SDK.dll` and its companion DLLs.
2. Verify that your Python installation matches the .NET architecture you want to use.
3. Confirm that `pythonnet.load("coreclr")` runs before importing `clr`.

### Connection Problems

1. Confirm the device is reachable on the expected IP.
2. For paired modes, verify master and slave IP order when using the commented examples.
3. Use the device identification log lines to confirm you are talking to the intended hardware.

## Related Files

- [`simple_example.py`](/d:/Development/GIT/a1052_sdk_examples/PythonExample/simple_example.py)
- [`a1040_example.py`](/d:/Development/GIT/a1052_sdk_examples/PythonExample/a1040_example.py)
- [`a1040_sdk_reference.md`](/d:/Development/GIT/a1052_sdk_examples/a1040_sdk_reference.md)
