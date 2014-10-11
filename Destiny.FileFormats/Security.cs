using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destiny.FileFormats
{
    public static class Security
    {
        public static byte[] SHA1(byte[] Input)
        {
            return System.Security.Cryptography.SHA1.Create().ComputeHash(Input);
        }
        public static bool ArrayEquals<T>(T[] a, T[] b)
        {
            if (a.Length != b.Length)
                return false;

            return !a.Where((t, i) => !t.Equals(b[i])).Any();
        }
    }
}
