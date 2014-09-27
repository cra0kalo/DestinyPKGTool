using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//typedef c++ <3
using magic8 = System.UInt64;
using asciiz = System.String;

using int8 = System.Byte;
using int16 = System.Int16;
using int32 = System.Int32;
using int64 = System.Int64;

using uint8 = System.Byte;
using uint16 = System.UInt16;
using uint32 = System.UInt32;
using uint64 = System.UInt64;




namespace DestinyPKGTool
{
class PKG
{

    struct PK_Header
    {
        public byte[] magic;  //00 18 00 03 for xbox, 00 18 00 04 for ps3
        public uint16 nElementCount;
        public uint16 version;
        public uint32 unknownA;
        public uint32 unknownB;
        public uint32 blankA;
        public uint32 flagA;
        public uint32 constValA; //doesnt change?
        public uint32 constValB; //count maybe const
        public byte[] stampText; //size 132
    };



    struct PK_RSAsignatureDesc
    {
        public uint32 unknownA;
        public uint32 unknownB;
        public uint32 unknownC;
        public uint32 dataPointer; //points to data
        public uint32 unkParA;
        public uint32 unkParB;
        public byte[] chunk; //size 20
    };


    //Points the ArcEntries?
    struct PK_FATEntryDesc
    {
        public uint32 entryBlockCount;
        public uint32 entryBlockLocation; //points to entry start
        public byte[] chunk; //size 20
    };

    struct PK_Entry
    {
        public uint32 FileOffset;
        public uint32 CompressedFileSize;
        public byte[] flag; //256-> encrypted, 3rd byte tells the game whether the data is compressed
        public byte[] Sha1FileHash; //size 20
    };


    struct PK_UnknownTag
    {
        public uint32 flagA;
        public uint32 flagB;
        public byte[] text; //60
    };

    public enum PlatformType: byte
    {
        XBOX = 0x3,
        PS3 = 0x4
    }

    private bool ignoreCompressed;
    private string In_FilePath;
    private string Out_FolderPath;

    //Reader
    private BinaryReader br;
    private FileStream fs;

    //Writer
    private BinaryWriter bw;
    private FileStream fw;


    //CORE DATA ITEMS
    PK_Header header;
    PlatformType consoleType;
    PK_RSAsignatureDesc pkgSigInfo; //RSA Signature info stuff
    PK_FATEntryDesc entryDescriptor; //Chunks or entries
    PK_FATEntryDesc entryText; //??
    private List<PK_Entry> entries = new List<PK_Entry>();



    public PKG(string InFilePath, string OutFolderPath,bool ignoreCompressed)
    {
        this.In_FilePath = InFilePath;
        this.Out_FolderPath = OutFolderPath;
        this.ignoreCompressed = ignoreCompressed;
    }


    public void ParseExport()
    {
        //TODO: IMPLEMENT


    }


    public void DebugExport()
    {

        //fire up the stream and reader
        Program.VText("Initalizing PKG Export..");
        fs = new FileStream(In_FilePath, FileMode.Open, FileAccess.Read);
        br = new BinaryReader(fs);
        IO.ByteOrder byteSex = IO.ByteOrder.BigEndian;


        header.magic = IO.ReadBytes(br,4,byteSex);
        if (this.checkFOURCC(header.magic) != true)
        {
            Program.PError("ERROR: INCORRECT MAGIC BYTES - looking for 00 18 00 XX");
            return;
        }
        Program.VText("Detected Platform: " + this.consoleType.ToString());


        header.nElementCount = IO.ReadUInt16(br, byteSex);
        header.version = IO.ReadUInt16(br, byteSex);
        header.unknownA = IO.ReadUInt32(br, byteSex);
        header.unknownB = IO.ReadUInt32(br, byteSex);
        header.blankA = IO.ReadUInt32(br, byteSex);
        header.flagA = IO.ReadUInt32(br, byteSex);
        header.constValA = IO.ReadUInt32(br, byteSex);
        header.constValB = IO.ReadUInt32(br, byteSex);
        header.stampText = IO.ReadBytes(br, 132, byteSex);


        pkgSigInfo.unknownA = IO.ReadUInt32(br, byteSex);
        pkgSigInfo.unknownB = IO.ReadUInt32(br, byteSex);
        pkgSigInfo.unknownC = IO.ReadUInt32(br, byteSex);
        pkgSigInfo.dataPointer = IO.ReadUInt32(br, byteSex);
        pkgSigInfo.unkParA = IO.ReadUInt32(br, byteSex);
        pkgSigInfo.unkParB = IO.ReadUInt32(br, byteSex);
        pkgSigInfo.chunk = IO.ReadBytes(br, 20, byteSex);


        entryDescriptor.entryBlockCount = IO.ReadUInt32(br, byteSex);
        entryDescriptor.entryBlockLocation = IO.ReadUInt32(br, byteSex);
        entryDescriptor.chunk = IO.ReadBytes(br, 20, byteSex);


        //seek to the start of the entries
        fs.Seek(entryDescriptor.entryBlockLocation, SeekOrigin.Begin);

        //Read each entry and process
        Program.VText("Reading Package entries..");
        for (uint i = 0; i < entryDescriptor.entryBlockCount; i++)
        {
            PK_Entry curEntry;
            curEntry.FileOffset = IO.ReadUInt32(br, byteSex);
            curEntry.CompressedFileSize = IO.ReadUInt32(br, byteSex);
            curEntry.flag = IO.ReadBytes(br, 4, byteSex);
            curEntry.Sha1FileHash = IO.ReadBytes(br, 20, byteSex);

            //append
            entries.Add(curEntry);
        }

        //now export those entries
        Program.VText("Exporting Package entries..");
        foreach (PK_Entry pkFile in this.entries)
        {
            fs.Seek(pkFile.FileOffset, SeekOrigin.Begin);

            if (pkFile.flag[1] == 0x1)
            {
                if (this.ignoreCompressed != true)
                    Program.VText(String.Format("Entry: " + "0x{0}", pkFile.FileOffset.ToString("x")) + " compressed");
                else
                    Program.VText(String.Format("Entry: " + "0x{0}", pkFile.FileOffset.ToString("x")) + " is compressed will ignore");
            }
            else
            {
                Program.VText(String.Format("Entry: " + "0x{0}", pkFile.FileOffset.ToString("x")) + " uncompressed");
            }


            if (this.ignoreCompressed != true)
            {
                string outFilePath = Path.Combine(Out_FolderPath,String.Format("0x{0}",pkFile.FileOffset.ToString("x")) + ".dat");
                using (FileStream fw = new FileStream(outFilePath, FileMode.Create, FileAccess.Write))
                {
                    bw = new BinaryWriter(fw);
                    bw.Write(br.ReadBytes((int)pkFile.CompressedFileSize));
                }
                Program.VText("-->Saved: " + Path.GetFileName(outFilePath));
                Program.VText("");
            }



        }


        //close reader and underlying stream
        br.Close();
        Program.VText("");
        Program.VText("Done!");

    }



    private bool checkFOURCC(byte[] code)
    {

        if (code[3] != 0x0)
            return false;
        if (code[2] != 0x18)
            return false;
        if (code[1] != 0x0)
            return false;

        if (code[0] == (byte)PlatformType.XBOX)
            this.consoleType = PlatformType.XBOX;
        else if (code[0] == (byte)PlatformType.PS3)
            this.consoleType = PlatformType.PS3;
        else
            return false;

        return true;
    }




}
}
