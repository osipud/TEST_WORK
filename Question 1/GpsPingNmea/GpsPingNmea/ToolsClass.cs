using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;


    public class NetTools
    {
        

        /// <summary>
        /// Функция отправки ICMP запроса и получения в ms ответа от сервера.
        /// </summary> 
        public long PingRoundTripTime(string IPAddress)
        {
            long ResultOutput = 0;
            try
            {
                Ping ping = new Ping();
                PingReply reply = ping.Send(IPAddress);
                ResultOutput = reply.RoundtripTime;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ResultOutput;
        }

        public async IAsyncEnumerable<string[]> MmeaMessageReceiverAsync(string IPAddressDomain, int IPPort)
        {
            using TcpClient client = new();
            var ipEndPoint = new IPEndPoint(IPAddress.Parse(IPAddressDomain), IPPort);
            await client.ConnectAsync(ipEndPoint);
            await using NetworkStream stream = client.GetStream();
            while (true) 
            {
                
                await Task.Delay(500);
                //public static async Task MmeaMessageReceiver(string IPAddressDomain, int IPPort
                var buffer = new byte[1_024];
                int received = await stream.ReadAsync(buffer);
                string message = Encoding.UTF8.GetString(buffer, 0, received);
                string[] words = message.Split(new char[] { '\r', '\n', '"'});   
                yield return words;
            }
        }


    }
    public class FileIoTools
    {

        /// <summary>
        /// Функция записи данных в текстовый файл в формате CSV.
        /// </summary> 
        public void RecordToTxtFile(string FileName, double latitude, double Longitude, long RoundtripTime)
        {
            string OutCombo = latitude + ";" + Longitude +";"+ RoundtripTime;
            using (StreamWriter print = new StreamWriter(FileName + ".txt", true))
            {
                print.WriteLine(OutCombo);
            }
        }

        public void RecordToTxtFileString(string FileName, string Data)
        {
            using (StreamWriter print = new StreamWriter(FileName + ".txt", true))
            {
                print.WriteLine(Data);
            }

        }

        public int GetRandomNumber()
        {
            //Создание объекта для генерации чисел
            Random rnd = new Random();

            //Получить очередное (в данном случае - первое) случайное число
            int value = rnd.Next();

            return value;
        }
    
        public void RecordToGeoJsonFileString(string FileName, string Data)
        {
            using (StreamWriter print = new StreamWriter(FileName + ".geojson"))
            {
                print.WriteLine(Data);
            }
        }


    }

    


