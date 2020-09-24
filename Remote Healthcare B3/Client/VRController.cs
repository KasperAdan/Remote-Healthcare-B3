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
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Client
{
    class VRController
    {
        private static TcpClient Client;
        private static NetworkStream Stream;
        private static string TunnelId;
        private static byte[] Buffer = new byte[1024];
        private static string TotalBuffer = "";
        private VRObject vrObject;
        private List<JObject> ServerResponses;
        private bool Connected = false;
        private int messageLength = 100000;

        public VRController()
        {
            Client = new TcpClient();
            Client.BeginConnect("145.48.6.10", 6666, new AsyncCallback(OnConnect), null);
            this.ServerResponses = new List<JObject>(10000);
            this.ServerResponses.Add(null);
            Console.WriteLine(ServerResponses.Count);
            while (!Connected){}
            Init();

            WriteTextMessage(GenerateMessage(Scene.Get(1)));
            JObject response = this.ServerResponses[1];
            Console.WriteLine(response.ToString());
            SaveObjects(response, VRObjects.BASE);

            if (vrObject.getUUID("rightHand") != string.Empty)
            {
                string parentPanel = vrObject.getUUID("rightHand");
                WriteTextMessage(GenerateMessage(Scene.Node.Add(3, "SpeedPanel", parentPanel, new int[] { 1, 1 }, new int[] { 512, 512 }, new int[] { 1, 1, 1, 1 }, false)));
            }
            response = this.ServerResponses[3];
            SaveObjects(response, VRObjects.PANEL);

        }


        private void Init()
        {
            vrObject = new VRObject();
            WriteTextMessage("{\"id\":\"session/list\"}");
            Console.WriteLine("\n\n\n send \n\n\n");
            while (this.ServerResponses[0] == null){}
            JObject value = this.ServerResponses[0];
            Console.WriteLine(value.ToString());
            // stap 2 (get response)
            var properSession = value["data"].Where(e => e["clientinfo"]["user"].ToObject<string>() == Environment.UserName).Last();
            var sessionId = properSession["id"];
            this.ServerResponses[0] = null;
            //Console.WriteLine(sessionId); // stap 3 (getting id)

            WriteTextMessage("{\"id\":\"tunnel/create\",\"data\":{\"session\":\"" + sessionId + "\"}}");

            Debug.WriteLine("Na tunnel create");
            while (this.ServerResponses[0] == null){} 

            JObject response = this.ServerResponses[0];
            Console.WriteLine(response.ToString());
            // stap 4 (get response)
            var tunnelId = response["data"]["id"];
            TunnelId = (string)tunnelId;
            Debug.WriteLine("Init(): " + TunnelId);
            this.ServerResponses[0] = null;

            WriteTextMessage(GenerateMessage(Scene.Reset(2)));
        }

        private void OnConnect(IAsyncResult ar)
        {
            Client.EndConnect(ar);
            Stream = Client.GetStream();
            Stream.BeginRead(Buffer, 0, Buffer.Length, new AsyncCallback(OnRead), null);
            this.Connected = true;
        }

        //private void OnRead(IAsyncResult ar)
        //{
        //    int receivedBytes = Stream.EndRead(ar);
        //    string receivedText = Encoding.ASCII.GetString(Buffer, 0, receivedBytes);
        //    TotalBuffer += receivedText;

        //    string length = TotalBuffer.Substring(0, 4);
        //    byte[] messageLength = Encoding.ASCII.GetBytes(length);
        //    messageLength.Reverse();
        //    int messageLengthInt = BitConverter.ToInt32(messageLength);

        //    while (TotalBuffer.Length >= messageLengthInt+4)
        //    {
        //        string packet = TotalBuffer.Substring(4, messageLengthInt);
        //        TotalBuffer = TotalBuffer.Substring(messageLengthInt+4);
        //        JObject messageJson = JObject.Parse(packet);
        //        Console.WriteLine(messageJson);
        //        Debug.WriteLine(messageJson + "\n\n\n\n");
        //        int serial;
        //        try
        //        {
        //            serial = (int)messageJson["data"]["data"]["serial"];
        //        }
        //        catch (Exception)
        //        {
        //            serial = 9999;
        //        }
        //        this.ServerResponses[serial] = messageJson;
        //        this.messageLength = 10000000;
        //    }
        //    Stream.BeginRead(Buffer, 0, Buffer.Length, new AsyncCallback(OnRead), null);
        //}

        private void OnRead(IAsyncResult ar)
        {
            Console.WriteLine("\n\n\n recieve \n\n\n");
            int receivedBytes = Stream.EndRead(ar);
            if (receivedBytes == 4 && TotalBuffer.Length < 10)
            {
                byte[] messageLength = Buffer;
                Console.WriteLine(messageLength);
                messageLength.Reverse();
                int messageLengthInt = BitConverter.ToInt32(messageLength);
                this.messageLength = messageLengthInt;
                Console.WriteLine($"\n\n\n length: {messageLengthInt} \n\n\n");
                Console.WriteLine(TotalBuffer);
                TotalBuffer = "";
            }
            else
            {
                string receivedText = Encoding.ASCII.GetString(Buffer, 0, receivedBytes);
                TotalBuffer += receivedText;
            }


            if (TotalBuffer.Length >= this.messageLength)
            {
                Console.WriteLine("\n\n\n proces \n\n\n");
                JObject messageJson = JObject.Parse(TotalBuffer);
                Console.WriteLine(messageJson.ToString());
                TotalBuffer = "";

                int serial = 0;
                try
                {
                    serial = (int)messageJson["data"]["data"]["serial"];
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                    serial = 0;
                }
                this.ServerResponses[serial] = messageJson;

            }
            Stream.BeginRead(Buffer, 0, Buffer.Length, new AsyncCallback(OnRead), null);
        }

        public static void WriteTextMessage(String message)
        {
            Debug.WriteLine("WriteTextMessage():  " + message);
            int messageLength = message.Length;
            byte[] dataLength = BitConverter.GetBytes(messageLength);
            byte[] messageData = Encoding.ASCII.GetBytes(message);
            byte[] data = dataLength.Concat(messageData).ToArray();

            Stream.Write(data);
            Stream.Flush();
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
                WriteTextMessage(GenerateMessage(Scene.Panel.DrawText(4, panelUUID, speed.ToString(), new float[] { 100, 100 }, 32, new int[] { 0, 0, 0, 1 }, "Calibri")));
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
