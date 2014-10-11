using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destiny.IO;

namespace Destiny.FileFormats
{
    /// <summary>
    /// Offset for the location of the Resource Entry Table is located at 0xB8 in the *.pkg files
    /// </summary>
    public class ResourceEntry
    {
        public uint Unknown1;
        public uint Unknown2;

        public ulong ResourceInformation;

        public ResourceEntry(EndianIO io)
        {
            Unknown1 = io.ReadUInt32();
            Unknown2 = io.ReadUInt32();

            ResourceInformation = io.ReadUInt64();
        }

        /// <summary>
        /// Calculates the position of the resource in the fully decompressed file
        /// </summary>
        /// <returns>Position of resource in file.</returns>
        public uint GetRealOffset()
        {
            uint blockIndex = (uint)((ResourceInformation & 0x3FF) * 0x40000);
            return (uint)((ResourceInformation >> 0xE) << 0x4) & 0x3FFF0;
        }
        /// <summary>
        /// Gets the length of the resource data.
        /// </summary>
        /// <returns>Length of resource entry item.</returns>
        public uint GetLength()
        {
            return (uint)((ResourceInformation >> 0x1C) & 0x3FFFFFFF);
        }
    }
}
