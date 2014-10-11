using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destiny.IO;

namespace Destiny.FileFormats
{
    public class Package
    {
        public Package(EndianIO io)
        {
            short BaseVersion = io.ReadInt16();
            PackagePlatform Platform = (PackagePlatform)io.ReadInt16();
            ushort EntryCount = io.ReadUInt16(); // max = 0xFFFF

#if DEBUG
            Verify(io);
#endif
        }

        /// <summary>
        /// Function verifies the hashes stored in the header of the pkg file. (Does not verify the RSA Signature at POSITION: 0x800)
        /// </summary>
        private void Verify(EndianIO io)
        {
            byte[] hash;

            //verify the resource entry table
            io.Position = 0xB4;
            uint entryCount = io.ReadUInt32();
            uint entryTableLocation = io.ReadUInt32();
            hash = io.ReadByteArray(0x14);

            io.Position = entryTableLocation;
            if (!Security.ArrayEquals(Security.SHA1(io.ReadByteArray(entryCount * 0x10)), hash))
                throw new Exception("Failed to verify the entry table hash in the pkg file.");

            //verify the data block table
            io.Position = 0xD0;
            uint blockCount = io.ReadUInt32();
            uint blockTableLocation = io.ReadUInt32();
            hash = io.ReadByteArray(0x14);

            io.Position = blockTableLocation;
            if (!Security.ArrayEquals(Security.SHA1(io.ReadByteArray(blockCount * 0x20)), hash))
                throw new Exception("Failed to verify the block table hash in the pkg file.");

            // verify the symbol table
            io.Position = 0xEC;
            uint symbolCount = io.ReadUInt32();
            uint symbolTableLocation = io.ReadUInt32();
            hash = io.ReadByteArray(0x14);

            io.Position = symbolTableLocation;
            if (!Security.ArrayEquals(Security.SHA1(io.ReadByteArray(symbolCount * 0x44)), hash))
                throw new Exception("Failed to verify the symbol table hash in the pkg file.");

        }
    }
}
