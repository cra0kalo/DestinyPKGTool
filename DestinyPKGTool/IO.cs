using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyPKGTool
{


public static class IO
{

    public enum ByteOrder : int
    {
        LittleEndian,
        BigEndian
    }



    public static byte[] ReadBytes(BinaryReader reader, int fieldSize, ByteOrder byteOrder)
    {
        byte[] bytes = new byte[fieldSize];
        if (byteOrder == ByteOrder.LittleEndian)
        {
            return reader.ReadBytes(fieldSize);
        }
        else
        {
            for (int i = fieldSize - 1; i >= 0; i--)
            {
                bytes[i] = reader.ReadByte();
            }
            return bytes;
        }
    }



    public static byte[] ReadByteArray_BIG(BinaryReader reader, ulong dataAsize)
    {
        return ReadBytes(reader, Convert.ToInt32(dataAsize), ByteOrder.BigEndian);
    }


    public static long ReadLong64(BinaryReader reader, ByteOrder byteOrder)
    {
        if (byteOrder == ByteOrder.LittleEndian)
        {
            return reader.ReadInt64();
        }
        else // Big-Endian
        {
            return BitConverter.ToInt64(ReadBytes(reader, 8, ByteOrder.BigEndian), 0);
        }
    }

    public static ulong ReadULong64(BinaryReader reader, ByteOrder byteOrder)
    {
        if (byteOrder == ByteOrder.LittleEndian)
        {
            return reader.ReadUInt64();
        }
        else // Big-Endian
        {
            return BitConverter.ToUInt64(ReadBytes(reader, 8, ByteOrder.BigEndian), 0);
        }
    }


    public static int ReadInt32(BinaryReader reader, ByteOrder byteOrder)
    {
        if (byteOrder == ByteOrder.LittleEndian)
        {
            return reader.ReadInt32();
        }
        else // Big-Endian
        {
            return BitConverter.ToInt32(ReadBytes(reader, 4, ByteOrder.BigEndian), 0);
        }
    }

    public static uint ReadUInt32(BinaryReader reader, ByteOrder byteOrder)
    {
        if (byteOrder == ByteOrder.LittleEndian)
        {
            return reader.ReadUInt32();
        }
        else // Big-Endian
        {
            return BitConverter.ToUInt32(ReadBytes(reader, 4, ByteOrder.BigEndian), 0);
        }
    }

    public static ulong ReadUInt64(BinaryReader reader, ByteOrder byteOrder)
    {
        if (byteOrder == ByteOrder.LittleEndian)
        {
            return reader.ReadUInt64();
        }
        else // Big-Endian
        {
            return BitConverter.ToUInt64(ReadBytes(reader, 8, ByteOrder.BigEndian), 0);
        }
    }



    public static short ReadInt16(BinaryReader reader, ByteOrder byteOrder)
    {
        if (byteOrder == ByteOrder.LittleEndian)
        {
            return reader.ReadInt16();
        }
        else // Big-Endian
        {
            return BitConverter.ToInt16(ReadBytes(reader, 2, ByteOrder.BigEndian), 0);
        }
    }

    public static ushort ReadUInt16(BinaryReader reader, ByteOrder byteOrder)
    {
        if (byteOrder == ByteOrder.LittleEndian)
        {
            return reader.ReadUInt16();
        }
        else // Big-Endian
        {
            return BitConverter.ToUInt16(ReadBytes(reader, 2, ByteOrder.BigEndian), 0);
        }
    }

    public static byte ReadUInt8(BinaryReader reader, ByteOrder byteOrder)
    {
        if (byteOrder == ByteOrder.LittleEndian)
        {
            return reader.ReadByte();
        }
        else // Big-Endian
        {
            return reader.ReadByte();
        }
    }


    //Float

    public static float ReadFloat(BinaryReader reader, ByteOrder byteOrder)
    {
        if (byteOrder == ByteOrder.LittleEndian)
        {
            return reader.ReadSingle();
        }
        else // Big-Endian
        {
            return BitConverter.ToSingle(ReadBytes(reader, 4, ByteOrder.BigEndian), 0);
        }
    }







}
}
