using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
namespace ResourceCollector
{
    public static class OtherFunctions
    {
        public static void WritePackString(this System.IO.BinaryWriter self, string @string)//стринг с нулём, но длина без нуля
        {
            self.Write(@string.Length - 1);
            self.Write(@string.ToCharArray());
        }
        public static string ReadPackString(this System.IO.BinaryReader self)//стринг с нулём
        {
            var length = self.ReadInt32();
            return new string(self.ReadChars(length + 1));
        }
        public static int GetPackStringLengthForWrite(string @string)//стринг с нулём, но длина без нуля
        {
            return @string.Length - 1;
        }
        
    }
}
