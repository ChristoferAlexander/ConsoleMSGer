using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleServer
{
    public static class Utils
    {       
        static Utils() { }
        // Starts a thread that sents a msg over a TCP connection
        public static void sent(string IP, int PORT, string MSG)
        {          
            Thread sentMsgThread = new Thread(() => tcpSent(IP, PORT, MSG));
            sentMsgThread.IsBackground = true;
            sentMsgThread.Start();

        }

        // Method that sents a msg over a TCP connection (max tries 10, timeout 300ms)
        public static void tcpSent(string ip, int port, string msg)
        {
            bool exit = false;

            TcpClient client;
            StreamWriter sWriter;

            int loop = 0;

            while (exit == false)
            {
                try
                {
                    if(loop < 10)
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
                        // If a msg fails to deliver logout msg reviever user
                        if (Program.server.userList.Exists(x => x.ip == ip))
                        {
                            Program.server.userList.Find(x => x.ip == ip).isOnline = false;
                            
                        }
                        exit = true;
                    }                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.GetBaseException().ToString());
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

        // Deseriazizes string to JToken
        internal static JMessage Deserialize(string data)
        {
            return JToken.Parse(data).ToObject<JMessage>();
        }
    }
}
