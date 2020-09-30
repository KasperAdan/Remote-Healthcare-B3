using Client.VR_Libraries;
using Client_VR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class VRController
    {
        private static TcpClient Client;
        private static NetworkStream Stream;
        private static string TunnelId;
        private static byte[] Buffer = new byte[1024];
        private static byte[] TotalBuffer = new byte[0];
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
            while (!Connected) { }
            Init();

            WriteTextMessage(GenerateMessage(Scene.Get(1)));
            JObject response = GetResponse(1);
            SaveObjects(response, VRObjects.BASE);

            //exercise 3b: delete groundplane
            //while (vrObject.getUUID("GroundPlane") == string.Empty) { }
            string planeUUID = vrObject.getUUID("GroundPlane");
            JObject delNode = Scene.Node.Delete(5, planeUUID);
            WriteTextMessage(GenerateMessage(delNode));

            //while (vrObject.getUUID("RightHand") == string.Empty) { }
            string parentPanel = vrObject.getUUID("RightHand");
            WriteTextMessage(GenerateMessage(Scene.Node.Add(3, "SpeedPanel", parentPanel, new float[] { 0, 0.1f, -0.1f }, 0.25f, new int[] { -35, 0, 0 }, new int[] { 1, 1 }, new int[] { 512, 512 }, new float[] { 1, 0, 0, 1 }, true)));

            response = GetResponse(3);
            SaveObjects(response, VRObjects.PANEL);

            SetSpeed(50);

        }

        private void Init()
        {
            vrObject = new VRObject();
            WriteTextMessage("{\"id\":\"session/list\",\"serial\":0}");
            JObject value = GetResponse(0);
            // stap 2 (get response)
            var properSession = value["data"].Where(e => e["clientinfo"]["user"].ToObject<string>() == Environment.UserName).Last();
            var sessionId = properSession["id"];
            this.ServerResponses[0] = null;
            //Console.WriteLine(sessionId); // stap 3 (getting id)

            WriteTextMessage("{\"id\":\"tunnel/create\",\"data\":{\"session\":\"" + sessionId + "\"}}");

            Debug.WriteLine("Na tunnel create");
            JObject response = GetResponse(0);
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

        private void OnRead(IAsyncResult ar)
        {
            int receivedBytes = Stream.EndRead(ar);
            TotalBuffer = AddByteArrays(TotalBuffer, Buffer, receivedBytes);

            while (TotalBuffer.Length >= 4)
            {
                int packetLength = BitConverter.ToInt32(TotalBuffer, 0);
                if (TotalBuffer.Length >= packetLength + 4)
                {
                    string message = Encoding.ASCII.GetString(TotalBuffer, 4, packetLength);
                    JObject messageJson = JObject.Parse(message);
                    Console.WriteLine(messageJson.ToString());

                    byte[] tempBuffer = TotalBuffer.Skip(3 + packetLength).Take(receivedBytes - packetLength - 4).ToArray();
                    TotalBuffer = tempBuffer;

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
                } else
                {
                    break;
                }
            }

            Stream.BeginRead(Buffer, 0, Buffer.Length, new AsyncCallback(OnRead), null);
        }

        private static byte[] AddByteArrays(byte[] b1, byte[] b2, int count)
        {
            byte[] newByteArray = new byte[b1.Length + count];
            System.Buffer.BlockCopy(b1, 0, newByteArray, 0, b1.Length);
            System.Buffer.BlockCopy(b2, 0, newByteArray, b1.Length, count);
            return newByteArray;
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
            string panelUUID = vrObject.getUUID("SpeedPanel");
            WriteTextMessage(GenerateMessage(Scene.Panel.Swap(11, panelUUID)));
            WriteTextMessage(GenerateMessage(Scene.Panel.SetClearColor(6, panelUUID, new float[] { 1, 1, 1, 1 })));
            WriteTextMessage(GenerateMessage(Scene.Panel.DrawText(4, panelUUID, speed + "m/s", new float[] { 10, 100 }, 100, new float[] { 0, 0, 0, 1 }, "Calibri")));
            WriteTextMessage(GenerateMessage(Scene.Panel.Swap(11, panelUUID)));
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
    }
}
