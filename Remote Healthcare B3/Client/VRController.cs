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
        private int messageLength = int.MaxValue;

        public VRController()
        {
            Client = new TcpClient();
            Client.BeginConnect("145.48.6.10", 6666, new AsyncCallback(OnConnect), null);
            this.ServerResponses = new List<JObject>(1000);
            for (int i = 0; i < 1000; i++)
            {
                this.ServerResponses.Add(null);
            }
            Console.WriteLine(ServerResponses.Count);
            while (!Connected){}
            Init();

            WriteTextMessage(GenerateMessage(Scene.Get(1)));
            JObject response = GetResponse(1);
            Console.WriteLine(response.ToString());
            SaveObjects(response, VRObjects.BASE);

            if (vrObject.getUUID("RightHand") != string.Empty)
            {
                string parentPanel = vrObject.getUUID("RightHand");
                WriteTextMessage(GenerateMessage(Scene.Node.Add(3, "SpeedPanel", parentPanel, new float[] { 0, 0.1f, -0.1f }, 0.25f, new int[] { -35, 0, 0 }, new int[] { 1, 1 }, new int[] { 512, 512 }, new int[] { 255, 255, 255, 1 }, false)));
            }
            response = GetResponse(3);
            SaveObjects(response, VRObjects.PANEL);

        }


        private void Init()
        {
            vrObject = new VRObject();
            WriteTextMessage("{\"id\":\"session/list\",\"serial\":0}");
            Console.WriteLine("\n\n\n send \n\n\n");
            JObject value = GetResponse(0);
            Console.WriteLine(value.ToString());
            // stap 2 (get response)
            var properSession = value["data"].Where(e => e["clientinfo"]["user"].ToObject<string>() == Environment.UserName).Last();
            var sessionId = properSession["id"];
            this.ServerResponses[0] = null;
            //Console.WriteLine(sessionId); // stap 3 (getting id)

            WriteTextMessage("{\"id\":\"tunnel/create\",\"data\":{\"session\":\"" + sessionId + "\"}}");

            Debug.WriteLine("Na tunnel create");
            JObject response = GetResponse(0);
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
            Stream.BeginRead(Buffer, 0, Buffer.Length, new AsyncCallback(OnReadTest), null);
            this.Connected = true;
        }

        private void OnRead(IAsyncResult ar)
        {
            Console.WriteLine("\n\n\n recieve \n\n\n");
            int receivedBytes = Stream.EndRead(ar);
            Console.WriteLine(Encoding.ASCII.GetString(Buffer, 0, receivedBytes));
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
                string message = TotalBuffer.Substring(0, this.messageLength);
                JObject messageJson = JObject.Parse(message);
                Console.WriteLine(messageJson.ToString());
                TotalBuffer = TotalBuffer.Substring(messageLength);
                

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

        public JObject GetResponse(int serial)
        {
            bool running = true;
            while (running)
            {
                if (this.ServerResponses[serial] != null)
                {
                    return this.ServerResponses[serial];
                }
            }
            return null;
        }
        private void OnReadTest(IAsyncResult ar)
        {
            Console.WriteLine("\n\n\n recieve \n");
            int receivedBytes = Stream.EndRead(ar);
            string receivedString = Encoding.ASCII.GetString(Buffer, 0, receivedBytes);
            Console.WriteLine(Encoding.ASCII.GetString(Buffer, 0, receivedBytes));

            if (Encoding.ASCII.GetString(new byte[] { Buffer[0] }) != "{" && this.messageLength == int.MaxValue)
            {
                byte[] lengthBytes;
                if (receivedString.Contains('{'))
                {
                    int indexBracket = receivedString.IndexOf('{');
                    lengthBytes = new byte[indexBracket];
                    for (int i = 0; i < indexBracket; i++)
                    {
                        lengthBytes[i] = Buffer[i];
                    }
                    TotalBuffer += receivedString.Substring(indexBracket);
                }
                else
                {
                    lengthBytes = Buffer;
                }
                lengthBytes.Reverse();
                int messageLengthInt = BitConverter.ToInt32(lengthBytes);
                this.messageLength = messageLengthInt;
                Console.WriteLine($"\n\n\n length: {messageLengthInt} \n\n\n");
                if (TotalBuffer == "")
                {
                    Stream.BeginRead(Buffer, 0, Buffer.Length, new AsyncCallback(OnReadTest), null);
                    return;
                }
            }
            else
            {
                TotalBuffer += receivedString;
            }

            if (TotalBuffer.Length >= this.messageLength)
            {
                Console.WriteLine("\n\n\n proces \n");
                string message = TotalBuffer.Substring(0, this.messageLength);
                JObject messageJson = JObject.Parse(message);
                Console.WriteLine(messageJson.ToString());
                TotalBuffer = TotalBuffer.Substring(messageLength);

                if (TotalBuffer.Length > 1)
                {
                    Console.WriteLine($"\n\n\n substring1 : {TotalBuffer}");
                    byte[] lengthBytes;
                    if (TotalBuffer.Contains('{'))
                    {
                        int indexBracket = TotalBuffer.IndexOf('{');
                        lengthBytes = new byte[indexBracket];
                        for (int i = 0; i < indexBracket; i++)
                        {
                            lengthBytes[i] = Buffer[i+messageLength];
                        }
                        Console.WriteLine("\n\n LengteBytes: ");
                        foreach (var item in lengthBytes)
                        {
                            Console.WriteLine(item);
                        }
                        TotalBuffer = TotalBuffer.Substring(indexBracket);
                        Console.WriteLine($"\n\n\n substring2 : {TotalBuffer}");
                    }
                    else
                    {
                        lengthBytes = Buffer;
                    }
                    lengthBytes.Reverse();
                    int messageLengthInt = BitConverter.ToInt32(lengthBytes);
                    this.messageLength = messageLengthInt;
                    Console.WriteLine($"\n\n\n length: {messageLengthInt} \n\n\n");
                }
                else 
                {
                    this.messageLength = int.MaxValue;
                }

                


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

            Stream.BeginRead(Buffer, 0, Buffer.Length, new AsyncCallback(OnReadTest), null);
        }
    }
}
