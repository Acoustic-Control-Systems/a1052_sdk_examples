import pythonnet
# load core library. otherwise pythonnet cannot import clr
pythonnet.load("coreclr")
import clr
import time
import logging
import os

# Simple logging setup
logging.basicConfig(level=logging.INFO, format='%(asctime)s [%(levelname)s] %(message)s')
logger = logging.getLogger(__name__)


def load_sdk_dlls(sdk_folder_path: str):
    """
    Load necessary A10x SDK DLLs from the specified folder.
    
    :param sdk_folder_path: Path to the folder containing A10x SDK DLLs. Could be relative or absolute.
    :type sdk_folder_path: str
    
    :return: None    
    """
    # check relative path to the SDK folder
    if not os.path.isabs(sdk_folder_path):
        # if the path is relative, make it absolute
        logger.warning(f"SDK folder path is relative: {sdk_folder_path}. Trying to resolve it relative to the current file.")
        # resolve relative path to the current file's directory
        sdk_folder_path = os.path.join(os.path.dirname(__file__), sdk_folder_path)

    # enumerate the dependencies directory
    if not os.path.exists(sdk_folder_path):
        logger.error(f"Dependencies directory does not exist: {sdk_folder_path}")
        raise FileNotFoundError(f"Dependencies directory not found: {sdk_folder_path}")

    # sys.path.append(dependencies_dir)

    # Add references to all required DLLs
    clr.AddReference(os.path.join(sdk_folder_path, "A10x_SDK.dll"))
    clr.AddReference(os.path.join(sdk_folder_path, "Microsoft.Extensions.Logging.Abstractions.dll"))
    clr.AddReference(os.path.join(sdk_folder_path, "Microsoft.Extensions.DependencyInjection.Abstractions.dll"))
    clr.AddReference(os.path.join(sdk_folder_path, "System.Diagnostics.DiagnosticSource.dll"))

def has_feature(feature_name):
    try:
        # Try to access version-specific features
        getattr(A1052SDK, feature_name)
        return True
    except AttributeError:
        return False
    
def check_sdk_version(required_version:str)-> str | None:
    import System
    try:
        # Get assembly version
        assembly = System.Reflection.Assembly.GetAssembly(A1052SDK)
        version = assembly.GetName().Version
        version_str = f"{version.Major}.{version.Minor}.{version.Build}"
        logger.info(f"A10x SDK Version: {version_str}")
        
        # Check minimum required version
        if version_str < required_version:
            raise Exception(f"SDK version {version_str} is below required {required_version}")
        
        return version_str
    except Exception as e:
        logger.error(f"Version check failed: {e}")
        return None

sdk_folder = r"..\sdk"

load_sdk_dlls(sdk_folder)

from A10x_SDK import A1052SDK, A10xIdentity

# Check SDK version after loading
sdk_version = check_sdk_version("1.0.0")
if not sdk_version:
    logger.warning("Cannot verify SDK version. Proceeding with caution...")


def simple_example():
    """
    Simple example showing basic A10x SDK usage in Python
    """
    # Device IP address - change this to match your device
    device_ip = "192.168.137.123"
    

        
    # Check if device is available
    if not A10xIdentity.IdentifyDevice(device_ip):
        logger.error(f"Cannot identify device at {device_ip}")
        return False
    
    # Get basic device info
    info = A10xIdentity.GetDeviceInfo(device_ip)
    logger.info(f"Connected to device: Serial={info.Serial}, MAC={info.Mac}")
    
    # Create SDK instance
    sdk = A1052SDK()
    
    # Check if specific features are available
    if not has_feature("SetGain"):
        logger.error("Required feature 'SetGain' is not available in this SDK version.")
        return False
    
    # there is no such feature in the SDK, but this is just an example
    # if not has_feature("ChangeDeviceIp"):
    #     logger.error("Required feature 'ChangeDeviceIp' is not available in this SDK version.")
    #     return False
    
    try:
        # Connect to device
        sdk.Connect(device_ip)
        logger.info("Connected to device")
        
        # Basic configuration
        sdk.SetGain(10) # gain in dB
        sdk.SetAscanAveraging(1)
        sdk.SetPulseRepetitionRate(3) # Hz
        sdk.SetQuadro8x4Transmitter(0) # for quadro mode (4 sensors in one column emit wave) set first column of sensors to transmit
        sdk.SetSingle8x4Transmitter(0) # for single mode (1 sensor emits wave) set first sensor to transmit
        
        # Set up a simple data handler
        def on_data_received(data, length, timestamp):
            logger.info(f"Received {length} samples at {timestamp}")
        
        # Subscribe to data events
        sdk.AscanDataReceived += on_data_received
        
        # Start acquisition
        sdk.StartAscanSingleTransmitter()
        logger.info("Started data acquisition...")
        
        # Collect data for 5 seconds
        time.sleep(5)
        
        # Stop acquisition
        sdk.StopAcquisition()
        logger.info("Stopped data acquisition")
        
        # Unsubscribe from events
        sdk.AscanDataReceived -= on_data_received
        
        return True
        
    except Exception as e:
        logger.error(f"Error during operation: {e}")
        return False
        
    finally:
        # Always disconnect or dispose the SDK instance
        sdk.Disconnect()
        logger.info("Disconnected from device")

if __name__ == "__main__":
    logger.info("Starting simple A10x SDK example...")
    success = simple_example()
    
    if success:
        logger.info("Example completed successfully!")
    else:
        logger.error("Example failed!")
