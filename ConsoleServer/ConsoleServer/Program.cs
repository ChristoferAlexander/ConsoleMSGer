using System;

namespace ConsoleServer
{
    class Program
    {
        public static Server server;
        static void Main(string[] args)
        {
            server = new Server(7777);          
            bool quit = false;
            Console.WriteLine("Press 'q' to quit");

            while (quit == false)
            {
                string cmd;
                cmd = Console.ReadLine();
                
                if(cmd == "quit" || cmd == "q")
                {
                    server.StopServer();
                    quit = true;
                }
            }
        }
    }
}
