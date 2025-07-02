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

string DeviceIP = "192.168.1.31"; // device IP address
int ascanLength = 4096;

// Check if the IP address is valid and device is reachable
var identified = A10xIdentity.IdentifyDevice(DeviceIP);
logger.LogInformation("Identify result: {Identified}", identified);

if (!identified)
{
    logger.LogError("Device with IP {IP} could not be identified. Please check the connection.", DeviceIP);
    return;
}

// Get device info. You can also use this method to check if the device is reachable
var devInfo = A10xIdentity.GetDeviceInfo(DeviceIP);
logger.LogInformation("Found device with IP {IP}, Serial {Serial}, MAC {MAC}, Version {Version}",
    DeviceIP,
    devInfo.Serial, devInfo.Mac, devInfo.Version);


using var sdk = new A1052SDK(logger);

// Subscribe to events to monitor what happens
sdk.NetworkConnected += connected =>
    logger.LogInformation("Network Connected: {Connected}", connected);
sdk.NetworkDisconnected += () =>
    logger.LogInformation("Network Disconnected");

sdk.AscanDataReceived += (array, length, time) =>
{
    int ascansCount = length / ascanLength;
    logger.LogInformation("Received A-Scan data of overal length {Length} samples at {Time}, number of single ascans {AscansCount}", length, time, ascansCount);
};

sdk.MasterBatteryInfoReceived += (BatteryResult batteryInfo) =>
{
    logger.LogInformation("Received Battery Info: {BatteryInfo}", batteryInfo.ToInfoString());
};

logger.LogInformation("Starting SDK...");
sdk.Connect(DeviceIP);

logger.LogInformation("SDK Status - IsRunning: {IsRunning}, IsCancelled: {IsCancelled}", sdk.IsConnected, sdk.IsCommunicationCancelled);

sdk.SetGain(10);
sdk.SetPulseRepetitionRate(3);
sdk.SetAscanAveraging(5);
sdk.SetQuadro8x4Transmitter(0);
sdk.SetSingle8x4Transmitter(0);
// matrix
sdk.StartAscanSingleTransmitter();
logger.LogInformation("Started A-Scan with Single transmitter, waiting for data...");

// Wait and monitor status changes
for (int i = 0; i < 5; i++) // Check for 5 seconds total
{
    await Task.Delay(1000);
    sdk.RequestBatteryInfo();

    // Check if the timeout triggered cancellation
    if (sdk.IsCommunicationCancelled && !sdk.IsConnected)
    {
        logger.LogInformation("SDK was cancelled and stopped running.");
        break;
    }
}

sdk.StopAcquisition();
logger.LogInformation("Acquisition stopped, waiting for 500ms...");
await Task.Delay(500);

// linear
sdk.StartAscanQuadroTransmitter();
logger.LogInformation("Started A-Scan with Quadro transmitter, waiting for data...");
for (int i = 0; i < 5; i++) // Check for 5 seconds total
{
    await Task.Delay(1000);
    sdk.RequestBatteryInfo();

    // Check if the timeout triggered cancellation
    if (sdk.IsCommunicationCancelled && !sdk.IsConnected)
    {
        logger.LogInformation("SDK was cancelled and stopped running.");
        break;
    }
}

// Wait a bit more to see if anything else happens
await Task.Delay(1000);

// Disconned here is for example. It will be also called in Dispose method, no need to call it explicitly
sdk.Disconnect();
await Task.Delay(500);

logger.LogInformation("Press any key to exit...");
Console.ReadKey();
