using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MessegerClient
{
    public static class Utils
    {
        static string ip;
        static int port;
        static string msg; 
        static Utils() { }
        // Starts a thread that sents a msg over a TCP connection
        public static void sent(string IP, int PORT, string MSG)
        {
            ip = IP;
            port = PORT;
            msg = MSG;
            Thread serverThread = new Thread(new ThreadStart(tcpSent));
            serverThread.IsBackground = true;
            serverThread.Start();
        }
        // Method that sents a msg over a TCP connection (max tries 4, timeout 300ms)
        public static void tcpSent()
        {
            bool exit = false;

            TcpClient client;
            StreamWriter sWriter;

            int loop = 0;

            while (exit == false)
            {
                try
                {
                    if (loop < 4)
                    {
                        loop++;
                        client = new TcpClient();
                        client.Connect(ip, port);
                        sWriter = new StreamWriter(client.GetStream());
                        sWriter.WriteLine(msg);
                        sWriter.Close();
                        client.Close();
                        exit = true;
                    }
                    else
                    {
                        Console.WriteLine("Default server IP " + Program.serverIp + " can not be reached, please change to correct IP using 'changesip' command and make sure server is up and running ...");
                        exit = true;    
                    }
                               
                }
                catch (Exception)
                {
                    Thread.Sleep(300);                
                }

            }
        }
        // Returns host machine IP
        public static string GetPublicIP()
        {          
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("IP Address Not Found!");
        }
        // Serializes JMessage Obj to JToken
        internal static string Serialize(JMessage message)
        {
            return JToken.FromObject(message).ToString();
        }
        // Deseriazize's string to JToken
        internal static JMessage Deserialize(string data)
        {
            return JToken.Parse(data).ToObject<JMessage>();
        }

    }
}
