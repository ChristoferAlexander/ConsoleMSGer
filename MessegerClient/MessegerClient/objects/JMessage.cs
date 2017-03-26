using MessegerClient;
using Newtonsoft.Json.Linq;
using System;

[Serializable]
class JMessage
{
    public int Type { get; set; }
    public JToken Value { get; set; }

    public static JMessage FromValue<T>(T value)
    {
        int type = -1;
        if (value.GetType() == typeof(UserSentMsg))
        {
            type = 0;
        }
        else if (value.GetType() == typeof(UserLoginMsg))
        {
            type = 1;
        }
        else if (value.GetType() == typeof(UserLogoutMsg))
        {
            type = 2;
        }
        else if (value.GetType() == typeof(UserCreateMsg))
        {
            type = 3;
        }
        return new JMessage { Type = type, Value = JToken.FromObject(value) };
    }

    public string Serialize(JMessage message)
    {
        return JToken.FromObject(message).ToString();
    }

    public JMessage Deserialize(string data)
    {
        return JToken.Parse(data).ToObject<JMessage>();
    }
}