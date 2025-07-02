# A10x SDK Python Example

This directory contains a Python example demonstrating how to use the A10x SDK with Python through .NET interop.

## SDK Version

**Current SDK Version:** 1.0.0

This example is compatible with A10x SDK version 1.0.0 and later. Always verify SDK version compatibility before use.

## Prerequisites

1. **Python 3.7 or higher**
2. **A10x SDK .NET DLL and its dependencies (v1.0.0+)**
3. **pythonnet package** for .NET interop
4. **.NET 8 or higher** (for A10x SDK)

## Setup

1. **Install Python dependencies:**
   ```powershell
   pip install -r requirements.txt
   ```

2. **Ensure A10x SDK DLLs are accessible:**
   - Place the A10x SDK DLLs in the 'sdk' directory
      - You can use another directory. Just make sure, you reference the correct one in the python script
      
3. **For test with the real device**
   - Power on the device
   - Wait till it's initialized
   - Once device' access point is visible, connect to it
   - Consult device IP to use it later in script

## Project Structure

```
PythonExample/
├── README.md
├── requirements.txt
├── simple_example.py
└── sdk/ (place A10x SDK DLLs here)
```

## Usage

### Running the Examples

**Simple Example (minimal setup):**
   ```powershell
   python simple_example.py
   ```

### Code Overview

The `simple_example.py` demonstrates:

- **SDK Initialization:** Loading the A10x .NET assembly and initializing the SDK
- **Version Verification:** Checking SDK version compatibility
- **Basic Operations:** Common SDK operations like connecting, configuring, and data retrieval
- **Error Handling:** Proper exception handling for .NET interop
- **Resource Cleanup:** Ensuring proper disposal of SDK resources

### Example Code Structure

```python
import pythonnet
# load core library. otherwise pythonnet cannot import clr
pythonnet.load("coreclr")
import clr
import sys
from pathlib import Path

def load_sdk_dlls(path:str):
    ...

# Add reference to A10x SDK
load_sdk_dlls("sdk")

from A10x.SDK import *

# Verify SDK version
def check_sdk_version()-> str | None:
    ...

# Change to your device' ip
device_ip = "192.168.1.31"

# Verify SDK version before use
sdk_version = check_sdk_version()
if not sdk_version:
    print("Cannot verify SDK version. Proceeding with caution...")

# Initialize SDK
sdk = A10xSDK()
try:
    # Your A10x operations here
    sdk.Connect(device_ip)
    # ... more operations ...
finally:
    sdk.Dispose()
```

## Configuration

### SDK Version Compatibility

To ensure compatibility with your Python application:

1. **Check SDK Version in Code:**
   ```python
   def get_sdk_version():
        import System
        # Get assembly version
        assembly = System.Reflection.Assembly.GetAssembly(A1052SDK)
        version = assembly.GetName().Version
        version_str = f"{version.Major}.{version.Minor}.{version.Build}"
        return version_str
   ```

2. **Version-Specific Feature Detection:**
   ```python
    def has_feature(feature_name):
        try:
            # Try to access version-specific features
            getattr(A1052SDK, feature_name)
            return True
        except AttributeError:
            return False
    ```

3. **Handle Version Differences:**
   ```python
   sdk_version = get_sdk_version()
   if sdk_version >= "1.1.0":
       # Use newer API
       result = sdk.NewMethod()
   else:
       # Use legacy API
       result = sdk.LegacyMethod()
   ```

## Common Issues and Troubleshooting

### Version Mismatch Issues

**Problem:** SDK version incompatibility

**Solutions:**
1. Check SDK version: Use the version check code above
2. Update SDK: Download the latest compatible version
3. Update Python code: Adapt to SDK version differences

### DLL Loading Issues

**Problem:** `System.IO.FileNotFoundException` when loading A10x SDK

**Solutions:**
1. Verify DLL path is correct
2. Ensure all dependencies are in the same directory
3. Check that the DLL architecture (x86/x64) matches your Python installation
4. Verify SDK version matches expected version

### Python.NET Issues

**Problem:** Import errors with `clr` module

**Solutions:**
1. Make sure you call ```pythonnet.load("coreclr")``` before importing ```clr```
2. Reinstall pythonnet: `pip uninstall pythonnet && pip install pythonnet`
3. Verify .NET Framework/Core installation
4. Check Python architecture compatibility

### Runtime Errors

**Problem:** Method calls fail or return unexpected results

**Solutions:**
1. Verify SDK initialization was successful
2. Check parameter types and values
3. Verify SDK version compatibility
4. Enable debug logging for more details

## Advanced Usage

### Version-Aware SDK Loading

```python

    sdk_folder = "sdk"

    load_sdk_dlls(sdk_folder)

    from A10x_SDK import A1052SDK, A10xIdentity

    # Check SDK version after loading
    sdk_version = check_sdk_version("1.0.0")
    if not sdk_version:
        logger.warning("Cannot verify SDK version. Proceeding with caution...")
```

### Async Operations

For long-running operations, consider using Python's asyncio:

```python
import asyncio
import threading

async def async_sdk_operation():
    # Wrap SDK calls in thread executor
    loop = asyncio.get_event_loop()
    result = await loop.run_in_executor(None, sdk.long_running_operation)
    return result
```

### Error Handling Best Practices

```python
from System import Exception as DotNetException

try:
    sdk.some_operation()
except DotNetException as e:
    print(f".NET Exception: {e.Message}")
    # Log SDK version for debugging
    print(f"SDK Version: {get_sdk_version()}")
except Exception as e:
    print(f"Python Exception: {str(e)}")
```

## Support

For issues specific to:
- **A10x SDK**: Consult the main SDK documentation
- **Python integration**: Check this example and pythonnet documentation
- **Version compatibility**: Refer to SDK changelog and version notes

## License

This example follows the same license as the A10x SDK. Please refer to the main SDK documentation for license details.

