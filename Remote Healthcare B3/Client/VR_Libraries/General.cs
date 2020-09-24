using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client_VR
{
    class General
    {
        public static JObject Get(int serial, string head)
        {
            JObject Get = new JObject(
                new JProperty("id", "get"),
                    new JProperty("serial", serial),
                new JProperty("data",
                new JObject(
                    new JProperty("type", head)
              )));
            return Get;
        }

        public static JObject SetCallBack(int serial, string button, string trigger, string left)
        {
            JObject SetCallBack = new JObject(
                new JProperty("id", "setcallback"),
                    new JProperty("serial", serial),
                new JProperty("data",
                new JObject(
                    new JProperty("type", button),
                    new JProperty("button", trigger),
                    new JProperty("hand", left)
              )));
            return SetCallBack;
        }

        public static JObject Play(int serial)
        {
            JObject Play = new JObject(
                new JProperty("id", "play"),
                    new JProperty("serial", serial),
                new JProperty("data"));

            return Play;
        }

        public static JObject Pause(int serial)
        {
            JObject Pause = new JObject(
                new JProperty("id", "pause"),
                    new JProperty("serial", serial),
                new JProperty("data"));

            return Pause;
        }
    }
}
