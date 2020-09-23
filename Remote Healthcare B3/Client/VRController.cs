using Client.VR_Libraries;
using Client_VR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Client
{
    class VRController
    {
        private static TcpClient Client;
        private static string TunnelId;
        private VRObject vrObject;

        public VRController()
        {
            Client = new TcpClient("145.48.6.10", 6666);
            Init();

            WriteTextMessage(GenerateMessage(Scene.Get()));
            SaveObjects(ReadTextMessage(), VRObjects.BASE);

            if (vrObject.getUUID("rightHand") != string.Empty)
            {
                string parentPanel = vrObject.getUUID("rightHand");
                WriteTextMessage(GenerateMessage(Scene.Node.Add("SpeedPanel", parentPanel, new int[] { 1, 1 }, new int[] { 512, 512 }, new int[] { 1, 1, 1, 1 }, false)));
            }
            SaveObjects(ReadTextMessage(), VRObjects.PANEL);

        }

        private void Init()
        {
            vrObject = new VRObject();
            WriteTextMessage("{\"id\":\"session/list\"}");

            JObject response = ReadTextMessage(); // stap 2 (get response)
            var sessionId = response["data"][0]["id"];
            //Console.WriteLine(sessionId); // stap 3 (getting id)

            WriteTextMessage("{\"id\":\"tunnel/create\",\"data\":{\"session\":\"" + sessionId + "\"}}");

            Debug.WriteLine("Na tunnel create");

            response = ReadTextMessage(); // stap 4 (get response)
            var tunnelId = response["data"]["id"];
            TunnelId = (string)tunnelId;
            Debug.WriteLine("Init(): " + TunnelId);

            WriteTextMessage(GenerateMessage(Scene.Reset()));
            response = ReadTextMessage();
        }

        public static void WriteTextMessage(String message)
        {
            var stream = Client.GetStream();
            {
                Debug.WriteLine("WriteTextMessage():  " + message);
                int messageLength = message.Length;
                byte[] dataLength = BitConverter.GetBytes(messageLength);
                byte[] messageData = Encoding.ASCII.GetBytes(message);
                byte[] data = dataLength.Concat(messageData).ToArray();

                stream.Write(data);
                stream.Flush();
            }
        }

        public static JObject ReadTextMessage()
        {

            var stream = Client.GetStream();
            {
                Debug.WriteLine("ReadTextMessage()");
                byte[] messageLength = new byte[4];
                int length = stream.Read(messageLength, 0, 4);

                //foreach (byte b in messageLength)
                //{
                //    Console.WriteLine(b + " ");
                //}
                messageLength.Reverse();
                int messageLenghtInt = BitConverter.ToInt32(messageLength);
                Debug.WriteLine("Length: " + messageLenghtInt);

                byte[] message = new byte[messageLenghtInt];
                int totalRead = 0;

                do
                {
                    int test = stream.Read(message, totalRead, messageLenghtInt - totalRead);
                    totalRead += test;
                } while (totalRead < messageLenghtInt);

                //Console.WriteLine("Length array: " + test);

                //Console.WriteLine(Encoding.ASCII.GetString(message));

                JObject messageJson = JObject.Parse(Encoding.UTF8.GetString(message));
                Debug.WriteLine(messageJson + "\n\n\n\n");

                return messageJson;
            }
        }

        public static string GenerateMessage(JObject message)
        {
            JObject totalMessage =
                new JObject(
                    new JProperty("id", "tunnel/send"),
                    new JProperty("data",
                    new JObject(
                        new JProperty("dest", TunnelId),
                        new JProperty("data", new JObject(message)))));
            Debug.WriteLine("GenerateMessage(): " + TunnelId);

            //JObject totalMessage = new JObject(new JProperty("id", "tunnel/send"), new JProperty("data", new JObject(new JProperty("dest", TunnelId), new JProperty("data", new JObject(message)))));
            return totalMessage.ToString();
        }

        public void SetSpeed(int speed)
        {
            if (vrObject.getUUID("SpeedPanel") != string.Empty)
            {
                string panelUUID = vrObject.getUUID("SpeedPanel");
                WriteTextMessage(GenerateMessage(Scene.Panel.DrawText(panelUUID, speed.ToString(), new float[] { 100, 100 }, 32, new int[] { 0, 0, 0, 1 }, "Calibri")));
            }
        }

        public void SaveObjects(JObject json, VRObjects objectType)
        {
            string jsonString = json.ToString();
            bool nextIsName = false;
            bool nextIsUuid = false;

            string name = "";
            string uuid = "";
            JsonTextReader reader = new JsonTextReader(new StringReader(jsonString));
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (nextIsName)
                    {
                        name = (string)reader.Value;
                        nextIsName = false;

                    }
                    else if (nextIsUuid)
                    {
                        uuid = (string)reader.Value;
                        nextIsUuid = false;

                    }
                    else if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "name")
                    {
                        nextIsName = true;

                    }
                    else if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "uuid")
                    {
                        nextIsUuid = true;

                    }
                }
                if (name != "" && uuid != "")
                {
                    this.vrObject.AddVRObject(objectType, name, uuid);
                    Debug.WriteLine("Name: {0} \nuuid: {1}", name, uuid);

                    name = "";
                    uuid = "";
                }
            }
        }
    }
}
