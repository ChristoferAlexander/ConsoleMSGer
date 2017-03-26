using System;

namespace ConsoleServer
{
    [Serializable]
    class UserSentMsg
    {
        public User fromUser;
        public string toUser;
        public string msg;

        public UserSentMsg() { }

        public UserSentMsg(User fromUser, string toUser, string msg)
        {
            this.fromUser = fromUser;
            this.toUser = toUser;
            this.msg = msg;
        }
    }
}
