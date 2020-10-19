using Client.VR_Libraries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client_VR
{
    class VRObject
    {

        //public static String ObjectName { get; set; }

        //public static String ObjectType { get; set; }
        //public static string ObjectUUID { get; set; }

        public static Dictionary<string, string> TerrainDictionary = new Dictionary<string, string>();
        public static Dictionary<string, string> PanelDictionary = new Dictionary<string, string>();
        public static Dictionary<string, string> RouteDictionary = new Dictionary<string, string>();
        public static Dictionary<string, string> RoadDictionary = new Dictionary<string, string>();
        public static Dictionary<string, string> NodeDictionary = new Dictionary<string, string>();
        public static Dictionary<string, string> BaseDictionary = new Dictionary<string, string>();

        public static List<Dictionary<String, string>> Dictionaries = new List<Dictionary<string, string>>() { TerrainDictionary, PanelDictionary, RouteDictionary, RoadDictionary, NodeDictionary, BaseDictionary };

        public VRObject()
        {

        }

        public void AddVRObject(VRObjects objectType, string objectName, string objectUUID)
        {
            switch (objectType.ToString())
            {
                case "BASE":
                    BaseDictionary.Add(objectName, objectUUID);
                    break;
                case "TERRAIN":
                    TerrainDictionary.Add(objectName, objectUUID);
                    break;
                case "PANEL":
                    PanelDictionary.Add(objectName, objectUUID);
                    break;
                case "ROUTE":
                    RouteDictionary.Add(objectName, objectUUID);
                    break;
                case "ROAD":
                    RoadDictionary.Add(objectName, objectUUID);
                    break;
                case "NODE":
                    NodeDictionary.Add(objectName, objectUUID);
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
        }


        public static int TotalDictionaryEntries()
        {

            return VRObject.TerrainDictionary.Count + VRObject.PanelDictionary.Count + VRObject.RouteDictionary.Count + VRObject.RoadDictionary.Count + VRObject.NodeDictionary.Count;
        }

        public static ArrayList LookupEntries(Enum ObjectName)
        {
            ArrayList ReturnList = new ArrayList();

            if (ObjectName.ToString().Equals(VRObjects.NODE.ToString()))
            {
                foreach (KeyValuePair<string, string> vrobject in VRObject.NodeDictionary)
                {
                    Console.WriteLine("Key: {0}, Value: {1}",
                    vrobject.Key, vrobject.Value);
                }
            }

            if (ObjectName.ToString().Equals(VRObjects.ROAD.ToString()))
            {
                foreach (KeyValuePair<string, string> vrobject in VRObject.RoadDictionary)
                {
                    Console.WriteLine("Key: {0}, Value: {1}",
                    vrobject.Key, vrobject.Value);
                }
            }

            if (ObjectName.ToString().Equals(VRObjects.ROUTE.ToString()))
            {
                foreach (KeyValuePair<string, string> vrobject in VRObject.RouteDictionary)
                {
                    Console.WriteLine("Key: {0}, Value: {1}",
                    vrobject.Key, vrobject.Value);
                }
            }

            if (ObjectName.ToString().Equals(VRObjects.PANEL.ToString()))
            {
                foreach (KeyValuePair<string, string> vrobject in VRObject.PanelDictionary)
                {
                    Console.WriteLine("Key: {0}, Value: {1}",
                    vrobject.Key, vrobject.Value);
                }
            }

            if (ObjectName.ToString().Equals(VRObjects.TERRAIN.ToString()))
            {
                foreach (KeyValuePair<string, string> vrobject in VRObject.TerrainDictionary)
                {
                    Console.WriteLine("Key: {0}, Value: {1}",
                    vrobject.Key, vrobject.Value);
                }
            }


            return ReturnList;

        }

        public string getUUID(string name)
        {
            bool found = false;
            while (!found)
            {
                foreach (var item in Dictionaries)
                {
                    if (item.TryGetValue(name, out string value))
                    {
                        return value;
                    }
                }
            }
            return "";
        }

    }




}
