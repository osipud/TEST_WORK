using Ghostware.NMEAParser;
using Ghostware.NMEAParser.Enums;
using Ghostware.NMEAParser.Extensions;
using Ghostware.NMEAParser.NMEAMessages;
using System.Net.Sockets;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using GpsPingNmea;
using System.Net.NetworkInformation;
using System.Diagnostics;
using Newtonsoft.Json.Linq;


NmeaParser parser = new();
NetTools netTools = new();
FileIoTools fileIoTools= new();
string NmeaIPServer = "127.0.0.1";
if (args[0] == null)
{
    NmeaIPServer = args[0];
}
else if (args[0] != null)
{

}

using TcpClient client = new();
var ipEndPoint = new IPEndPoint(IPAddress.Parse(args[0]), 50000);
await client.ConnectAsync(ipEndPoint);
await using NetworkStream stream = client.GetStream();

RootGeo rootGeo = new();
List<Feature> features = new List<Feature>();
bool BuiFlags = false;
string OutputFileName = String.Concat(fileIoTools.GetRandomNumber() + " - geo");

while (true)
{
    await Task.Delay(500);
    var buffer = new byte[1_024];
    int received = await stream.ReadAsync(buffer);
    string message = Encoding.UTF8.GetString(buffer, 0, received);

    var GpsData = parser.Parse(message);
    var GpsInfo = (GpggaMessage)GpsData;

    if (GpsInfo != null)
    {
        string YandexDNS = Convert.ToString(netTools.PingRoundTripTime("77.88.8.8"));
        string GoogleDNS = Convert.ToString(netTools.PingRoundTripTime("8.8.8.8"));


        Feature feature_ = new Feature();
        Geometry geometry_ = new Geometry();
        Properties properties_ = new Properties();

        if (BuiFlags == false) 
        {
            List<object> coordinates = new();
            coordinates.Add(GpsInfo.Longitude);
            coordinates.Add(GpsInfo.Latitude);
            geometry_.type = "Point";
            geometry_.coordinates = coordinates;
            BuiFlags = true;
        }
        else
        {
            geometry_.type = "LineString";
            List<object> CoordinateObject1 = new();
            List<object> CoordinateObject2 = new();
            List<object> coordinates = new();
            CoordinateObject1.Add(GpsInfo.Longitude);
            CoordinateObject1.Add(GpsInfo.Latitude);
            coordinates.Add(CoordinateObject1);
            CoordinateObject2.Add(GpsInfo.Longitude);
            CoordinateObject2.Add(GpsInfo.Latitude);
            coordinates.Add(CoordinateObject2);
            geometry_.coordinates = coordinates;
            Debug.WriteLine("LineString");
            BuiFlags = false;
        }
        Console.WriteLine($"Время: {DateTime.Now} Количество спустников: {GpsInfo.NumberOfSatellites} Долгота: {GpsInfo.Longitude} Широта: {GpsInfo.Latitude} PING: GOOGLE:{GoogleDNS}, YANDEX:{YandexDNS}");
        fileIoTools.RecordToTxtFileString("log", $"{DateTime.Now};{GpsInfo.NumberOfSatellites};{GpsInfo.Longitude};{GpsInfo.Latitude};{GoogleDNS};{YandexDNS}");
        properties_.name = $"PING: GOOGLE:{GoogleDNS}, YANDEX:{YandexDNS}";

        feature_.properties = properties_;
        feature_.geometry = geometry_;

        features.Add(feature_);

    }
    rootGeo.features = features;

    string json = JsonConvert.SerializeObject(rootGeo);

    fileIoTools.RecordToGeoJsonFileString(OutputFileName, json)
}
