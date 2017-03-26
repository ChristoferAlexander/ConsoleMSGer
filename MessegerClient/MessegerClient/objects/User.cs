using System;

namespace MessegerClient
{
    [Serializable]
    public class User
    {
        public string name { get; set; }
        public string ip { get; set; }
        public int port { get; set; }
        public bool isOnline { get; set; }
        public string pass { get; set; }

        public User(string name, string ip, int port, string pass)
        {
            this.name = name;
            this.port = port;
            this.ip = ip;
            this.pass = pass;
        }
    }
}
