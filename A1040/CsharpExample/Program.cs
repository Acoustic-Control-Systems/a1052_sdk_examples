using A10x_SDK;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

// Setup logger factory and logger
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(LogLevel.Debug)
        .AddSimpleConsole(options =>
        {
            options.ColorBehavior = LoggerColorBehavior.Enabled;
            options.SingleLine = true;
            options.TimestampFormat = "HH:mm:ss.fff ";
        });
});
var logger = loggerFactory.CreateLogger<Program>();

string MasterIP = "192.168.1.31"; // master device IP address
int ascanLength = 4096;

// Check if the IP address is valid and device is reachable
var identified = A10xIdentity.IdentifyDevice(MasterIP);
logger.LogInformation("Identify result: {Identified}", identified);

if (!identified)
{
    logger.LogError("Device with IP {IP} could not be identified. Please check the connection.", MasterIP);
    return;
}

// Get device info
var devInfo = A10xIdentity.GetDeviceInfo(MasterIP);
logger.LogInformation("Found device with IP {IP}, Serial {Serial}, MAC {MAC}, Version {Version}",
    MasterIP,
    devInfo.Serial, devInfo.Mac, devInfo.Version);

using var sdk = new A1040SDK(logger);

// Subscribe to events to monitor what happens
sdk.NetworkConnected += connected =>
    logger.LogInformation("Network Connected: {Connected}", connected);
sdk.NetworkDisconnected += () =>
    logger.LogInformation("Network Disconnected");

sdk.AscanDataReceived += (array, length, time) =>
{
    int ascansCount = length / ascanLength;
    logger.LogInformation("Received A-Scan data of overall length {Length} samples at {Time}, number of single ascans {AscansCount}",
        length, time, ascansCount);
};

sdk.MasterBatteryInfoReceived += (BatteryResult batteryInfo) =>
{
    logger.LogInformation("Received Master Battery Info: {BatteryInfo}", batteryInfo.ToInfoString());
};

sdk.SlaveBatteryInfoReceived += (BatteryResult batteryInfo) =>
{
    logger.LogInformation("Received Slave Battery Info: {BatteryInfo}", batteryInfo.ToInfoString());
};

logger.LogInformation("Connecting in 8x4 mode...");
sdk.Connect8x4(MasterIP);

logger.LogInformation("SDK Status - IsRunning: {IsRunning}, IsCancelled: {IsCancelled}", sdk.IsConnected, sdk.IsCommunicationCancelled);

sdk.SetGain(10);
sdk.SetPulseRepetitionRate(3);
sdk.SetAscanAveraging(5);

// Demonstrate laser control
sdk.SetLaser(true);
logger.LogInformation("Laser enabled");
await Task.Delay(1000);
sdk.SetLaser(false);
logger.LogInformation("Laser disabled");

// Single transmitter mode
sdk.SetSingle8x4Transmitter(0);
sdk.StartAscanSingleTransmitter();
logger.LogInformation("Started A-Scan with Single transmitter, waiting for data...");

for (int i = 0; i < 5; i++)
{
    await Task.Delay(1000);
    sdk.RequestBatteryInfo();

    if (sdk.IsCommunicationCancelled && !sdk.IsConnected)
    {
        logger.LogInformation("SDK was cancelled and stopped running.");
        break;
    }
}

sdk.StopAcquisition();
logger.LogInformation("Acquisition stopped, waiting for 500ms...");
await Task.Delay(500);

// Quadro transmitter mode
sdk.SetQuadro8x4Transmitter(0);
sdk.StartAscanQuadroTransmitter();
logger.LogInformation("Started A-Scan with Quadro transmitter, waiting for data...");

for (int i = 0; i < 5; i++)
{
    await Task.Delay(1000);
    sdk.RequestBatteryInfo();

    if (sdk.IsCommunicationCancelled && !sdk.IsConnected)
    {
        logger.LogInformation("SDK was cancelled and stopped running.");
        break;
    }
}

await Task.Delay(1000);
sdk.Disconnect();
await Task.Delay(500);

// --- 16x4 mode (paired devices) ---
// Uncomment and adapt the following section to use 16x4 mode:
//
// string SlaveIP = "192.168.1.32"; // slave device IP address
// using var sdk16x4 = new A1040SDK(logger);
// sdk16x4.NetworkConnected += connected =>
//     logger.LogInformation("16x4 Network Connected: {Connected}", connected);
// sdk16x4.AscanDataReceived += (array, length, time) =>
//     logger.LogInformation("16x4 Received A-Scan data of length {Length} samples at {Time}", length, time);
// sdk16x4.SlaveBatteryInfoReceived += (BatteryResult batteryInfo) =>
//     logger.LogInformation("16x4 Slave Battery: {BatteryInfo}", batteryInfo.ToInfoString());
//
// logger.LogInformation("Connecting in 16x4 mode...");
// sdk16x4.Connect16x4(MasterIP, SlaveIP);
// sdk16x4.SetGain(10);
// sdk16x4.SetPulseRepetitionRate(3);
// sdk16x4.SetAscanAveraging(5);
// sdk16x4.SetSingle16x4Transmitter(0);
// sdk16x4.SetLaser(true);
// await Task.Delay(1000);
// sdk16x4.SetLaser(false);
// sdk16x4.StartAscanSingleTransmitter();
// await Task.Delay(5000);
// sdk16x4.RequestBatteryInfo();
// sdk16x4.RequestSlaveBatteryInfo();
// sdk16x4.StopAcquisition();
// sdk16x4.Disconnect();

// --- 8x8 mode (paired devices) ---
// Uncomment and adapt the following section to use 8x8 mode:
//
// string SlaveIP8x8 = "192.168.1.32"; // slave device IP address
// using var sdk8x8 = new A1040SDK(logger);
// sdk8x8.NetworkConnected += connected =>
//     logger.LogInformation("8x8 Network Connected: {Connected}", connected);
// sdk8x8.AscanDataReceived += (array, length, time) =>
//     logger.LogInformation("8x8 Received A-Scan data of length {Length} samples at {Time}", length, time);
// sdk8x8.SlaveBatteryInfoReceived += (BatteryResult batteryInfo) =>
//     logger.LogInformation("8x8 Slave Battery: {BatteryInfo}", batteryInfo.ToInfoString());
//
// logger.LogInformation("Connecting in 8x8 mode...");
// sdk8x8.Connect8x8(MasterIP, SlaveIP8x8);
// sdk8x8.SetGain(10);
// sdk8x8.SetPulseRepetitionRate(3);
// sdk8x8.SetAscanAveraging(5);
// sdk8x8.SetSingle8x8Transmitter(0);
// sdk8x8.SetLaser(true);
// await Task.Delay(1000);
// sdk8x8.SetLaser(false);
// sdk8x8.StartAscanSingleTransmitter();
// await Task.Delay(5000);
// sdk8x8.RequestBatteryInfo();
// sdk8x8.RequestSlaveBatteryInfo();
// sdk8x8.StopAcquisition();
// sdk8x8.Disconnect();
