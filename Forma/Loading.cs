using UnityEngine;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Forma.Packets;

namespace Forma
{
    public static class Loading
    {
        public static FilePacket LoadItemFromFile(string FileName)
        {
            FilePacket packet = null;
            using (FileStream stream = File.Open(DataSystem.DataPath + $"/{FileName}.dat", FileMode.Open))
            {
                if (stream.Length != 0)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    packet = formatter.Deserialize(stream) as FilePacket;
                }
            }
            return packet;
        }

        public static FilePacket[] LoadItemsFromFile(string FileName)
        {
            FilePacket[] packets = null;

            using (FileStream stream = File.Open(DataSystem.DataPath + $"/{FileName}.dat", FileMode.Open))
            {
                if (stream.Length != 0)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    packets = formatter.Deserialize(stream) as FilePacket[];
                }
            }

            return packets;
        }
    }
}
