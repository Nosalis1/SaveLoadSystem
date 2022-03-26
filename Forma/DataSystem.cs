using UnityEngine;

using System;
using System.Reflection;

using Forma.Packets;

namespace Forma
{
    public static class DataSystem
    {
        public static string DataPath = DataManager.Instance.DataPath;  // Application.persistentDataPath;

        public static void Save(FilePacket Packet,string FileName)
        {
            CheckSerialization(ref Packet.Value);
            Saving.SaveInFile(Packet, FileName);
        }
        public static void Save(object Value,string FileName)
        {
            CheckSerialization(ref Value);
            FilePacket packet = new FilePacket((int)Value);
            Saving.SaveInFile(packet, FileName);
        }
        public static void Save(object[]Values,string FileName)
        {
            FilePacket[] packets = new FilePacket[Values.Length];
            for (int i = 0; i < Values.Length; i++)
            {
                CheckSerialization(ref Values[i]);
                packets[i] = new FilePacket(Values[i]);
            }
            Saving.SaveInFile(packets, FileName);
        }
        public static void SaveAll(object[] Values, string FileName)
        {
            object[] attributeValues = new object[0];
            MonoBehaviour[] sceneActive = GameObject.FindObjectsOfType<MonoBehaviour>();
            foreach (MonoBehaviour mono in sceneActive)
            {
                FieldInfo[] objectFields = mono.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);

                for (int i = 0; i < objectFields.Length; i++)
                {
                    SaveVar attribute = Attribute.GetCustomAttribute(objectFields[i], typeof(SaveVar)) as SaveVar;
                    if (attribute != null)
                    {
                        if (attribute.FileName == FileName)
                        {
                            Array.Resize(ref attributeValues, attributeValues.Length + 1);
                            object value = objectFields[i].GetValue(mono);
                            CheckSerialization(ref value);
                            MonoPacket packet = new MonoPacket(value, mono.GetInstanceID(), objectFields[i].Name);
                            attributeValues[attributeValues.Length - 1] = packet;
                        }
                    }
                }
            }
            FilePacket[] packets = new FilePacket[Values.Length + attributeValues.Length];

            int k = 0;
            for (int f = 0; f < Values.Length; f++)
            {
                packets[k] = new FilePacket(Values[f]);
                k++;
            }
            for (int s = 0; s < attributeValues.Length; s++)
            {
                packets[k] = new FilePacket(attributeValues[s], true);
                k++;
            }
            Saving.SaveInFile(packets, FileName);
        }

        public static object Load(string FileName)
        {
            FilePacket packet = Loading.LoadItemFromFile(FileName);
            UnckeckSerialization(ref packet.Value);
            return packet.Value;
        }
        public static object[] LoadAll(string FileName)
        {
            FilePacket[] packets = Loading.LoadItemsFromFile(FileName);
            object[] objects = new object[packets.Length];
            for (int i = 0; i < packets.Length; i++)
            {
                UnckeckSerialization(ref packets[i].Value);
                objects[i] = packets[i].Value;
            }
            return objects;
        }
        public static object[]LoadWithAttributes(string FileName)
        {
            FilePacket[] packets = Loading.LoadItemsFromFile(FileName);
            object[] objects = new object[0];

            MonoBehaviour[] sceneActive = GameObject.FindObjectsOfType<MonoBehaviour>();

            for (int i = 0; i < packets.Length; i++)
            {
                if (packets[i].IsAttribute)
                {
                    MonoPacket monoPacket = (MonoPacket)packets[i].Value;
                    object value = monoPacket.Value;
                    UnckeckSerialization(ref value);
                    MonoBehaviour instance = GetInstanceByID(sceneActive, monoPacket.instanceID);
                    FieldInfo[] objectFields = instance.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
                    for (int k = 0; k < objectFields.Length; k++)
                    {
                        if(objectFields[k].Name == monoPacket.instanceName)
                        {
                            objectFields[k].SetValue(instance, value);
                            break;
                        }
                    }
                }
                else
                {
                    Array.Resize(ref objects, objects.Length + 1);
                    UnckeckSerialization(ref packets[i].Value);
                    objects[objects.Length - 1] = packets[i].Value; 
                }
            }

            return objects;
        }

        static MonoBehaviour GetInstanceByID(MonoBehaviour[] instances, int ID)
        {
            for (int i = 0; i < instances.Length; i++)
            {
                if (instances[i].GetInstanceID() == ID)
                {
                    return instances[i];
                }
            }
            return null;
        }

        static void CheckSerialization(ref object Value)
        {
            System.Type type = Value.GetType();
            //Vector 3
            if (type == typeof(UnityEngine.Vector3) || type == typeof(UnityEngine.Vector3Int))
            {
                ConvertForFile(ref Value, type);
            }
            else if(type == typeof(UnityEngine.Vector3[])||type == typeof(UnityEngine.Vector3Int[]))
            {
                ConvertForFile(ref Value, type);
            }
            //Vector 2
            else if (type == typeof(UnityEngine.Vector2) || type == typeof(UnityEngine.Vector2Int))
            {
                ConvertForFile(ref Value, type);
            }
            else if (type == typeof(UnityEngine.Vector2[]) || type == typeof(UnityEngine.Vector2Int[]))
            {
                ConvertForFile(ref Value, type);
            }
            //Vector 4
            else if (type == typeof(UnityEngine.Vector4))
            {
                ConvertForFile(ref Value, type);
            }
            else if (type == typeof(UnityEngine.Vector4[]))
            {
                ConvertForFile(ref Value, type);
            }
            //Quaternion
            else if (type == typeof(UnityEngine.Quaternion))
            {
                ConvertForFile(ref Value, type);
            }
            else if (type == typeof(UnityEngine.Quaternion[]))
            {
                ConvertForFile(ref Value, type);
            }
            //Transform
            else if (type == typeof(UnityEngine.Transform))
            {
                ConvertForFile(ref Value, type);
            }
            else if (type == typeof(UnityEngine.Transform[]))
            {
                ConvertForFile(ref Value, type);
            }
        }

        static void UnckeckSerialization(ref object Value)
        {
            System.Type type = Value.GetType();
            //Vector 3
            if(type == typeof(Vector3Serializable))
            {
                ConvertForUnity(ref Value, type);
            }
            else if (type == typeof(Vector3Serializable[]))
            {
                ConvertForUnity(ref Value, type);
            }
            //Vector 2
            else if (type == typeof(Vector2Serializable))
            {
                ConvertForUnity(ref Value, type);
            }
            else if (type == typeof(Vector2Serializable[]))
            {
                ConvertForUnity(ref Value, type);
            }
            //Vector 4
            else if (type == typeof(Vector4Serializable))
            {
                ConvertForUnity(ref Value, type);
            }
            else if (type == typeof(Vector4Serializable[]))
            {
                ConvertForUnity(ref Value, type);
            }
            //Quaternion
            else if (type == typeof(QuaternionSerializable))
            {
                ConvertForUnity(ref Value, type);
            }
            else if (type == typeof(QuaternionSerializable[]))
            {
                ConvertForUnity(ref Value, type);
            }
            //Transform
            else if (type == typeof(TransformSerializable))
            {
                ConvertForUnity(ref Value, type);
            }
            else if (type == typeof(TransformSerializable[]))
            {
                ConvertForUnity(ref Value, type);
            }
        }

        static void ConvertForUnity(ref object Value, System.Type type)
        {
            //Vector 3
            if (type == typeof(Vector3Serializable))
            {
                Vector3Serializable vector = (Vector3Serializable)Value;
                Value = vector.Vector;
            }
            else if(type == typeof(Vector3Serializable[]))
            {
                Vector3Serializable[] vectors = (Vector3Serializable[])Value;
                Vector3[] unityVectors = new Vector3[vectors.Length];
                for (int i = 0; i < unityVectors.Length; i++)
                {
                    unityVectors[i] = vectors[i].Vector;
                }
                Value = unityVectors;
            }
            //Vector 2
            else if (type == typeof(Vector2Serializable))
            {
                Vector2Serializable vector = (Vector2Serializable)Value;
                Value = vector.Vector;
            }
            else if (type == typeof(Vector2Serializable[]))
            {
                Vector2Serializable[] vectors = (Vector2Serializable[])Value;
                Vector2[] unityVectors = new Vector2[vectors.Length];
                for (int i = 0; i < unityVectors.Length; i++)
                {
                    unityVectors[i] = vectors[i].Vector;
                }
                Value = unityVectors;
            }
            //Vector 4
            else if (type == typeof(Vector4Serializable))
            {
                Vector4Serializable vector = (Vector4Serializable)Value;
                Value = vector.Vector;
            }
            else if (type == typeof(Vector4Serializable[]))
            {
                Vector4Serializable[] vectors = (Vector4Serializable[])Value;
                Vector4[] unityVectors = new Vector4[vectors.Length];
                for (int i = 0; i < unityVectors.Length; i++)
                {
                    unityVectors[i] = vectors[i].Vector;
                }
                Value = unityVectors;
            }
            //Qaternion
            else if (type == typeof(QuaternionSerializable))
            {
                QuaternionSerializable vector = (QuaternionSerializable)Value;
                Value = vector.Vector;
            }
            else if (type == typeof(QuaternionSerializable[]))
            {
                QuaternionSerializable[] vectors = (QuaternionSerializable[])Value;
                Quaternion[] unityVectors = new Quaternion[vectors.Length];
                for (int i = 0; i < unityVectors.Length; i++)
                {
                    unityVectors[i] = vectors[i].Vector;
                }
                Value = unityVectors;
            }
            //Transform
            else if (type == typeof(TransformSerializable))
            {
                TransformSerializable vector = (TransformSerializable)Value;
                Value = vector.Value;
            }
            else if (type == typeof(TransformSerializable[]))
            {
                TransformSerializable[] vectors = (TransformSerializable[])Value;
                Transform[] unityVectors = new Transform[vectors.Length];
                for (int i = 0; i < unityVectors.Length; i++)
                {
                    unityVectors[i] = vectors[i].Value;
                }
                Value = unityVectors;
            }
        }

        static void ConvertForFile(ref object Value, System.Type type)
        {
            //Vector 3
            if (type == typeof(UnityEngine.Vector3) || type == typeof(UnityEngine.Vector3Int))
            {
                Value = new Vector3Serializable((Vector3)Value);
            }
            else if (type == typeof(UnityEngine.Vector3[]) || type == typeof(UnityEngine.Vector3Int[]))
            {
                Vector3[] currentArray = (Vector3[])Value;
                Vector3Serializable[] newArray = new Vector3Serializable[currentArray.Length];
                for (int i = 0; i < newArray.Length; i++)
                {
                    newArray[i] = new Vector3Serializable(currentArray[i]);
                }
                Value = newArray;
            }
            //Vector 2
            else if (type == typeof(UnityEngine.Vector2) || type == typeof(UnityEngine.Vector2Int))
            {
                Value = new Vector2Serializable((Vector2)Value);
            }
            else if (type == typeof(UnityEngine.Vector2[]) || type == typeof(UnityEngine.Vector2Int[]))
            {
                Vector2[] currentArray = (Vector2[])Value;
                Vector2Serializable[] newArray = new Vector2Serializable[currentArray.Length];
                for (int i = 0; i < newArray.Length; i++)
                {
                    newArray[i] = new Vector2Serializable(currentArray[i]);
                }
                Value = newArray;
            }
            //Vector 4
            else if (type == typeof(UnityEngine.Vector4))
            {
                Value = new Vector4Serializable((Vector4)Value);
            }
            else if (type == typeof(UnityEngine.Vector4[]))
            {
                Vector4[] currentArray = (Vector4[])Value;
                Vector4Serializable[] newArray = new Vector4Serializable[currentArray.Length];
                for (int i = 0; i < newArray.Length; i++)
                {
                    newArray[i] = new Vector4Serializable(currentArray[i]);
                }
                Value = newArray;
            }
            //Quaternion
            else if (type == typeof(UnityEngine.Quaternion))
            {
                Value = new QuaternionSerializable((Quaternion)Value);
            }
            else if (type == typeof(UnityEngine.Quaternion[]))
            {
                Quaternion[] currentArray = (Quaternion[])Value;
                QuaternionSerializable[] newArray = new QuaternionSerializable[currentArray.Length];
                for (int i = 0; i < newArray.Length; i++)
                {
                    newArray[i] = new QuaternionSerializable(currentArray[i]);
                }
                Value = newArray;
            }
            //Transform
            else if (type == typeof(UnityEngine.Transform))
            {
                Value = new TransformSerializable((Transform)Value);
            }
            else if (type == typeof(UnityEngine.Transform[]))
            {
                Transform[] currentArray = (Transform[])Value;
                TransformSerializable[] newArray = new TransformSerializable[currentArray.Length];
                for (int i = 0; i < newArray.Length; i++)
                {
                    newArray[i] = new TransformSerializable(currentArray[i]);
                }
                Value = newArray;
            }
        }
    }
}
