using System;

namespace MessegerClient
{
    class Program
    {
        // Our Ip/Port
        public static string ip;
        public static int port;
        // Server IP/Port
        public static string serverIp = "192.168.1.2";
        public static int serverPort = 7777;
        // Our name, password and status
        public static string myName;
        public static string pass;
        public static bool logedin = false;
        // Msg reciever name
        public static string reciever = null;
        // Program helping variables
        public static bool portSelected = false;
        public static bool quit = false;
        static void Main(string[] args)
        {
            // Input port to use
            Console.WriteLine("Welcome !!!" + "\n");
            Console.WriteLine("Enter port of your choice :");
            ip = Utils.GetPublicIP();

            while (portSelected == false)
            {
                try
                {
                    port = int.Parse(Console.ReadLine());

                    Server server = new Server(port);
                    portSelected = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("\n" + "Port invalid , please select another port ...");                
                }

            }

            Console.WriteLine("\n" + "Type help for more info ..");
            // Loop user commnads until user exits
            while (quit == false)
            {
                string command = Console.ReadLine();

                if (command == "create")
                {                  
                    Console.WriteLine("\n" + "-- Acount Creation --");
                    Console.WriteLine("Enter user name :");
                    string userName = Console.ReadLine();
                    Console.WriteLine("Enter password :");
                    string password = Console.ReadLine();
                    if(userName.Length >= 3) { 
                        // Create User Obj to pass to UserCreateMsg Obj
                        User user = new User(userName, Utils.GetPublicIP(), port, password);
                        UserCreateMsg userMsg = new UserCreateMsg(user);
                        // Parse UserCreateMsg to Json
                        string msg = Utils.Serialize(JMessage.FromValue(userMsg));      
                        // Sent Json formated msg to server            
                        Utils.sent(serverIp, serverPort, msg);
                    }
                    else
                    {
                        Console.WriteLine("\n" + "Name must be at least 3 chars ...");
                    }
                }
                else if (command == "login")
                {
                    if(logedin == false)
                    {
                        Console.WriteLine("User Name :");
                        string userName = Console.ReadLine();
                        Console.WriteLine("Password :");
                        string password = Console.ReadLine();
                        Console.WriteLine();
                        myName = userName;
                        pass = password;
                        // Create User Obj to pass to UserLoginMsg Obj
                        User user = new User(userName, ip, port, password);
                        UserLoginMsg userMsg = new UserLoginMsg(user);
                        // Parse UserLoginMsg to Json
                        string msg = Utils.Serialize(JMessage.FromValue(userMsg));
                        // Sent Json formated msg to server     
                        Utils.sent(serverIp, serverPort, msg);
                    }
                    else
                    {
                        Console.WriteLine("Please logout first ..." + "\n");
                    }                                 
                }
                else if (command.Length >= 3 && command.Substring(0, 3) == "to ")
                {
                    reciever = command.Substring(3, command.Length - 3);
                    Console.WriteLine("SELECTED RECIEVER : " + reciever + "\n");

                }
                else if (command == "logout")
                {
                    if (logedin == true && myName != null) {
                        // Create User Obj to pass to UserLogoutMsg Obj
                        User user = new User(myName, ip, port, pass);
                        UserLogoutMsg userMsg = new UserLogoutMsg(user);
                        // Parse UserLogoutMsg to Json
                        string msg = Utils.Serialize(JMessage.FromValue(userMsg));
                        // Sent Json formated msg to server     
                        Utils.sent(serverIp, serverPort, msg);                      
                    }
                    else
                    {
                        Console.WriteLine("You are already Loged Out ..." + "\n");
                    }
                }
                else if (command == "exit")
                {
                    quit = true;
                }
                else if (command == "help")
                {
                    Console.WriteLine("\n" + "-- COMMANDS --" + "\n");
                    Console.WriteLine("create    - CREATE NEW ACCOUNT");
                    Console.WriteLine("login     - LOGIN TO SERVER");
                    Console.WriteLine("logout    - LOGOUT FROM SERVER");                   
                    Console.WriteLine("to 'NAME' - SETS RECIEVER");
                    Console.WriteLine("clear     - CLEARS LOG");
                    Console.WriteLine("changesip - CHANGES SERVER IP");
                    Console.WriteLine("exit      - EXITS PROGRAM");
                    Console.WriteLine("help      - THIS MANUAL . . ." + "\n");
                }
                else if (command == "changeSIP")
                {
                    Console.WriteLine("Enter server IP :");
                    serverIp = Console.ReadLine();
                    Console.WriteLine();
                }
                else if (command == "clear")
                {
                    Console.Clear();                  
                }
                else
                {
                    // Sent msg to an other user
                    if(logedin == true)
                    {
                        if (reciever != null && myName !=null)
                        {
                            // Create User Obj to pass to UserSentMsg Obj                                                                                           
                            User user = new User(myName, ip, port, "");
                            UserSentMsg userMsg = new UserSentMsg(user, reciever, command);
                            // Parse UserSentMsg to Json
                            string msg = Utils.Serialize(JMessage.FromValue(userMsg));
                            // Sent Json formated msg to server     
                            Utils.sent(serverIp, serverPort, msg);
                        }
                        else
                        {
                            Console.WriteLine("\n" + "Please select a RECIEVER ...");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n" + "Please login first ...");
                    }                    
                }

            }
                     
        }
    }
}
