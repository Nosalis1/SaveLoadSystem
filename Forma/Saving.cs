using UnityEngine;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Forma.Packets;

namespace Forma
{
    public static class Saving
    {
        public static void SaveInFile(FilePacket Packet, string FileName)
        {
            using (FileStream stream = File.Create(DataSystem.DataPath + $"/{FileName}.dat"))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, Packet);
            }
        }

        public static void SaveInFile(FilePacket[]Packets,string FileName)
        {
            using(FileStream stream = File.Create(DataSystem.DataPath + $"/{FileName}.dat"))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, Packets);
            }
        }
    }
}
