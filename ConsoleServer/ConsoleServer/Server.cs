using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft;
using System.Text.RegularExpressions;

namespace ConsoleApplication1
{
    public class Server
    {

        private TcpListener mainTcpListener;
        private Boolean _isRunning;
        private Utils utils = new Utils();
        public List<User> userList = new List<User>();
        public Server(int port)
        {
            mainTcpListener = new TcpListener(IPAddress.Any, port);
            mainTcpListener.Start();
            _isRunning = true;
            StartServer();
        }

        public void StartServer()
        {
            Thread serverThread = new Thread(new ThreadStart(LoopClients));
            serverThread.IsBackground = true;
            serverThread.Start();
        }

        public void LoopClients()
        {
            while (_isRunning)
            {
                try
                {
                    TcpClient newClient = mainTcpListener.AcceptTcpClient();
                    Thread t1 = new Thread(new ParameterizedThreadStart(HandleClient));
                    t1.Start(newClient);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.GetBaseException().ToString());
                }
            }
        }
        public void StopServer()
        {
            _isRunning = false;
        }
        public void HandleClient(object obj)
        {
            try
            {
                TcpClient client = obj as TcpClient;
                StreamReader sReader = new StreamReader(client.GetStream());
                String sData = sReader.ReadToEnd();
                String clientIP = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                sReader.Close();
                client.Close();

                Console.Write(sData);

                JMessage message = new JMessage();
                message = JMessage.Deserialize(sData);
                if (message.Type == 0)
                {
                    UserSentMsg sentMSG = message.Value.ToObject<UserSentMsg>();
                    User fromUser = sentMSG.fromUser;
                    string userName = sentMSG.toUser;
                    string msg = sentMSG.msg;

                    userSentMsg(fromUser, userName, msg);
                }
                else if (message.Type == 1)
                {
                    UserLoginMsg loginMSG = message.Value.ToObject<UserLoginMsg>();
                    User user = loginMSG.user;

                    userLogin(user);
                }
                else if (message.Type == 2)
                {
                    UserLogoutMsg logoutMSG = message.Value.ToObject<UserLogoutMsg>();
                    User user = logoutMSG.user;

                    userLogout(user);
                }
                else if (message.Type == 3)
                {
                    UserCreateMsg createMSG = message.Value.ToObject<UserCreateMsg>();
                    User user = createMSG.user;

                    userCreate(user);
                }
                else
                {
                    Console.WriteLine("UNKNOWN MSM TYPE");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException().ToString());
            }
        }

        public void userSentMsg(User fromUser, string userName, string msg)
        {
            if (userList.Exists(x => x.name == userName))
            {
                User toUser = userList.Find(x => x.name == userName);
                if (toUser.isOnline == true)
                {
                    utils.sent(toUser.ip, toUser.port, Strip(fromUser.name) + ": " + msg);

                }
                else
                {
                    utils.sent(fromUser.ip, fromUser.port, toUser.name + " is offline !");
                }
            }
            else
            {
                utils.sent(fromUser.ip, fromUser.port, "User doesn't exists ! ! !");
            }
        }

        public void userCreate(User user)
        {
            if (userList.Exists(x => x.name == user.name) == false)
            {
                userList.Add(user);
                utils.sent(user.ip, user.port, "s*/" + user.name);
                Console.WriteLine("User created :" + user.name + " " + user.ip + ":" + user.port);
            }
            else
            {
                utils.sent(user.ip, user.port, "User already exists ! ! !");
            }
        }



        public void userLogin(User user)
        {
            if (userList.Exists(x => x.name == user.name))
            {
                if (user.pass == userList.Find(x => x.name == user.name).pass)
                {
                    userList.Find(x => x.name == user.name).isOnline = true;
                    userList.Find(x => x.name == user.name).port = user.port;
                    userList.Find(x => x.name == user.name).ip = user.ip;
                    utils.sent(user.ip, user.port, "lgdin");
                    Console.WriteLine("User loged in :" + user.name + " " + user.ip + ":" + user.port + "  STATUS: " + userList.Find(x => x.name == user.name).isOnline.ToString());
                }
                else
                {
                    utils.sent(user.ip, user.port, "Wrong password ! ! !");
                }
            }
            else
            {
                utils.sent(user.ip, user.port, "User doen't exists ! ! !");
            }
        }

        public void userLogout(User user)
        {
            if (userList.Exists(x => x.name == user.name))
            {
                userList[userList.Select(T => T.name).ToList().IndexOf(user.name)].isOnline = false;
                utils.sent(user.ip, user.port, "Loged Out");
                Console.WriteLine("User loged out :" + user.name + " " + user.ip + ":" + user.port);
            }
            else
            {
                utils.sent(user.ip, user.port, "Already loged out ! ! !");
            }
        }

        public string Strip(string text)
        {
            return Regex.Replace(text, @"<(.|\nr)*?>", string.Empty);
        }

    }
}
