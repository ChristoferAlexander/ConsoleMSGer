using System;

namespace ConsoleServer
{
    [Serializable]
    class UserLoginMsg
    {
        public User user;

        public UserLoginMsg(User user)
        {
            this.user = user;
        }
    }
}
