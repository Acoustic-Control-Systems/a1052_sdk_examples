# A1052 SDK Python Example

This directory contains the Python example for the `A1052SDK` device flow.

## Files

- `a1052_example.py`: A1052 example using `A10x_SDK.A1052SDK`
- `requirements.txt`: Python dependency list for pythonnet

## Prerequisites

1. Python 3.7 or higher
2. `pythonnet`
3. .NET 8 or higher
4. `A1052_SDK.dll` and companion DLLs in the repository `SDK` directory

## Setup

1. Install dependencies:

   ```powershell
   pip install -r requirements.txt
   ```

2. Make sure `..\..\SDK` contains `A1052_SDK.dll` and its companion DLLs.

3. Update the device IP in `a1052_example.py` before running the example.

## Usage

```powershell
python a1052_example.py
```

## What The Example Covers

- Loading `A1052_SDK.dll` from `..\..\SDK`
- Device identification with `A10xIdentity`
- Connecting with `A1052SDK.Connect`
- Gain, averaging, and pulse repetition setup
- Single-transmitter A-scan acquisition
- Cleanup through `StopAcquisition()` and `Disconnect()`

## Related Files

- [`a1052_example.py`](a1052_example.py)
- [`a1052_sdk_reference.md`](../a1052_sdk_reference.md)
