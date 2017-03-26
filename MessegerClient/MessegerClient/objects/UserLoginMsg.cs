using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessegerClient
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
