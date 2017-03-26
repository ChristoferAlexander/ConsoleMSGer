using System;

namespace ConsoleServer
{
    [Serializable]
    class UserLogoutMsg
    {
        public User user;

        public UserLogoutMsg(User user)
        {
            this.user = user;
        }
    }
}
