using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyPKGTool
{
    public static class Cra0Utilz
    {
        static public void CreatePath(string pathName)
        {
            if (!(Directory.Exists(pathName)))
            {
                Directory.CreateDirectory(pathName);
            }
        }

    }
}
