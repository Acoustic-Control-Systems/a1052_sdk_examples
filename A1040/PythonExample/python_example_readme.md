# A1040 SDK Python Example

This directory contains the Python example for the `A1040SDK` device flow.

## Files

- `a1040_example.py`: A1040 example using `A10x_SDK.A1040SDK`
- `requirements.txt`: Python dependency list for pythonnet

## Prerequisites

1. Python 3.7 or higher
2. `pythonnet`
3. .NET 8 or higher
4. A10x SDK DLLs in the shared repository `SDK` directory

## Setup

1. Install dependencies:

   ```powershell
   pip install -r requirements.txt
   ```

2. Make sure `..\..\SDK` contains `A10x_SDK.dll` and its companion DLLs.

3. Update the device IPs in `a1040_example.py` before running the example.

## Usage

```powershell
python a1040_example.py
```

## What The Example Covers

- Loading the shared SDK DLLs from `..\..\SDK`
- Device identification with `A10xIdentity`
- Connecting in `8x4` mode with `Connect8x4`
- Laser toggling through `SetLaser(True)` and `SetLaser(False)`
- Single and quadro transmitter selection
- Cleanup through `StopAcquisition()` and `Disconnect()`
- Commented `16x4` and `8x8` examples for paired setups

## Related Files

- [`a1040_example.py`](/d:/Development/GIT/a1052_sdk_examples/A1040/PythonExample/a1040_example.py)
- [`a1040_sdk_reference.md`](/d:/Development/GIT/a1052_sdk_examples/A1040/a1040_sdk_reference.md)
