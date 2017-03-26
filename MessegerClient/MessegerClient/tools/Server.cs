using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;

namespace MessegerClient
{
    class Server
    {
        private TcpListener mainTcpListener;
        // Server status
        private Boolean _isRunning;
        // Server instance constructor 
        public Server(int port)
        {
            mainTcpListener = new TcpListener(IPAddress.Any, port);
            mainTcpListener.Start();
            _isRunning = true;
            StartServer();
        }
        // Starts thread that handles incoming TCP requests 
        public void StartServer()
        {
            Thread serverThread = new Thread(new ThreadStart(LoopClients));
            serverThread.IsBackground = true;
            serverThread.Start();
        }
        // The method the serverThread runs ( waits for incoming TCP connections )
        public void LoopClients()
        {
            // Loop incoming TCP requests 
            while (_isRunning)
            {
                try
                {
                    // Check for TCP connection to accept
                    TcpClient newClient = mainTcpListener.AcceptTcpClient();
                    // Create thread to handle connection
                    Thread serverConnectionHandleThread = new Thread(new ParameterizedThreadStart(HandleConnection));
                    serverConnectionHandleThread.Start(newClient);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.GetBaseException().ToString());
                }
            }
        }
        // Stops thread
        public void StopServer()
        {
            _isRunning = false;
        }
        // The method the serverRequestThread/s runs ( Handles server request)
        public void HandleConnection(object obj)
        {
            try
            {
                // Get stream from TCP client and parse to String
                TcpClient client = obj as TcpClient;
                StreamReader sReader = new StreamReader(client.GetStream());
                String sData = sReader.ReadToEnd();              
                String clientIP = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                sReader.Close();
                client.Close();
                // Handles server request depending value
                if (sData.Substring(0, 5) == "lgdin")
                {                   
                    Program.logedin = true;
                    Console.WriteLine("-- Loged In --" + "\n");
                }
                else if (sData.Length >= 3 && sData.Substring(0, 3) == "crt")
                {
                    Program.myName = sData.Substring(3, sData.Length - 3);
                    Console.WriteLine("\n" + "-- Account Created --");
                    Console.WriteLine("WELCOME " + Program.myName);                   
                }
                else if(sData.Substring(0, 6) == "lgdout")
                {
                    Console.WriteLine("\n" + "-- Loged Out --" + "\n");
                    Program.myName = null;
                    Program.pass = null;
                    Program.reciever = null;
                    Program.logedin = false;
                }              
                else
                {
                    Console.WriteLine(sData);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException().ToString());
            }
        }
        //Cleans malformed strings
        public string Strip(string text)
        {
            return Regex.Replace(text, @"<(.|\nr)/*?>", string.Empty);
        }
    }
}
