using System;

namespace ConsoleServer
{
    [Serializable]
    class UserCreateMsg
    {
        public User user { get; set; }

        public UserCreateMsg(User user)
        {
            this.user = user;
        }
    }
}
