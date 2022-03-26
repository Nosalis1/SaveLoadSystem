using UnityEngine;

namespace Forma.Packets
{
    [System.Serializable]
    public class FilePacket
    {
        public object Value = 0;
        public bool IsAttribute = false;

        public FilePacket(object Value, bool IsAttribute = false)
        {
            this.Value = Value;
            this.IsAttribute = IsAttribute;
        }
        public FilePacket(bool IsAttribute = false)
        {
            this.IsAttribute = IsAttribute;
        }
    }

    [System.Serializable]
    public class MonoPacket
    {
        public int instanceID;
        public string instanceName;
        public object Value;

        public MonoPacket(object Value,int instanceID,string instanceName)
        {
            this.instanceID = instanceID;
            this.Value = Value;
            this.instanceName = instanceName;
        }
    }

    [System.Serializable]
    public class Vector3Serializable
    {
        float X, Y, Z;

        public static Vector3Serializable Zero = new Vector3Serializable(Vector3.zero);
        public static Vector3Serializable One = new Vector3Serializable(Vector3.one);

        public Vector3 Vector => new Vector3(X, Y, Z);
        public Vector3Int IntVector => new Vector3Int((int)X, (int)Y, (int)Z);

        public Vector3Serializable(Vector3Int Value)
        {
            X = Value.x;
            Y = Value.y;
            Z = Value.z;
        }
        public Vector3Serializable(Vector3 Value)
        {
            X = Value.x;
            Y = Value.y;
            Z = Value.z;
        }
        public Vector3Serializable()
        {

        }
    }

    [System.Serializable]
    public class Vector2Serializable
    {
        float X, Y;

        public static Vector2Serializable Zero = new Vector2Serializable(Vector2.zero);
        public static Vector2Serializable One = new Vector2Serializable(Vector2.one);

        public Vector2 Vector => new Vector2(X, Y);
        public Vector2Int IntVector => new Vector2Int((int)X, (int)Y);

        public Vector2Serializable(Vector2Int Value)
        {
            X = Value.x;
            Y = Value.y;
        }
        public Vector2Serializable(Vector2 Value)
        {
            X = Value.x;
            Y = Value.y;
        }
        public Vector2Serializable()
        {
        }
    }

    [System.Serializable]
    public class Vector4Serializable
    {
        float X, Y, Z, W;

        public static Vector4Serializable Zero = new Vector4Serializable(Vector4.zero);
        public static Vector4Serializable One = new Vector4Serializable(Vector4.one);

        public Vector4 Vector => new Vector4(X, Y, Z, W);

        public Vector4Serializable(Vector4 Value)
        {
            X = Value.x;
            Y = Value.y;
            Z = Value.z;
            W = Value.w;
        }
        public Vector4Serializable()
        {
        }
    }

    [System.Serializable]
    public class QuaternionSerializable
    {
        float X, Y, Z, W;

        public static QuaternionSerializable Zero = new QuaternionSerializable(Quaternion.identity);

        public Quaternion Vector => new Quaternion(X, Y, Z, W);

        public QuaternionSerializable(Quaternion Value)
        {
            X = Value.x;
            Y = Value.y;
            Z = Value.z;
            W = Value.w;
        }
        public QuaternionSerializable()
        {
        }
    }

    [System.Serializable]
    public class TransformSerializable
    {
        int instanceID = 0;

        Vector3Serializable position;
        Vector3Serializable scale;
        QuaternionSerializable rotation;

        public Transform Value
        {
            get
            {
                Transform transform = GetBehaviourByInstance(instanceID);
                if(transform != null)
                {
                    transform.position = position.Vector;
                    transform.rotation = rotation.Vector;
                    transform.localScale = scale.Vector;
                }
                return transform;
            }
        }

        public TransformSerializable(Transform Value)
        {
            instanceID = Value.GetInstanceID();
            position = new Vector3Serializable(Value.position);
            scale = new Vector3Serializable(Value.localScale);
            rotation = new QuaternionSerializable(Value.rotation);
        }
        public TransformSerializable()
        {
            instanceID = -1;
            position = Vector3Serializable.Zero;
            scale = Vector3Serializable.One;
            rotation = QuaternionSerializable.Zero;
        }

        static Transform GetBehaviourByInstance(int ID)
        {
            Transform[] all = GameObject.FindObjectsOfType<Transform>();
            for (int i = 0; i < all.Length; i++)
            {
                if (all[i].GetInstanceID() == ID)
                {
                    return all[i];
                }
            }
            return null;
        }
    }
}
