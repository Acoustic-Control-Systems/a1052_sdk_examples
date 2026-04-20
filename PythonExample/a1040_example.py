import pythonnet

# load core library. otherwise pythonnet cannot import clr
pythonnet.load("coreclr")

import clr
import time
import logging
import os


logging.basicConfig(level=logging.INFO, format="%(asctime)s [%(levelname)s] %(message)s")
logger = logging.getLogger(__name__)


def load_sdk_dlls(sdk_folder_path: str):
    """
    Load necessary A10x SDK DLLs from the specified folder.

    :param sdk_folder_path: Path to the folder containing A10x SDK DLLs. Could be relative or absolute.
    :type sdk_folder_path: str
    """
    if not os.path.isabs(sdk_folder_path):
        logger.warning("SDK folder path is relative: %s. Trying to resolve it relative to the current file.", sdk_folder_path)
        sdk_folder_path = os.path.join(os.path.dirname(__file__), sdk_folder_path)

    if not os.path.exists(sdk_folder_path):
        logger.error("Dependencies directory does not exist: %s", sdk_folder_path)
        raise FileNotFoundError(f"Dependencies directory not found: {sdk_folder_path}")

    clr.AddReference(os.path.join(sdk_folder_path, "A10x_SDK.dll"))
    clr.AddReference(os.path.join(sdk_folder_path, "Microsoft.Extensions.Logging.Abstractions.dll"))
    clr.AddReference(os.path.join(sdk_folder_path, "Microsoft.Extensions.DependencyInjection.Abstractions.dll"))
    clr.AddReference(os.path.join(sdk_folder_path, "System.Diagnostics.DiagnosticSource.dll"))


sdk_folder = r"..\SDK"

load_sdk_dlls(sdk_folder)

from A10x_SDK import A1040SDK, A10xIdentity


def a1040_example():
    """
    Example showing A1040 SDK usage in Python with 8x4 mode.
    """
    master_ip = "192.168.1.31"

    if not A10xIdentity.IdentifyDevice(master_ip):
        logger.error("Cannot identify device at %s", master_ip)
        return False

    info = A10xIdentity.GetDeviceInfo(master_ip)
    logger.info("Connected to device: Serial=%s, MAC=%s, Version=%s", info.Serial, info.Mac, info.Version)

    sdk = A1040SDK()

    try:
        sdk.Connect8x4(master_ip)
        logger.info("Connected in 8x4 mode")

        sdk.SetGain(10)
        sdk.SetAscanAveraging(5)
        sdk.SetPulseRepetitionRate(3)

        sdk.SetLaser(True)
        logger.info("Laser enabled")
        time.sleep(1)
        sdk.SetLaser(False)
        logger.info("Laser disabled")

        def on_data_received(data, length, timestamp):
            logger.info("Received %s samples at %s", length, timestamp)

        sdk.AscanDataReceived += on_data_received

        sdk.SetSingle8x4Transmitter(0)
        sdk.StartAscanSingleTransmitter()
        logger.info("Started A-scan with single transmitter...")
        time.sleep(5)

        sdk.StopAcquisition()
        logger.info("Stopped single-transmitter acquisition")
        time.sleep(0.5)

        sdk.SetQuadro8x4Transmitter(0)
        sdk.StartAscanQuadroTransmitter()
        logger.info("Started A-scan with quadro transmitter...")
        time.sleep(5)

        sdk.StopAcquisition()
        logger.info("Stopped quadro-transmitter acquisition")

        sdk.AscanDataReceived -= on_data_received
        return True

    except Exception as e:
        logger.error("Error during operation: %s", e)
        return False

    finally:
        sdk.Disconnect()
        logger.info("Disconnected from device")


# --- 16x4 mode example (uncomment to use) ---
# def a1040_16x4_example():
#     master_ip = "192.168.1.31"
#     slave_ip = "192.168.1.32"
#     sdk = A1040SDK()
#     try:
#         sdk.Connect16x4(master_ip, slave_ip)
#         sdk.SetGain(10)
#         sdk.SetAscanAveraging(5)
#         sdk.SetPulseRepetitionRate(3)
#         sdk.SetSingle16x4Transmitter(0)
#         sdk.SetLaser(True)
#         time.sleep(1)
#         sdk.SetLaser(False)
#         sdk.StartAscanSingleTransmitter()
#         time.sleep(5)
#         sdk.RequestBatteryInfo()
#         sdk.RequestSlaveBatteryInfo()
#         sdk.StopAcquisition()
#     finally:
#         sdk.Disconnect()

# --- 8x8 mode example (uncomment to use) ---
# def a1040_8x8_example():
#     master_ip = "192.168.1.31"
#     slave_ip = "192.168.1.32"
#     sdk = A1040SDK()
#     try:
#         sdk.Connect8x8(master_ip, slave_ip)
#         sdk.SetGain(10)
#         sdk.SetAscanAveraging(5)
#         sdk.SetPulseRepetitionRate(3)
#         sdk.SetSingle8x8Transmitter(0)
#         sdk.SetLaser(True)
#         time.sleep(1)
#         sdk.SetLaser(False)
#         sdk.StartAscanSingleTransmitter()
#         time.sleep(5)
#         sdk.RequestBatteryInfo()
#         sdk.RequestSlaveBatteryInfo()
#         sdk.StopAcquisition()
#     finally:
#         sdk.Disconnect()


if __name__ == "__main__":
    logger.info("Starting A1040 SDK example...")
    success = a1040_example()

    if success:
        logger.info("Example completed successfully!")
    else:
        logger.error("Example failed!")
