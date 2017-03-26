using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessegerClient
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
