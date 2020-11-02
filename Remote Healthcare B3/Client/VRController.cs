using Client.VR_Libraries;
using Client_VR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Client.Properties;

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
            SaveObjects("", response, VRObjects.BASE);




            //exercise 3b: delete groundplane
            //while (vrObject.getUUID("GroundPlane") == string.Empty) { }
            string planeUUID = vrObject.getUUID("GroundPlane");
            JObject delNode = Scene.Node.Delete(5, planeUUID);
            WriteTextMessage(GenerateMessage(delNode));

            InitVR();
        }

        private void InitVR()
        {
            //Adding mountains
            JObject terain = Scene.Terrain.Add(90, Resources.Height_Map4);
            WriteTextMessage(GenerateMessage(terain));
            JObject grondRender = Scene.Node.Add(91, "Grond", new int[] { -128, 0, -128 }, 1, new float[] { 0, 0, 0 }, true);
            WriteTextMessage(GenerateMessage(grondRender));
            SaveObjects("", GetResponse(91), VRObjects.NODE);

            //Adding texture layer
            WriteTextMessage(GenerateMessage(Scene.Node.AddLayer(100, vrObject.getUUID("Grond"), @"data\NetworkEngine\textures\terrain\grass_autumn_red_d.jpg", @"data\NetworkEngine\textures\terrain\grass_diffuse.png", -100, 100, 1)));

            //add route
            Route.RouteNode[] routeNodes = new Route.RouteNode[4];
            routeNodes[0] = new Route.RouteNode(0, 0, 0, 5, 0, -5);
            routeNodes[1] = new Route.RouteNode(50, 0, 0, 5, 0, 5);
            routeNodes[2] = new Route.RouteNode(50, 0, 50, -5, 0, 5);
            routeNodes[3] = new Route.RouteNode(0, 0, 50, -5, 0, -5);
            JObject addRoute = Route.Add(92, routeNodes);
            WriteTextMessage(GenerateMessage(addRoute));
            SaveObjects("route", GetResponse(92), VRObjects.ROUTE);
            WriteTextMessage(GenerateMessage(Route.Show(999, false)));

            //adding road over the route
            JObject addRoad = Scene.Road.Add(93, vrObject.getUUID("route"));
            WriteTextMessage(GenerateMessage(addRoad));

            //adding a bicycle object
            JObject addBike = Scene.Node.Add(94, "bike", new int[] { 0, 0, 0 }, 0.01f, new float[] { 0, 0, 0 }, @"data\NetworkEngine\models\bike\bike_anim.fbx", false, true, "Armature|Fietsen");
            WriteTextMessage(GenerateMessage(addBike));
            SaveObjects("", GetResponse(94), VRObjects.NODE);

            WriteTextMessage(GenerateMessage(Scene.Node.Update(95, vrObject.getUUID("Camera"), vrObject.getUUID("bike"), new int[] { 0, 50, 0 }, 100, new int[] { 0, 90, 0 })));

            WriteTextMessage(GenerateMessage(Route.Follow(96, vrObject.getUUID("route"), vrObject.getUUID("bike"), 2, 0, Route.Rotation.XZ, 1, false, new float[] { 0, 0, 0 }, new int[] { 0, 0, 0 })));

            //bike panel
            WriteTextMessage(GenerateMessage(Scene.Node.Add(3, "BikePanel", vrObject.getUUID("bike"), new float[] { -35, 120, 0 }, 25f, new float[] { -50, 90, 0 }, new int[] { 1, 1 }, new int[] { 512, 512 }, new float[] { 0, 0, 0, 1 }, false)));
            SaveObjects("", GetResponse(3), VRObjects.PANEL);
            UpdateBikePanel(0, 0, 0);

            //chat panel
            WriteTextMessage(GenerateMessage(Scene.Node.Add(110, "ChatPanel", vrObject.getUUID("bike"), new float[] { -20, 120, -40 }, 25f, new float[] { -40, 45, 0 }, new int[] { 1, 1 }, new int[] { 512, 512 }, new float[] { 0, 0, 0, 1 }, false)));
            SaveObjects("", GetResponse(110), VRObjects.PANEL);
            UpdateChatPanel(new string[] { });
        }

        private void Init()
        {
            vrObject = new VRObject();
            WriteTextMessage("{\"id\":\"session/list\",\"serial\":0}");
            JObject value = GetResponse(0);
            // stap 2 (get response)

            try
            {
                var properSession = value["data"].Where(e => e["clientinfo"]["user"].ToObject<string>() == Environment.UserName).Last();
                var sessionId = properSession["id"];
                this.ServerResponses[0] = null;
                WriteTextMessage("{\"id\":\"tunnel/create\",\"data\":{\"session\":\"" + sessionId + "\"}}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            //Console.WriteLine(sessionId); // stap 3 (getting id)

            Debug.WriteLine("Na tunnel create");
            JObject response = GetResponse(0);
            // stap 4 (get response)
            var tunnelId = response["data"]["id"];
            TunnelId = (string)tunnelId;
            Debug.WriteLine("Init(): " + TunnelId);
            this.ServerResponses[0] = null;

            WriteTextMessage(GenerateMessage(Scene.Reset(2)));
            WriteTextMessage(GenerateMessage(Scene.Skybox.SetTime(999, 12f)));
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
                    //Console.WriteLine(messageJson.ToString());

                    byte[] tempBuffer = TotalBuffer.Skip(4 + packetLength).Take(TotalBuffer.Length - packetLength - 4).ToArray();
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
                    Console.WriteLine($"Added ServerResponse with serial: {serial}");
                    //Console.WriteLine($"Response \n: {messageJson}");
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
            //    Debug.WriteLine("WriteTextMessage():  " + message);
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

        public void UpdateBikePanel(float speed, float heartrate, float resistance)
        {
            string panelUUID = vrObject.getUUID("BikePanel");
            WriteTextMessage(GenerateMessage(Scene.Panel.Clear(11, panelUUID)));
            WriteTextMessage(GenerateMessage(Scene.Panel.DrawText(4, panelUUID, $"{speed:#0.00} m/s", new float[] { 100, 160 }, 70, new float[] { 1, 1, 1, 1 }, "Calibri")));
            WriteTextMessage(GenerateMessage(Scene.Panel.DrawText(4, panelUUID, $"{heartrate:#0.00} bpm", new float[] { 100, 260 }, 70, new float[] { 1, 1, 1, 1 }, "Calibri")));
            WriteTextMessage(GenerateMessage(Scene.Panel.DrawText(4, panelUUID, $"{resistance:#0.00} %", new float[] { 100, 360 }, 70, new float[] { 1, 1, 1, 1 }, "Calibri")));
            WriteTextMessage(GenerateMessage(Scene.Panel.Swap(11, panelUUID)));
            WriteTextMessage(GenerateMessage(Route.SetFollowSpeed(97, vrObject.getUUID("bike"), speed)));
        }

        public void UpdateChatPanel(string[] messages)
        {
            string panelUUID = vrObject.getUUID("ChatPanel");
            WriteTextMessage(GenerateMessage(Scene.Panel.Clear(111, panelUUID)));
            WriteTextMessage(GenerateMessage(Scene.Panel.DrawText(112, panelUUID, $"Chat", new float[] { 20, 70 }, 70, new float[] { 1, 1, 1, 1 }, "Calibri")));

            int offset = 0;
            for (int i = 0; i < messages.Length; i++)
            {
                string message = messages[messages.Length - 1 - i];
                if (offset >= 12)
                {
                    break;
                }

                if (message.Length > 30)
                {
                    var stringWrapped = WordWrap(message, 30);
                    if (stringWrapped.Count + offset > 12)
                    {
                        break;
                    }
                    stringWrapped.Reverse();
                    foreach (var item in stringWrapped)
                    {
                        WriteTextMessage(GenerateMessage(Scene.Panel.DrawText(112, panelUUID, item, new float[] { 20, 500 - (offset * 30) }, 30, new float[] { 1, 1, 1, 1 }, "Calibri")));
                        offset++;
                    }
                }
                else
                {
                    WriteTextMessage(GenerateMessage(Scene.Panel.DrawText(112, panelUUID, message, new float[] { 20, 500 - (offset * 30) }, 30, new float[] { 1, 1, 1, 1 }, "Calibri")));
                    offset++;
                }
            }
            WriteTextMessage(GenerateMessage(Scene.Panel.Swap(113, panelUUID)));
        }

        public static List<string> WordWrap(string text, int maxLineLength)
        {
            var list = new List<string>();

            int currentIndex;
            var lastWrap = 0;
            var whitespace = new[] { ' ', '\r', '\n', '\t' };
            do
            {
                currentIndex = lastWrap + maxLineLength > text.Length ? text.Length : (
                    text.LastIndexOfAny(new[] { ' ', ',', '.', '?', '!', ':', ';', '-', '\n', '\r', '\t' }, 
                    Math.Min(text.Length - 1, lastWrap + maxLineLength)) + 1);
                if (currentIndex <= lastWrap)
                    currentIndex = Math.Min(lastWrap + maxLineLength, text.Length);
                list.Add(text.Substring(lastWrap, currentIndex - lastWrap).Trim(whitespace));
                lastWrap = currentIndex;
            } while (currentIndex < text.Length);

            return list;
        }

        public void SaveObjects(string name, JObject json, VRObjects objectType)
        {
            string jsonString = json.ToString();
            bool nextIsName = false;
            bool nextIsUuid = false;

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
                if ( uuid != "")
                {
                    this.vrObject.AddVRObject(objectType, name, uuid);
                    Debug.WriteLine("Name: {0} \nuuid: {1}", name, uuid);

                    name = "empty";
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
