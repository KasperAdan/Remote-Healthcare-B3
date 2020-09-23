﻿using Client.VR_Libraries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Client_VR
{
    class VRObject {

        public static String ObjectName { get; set; }

        public static String ObjectType { get; set; }
        public static int ObjectUUID { get; set; }

        public static Dictionary<String, int> TerrainDictionary = new Dictionary<String, int>();
        public static Dictionary<String, int> PanelDictionary = new Dictionary<String, int>();
        public static Dictionary<String, int> RouteDictionary = new Dictionary<String, int>();
        public static Dictionary<String, int> RoadDictionary = new Dictionary<String, int>();
        public static Dictionary<String, int> NodeDictionary = new Dictionary<String, int>();
        public static Dictionary<String, int> BaseDictionary = new Dictionary<String, int>();

        public VRObject(VRObjects objecttype, string objectName, int objectUUID)
        {
            ObjectName = objectName;
            ObjectUUID = objectUUID;
            ObjectType = objecttype.ToString();

            switch (ObjectType)
            {
                case "BASE":
                    BaseDictionary.Add(ObjectName, ObjectUUID);
                    break;
                case "TERRAIN":
                    TerrainDictionary.Add(ObjectName, ObjectUUID);
                    break;
                case "PANEL":
                    PanelDictionary.Add(ObjectName, ObjectUUID);
                    break;
                case "ROUTE":
                    RouteDictionary.Add(ObjectName, ObjectUUID);
                    break;
                case "ROAD":
                    RoadDictionary.Add(ObjectName, ObjectUUID);
                    break;
                case "NODE":
                    NodeDictionary.Add(ObjectName, ObjectUUID);
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
                foreach (KeyValuePair<String, int> vrobject in VRObject.NodeDictionary)
                {
                    Console.WriteLine("Key: {0}, Value: {1}",
                    vrobject.Key, vrobject.Value);
                }
            }

            if (ObjectName.ToString().Equals(VRObjects.ROAD.ToString()))
            {
                foreach (KeyValuePair<String, int> vrobject in VRObject.RoadDictionary)
                {
                    Console.WriteLine("Key: {0}, Value: {1}",
                    vrobject.Key, vrobject.Value);
                }
            }

            if (ObjectName.ToString().Equals(VRObjects.ROUTE.ToString()))
            {
                foreach (KeyValuePair<String, int> vrobject in VRObject.RouteDictionary)
                {
                    Console.WriteLine("Key: {0}, Value: {1}",
                    vrobject.Key, vrobject.Value);
                }
            }

            if (ObjectName.ToString().Equals(VRObjects.PANEL.ToString()))
            {
                foreach (KeyValuePair<String, int> vrobject in VRObject.PanelDictionary)
                {
                    Console.WriteLine("Key: {0}, Value: {1}",
                    vrobject.Key, vrobject.Value);
                }
            }

            if (ObjectName.ToString().Equals(VRObjects.TERRAIN.ToString()))
            {
                foreach (KeyValuePair<String, int> vrobject in VRObject.TerrainDictionary)
                {
                    Console.WriteLine("Key: {0}, Value: {1}",
                    vrobject.Key, vrobject.Value);
                }
            }


            return ReturnList;

        }

    }

    


}
