/*
See System.Convert.ToBase64String
and System.IO.FileStream and others for Base64.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace EncodingTestNamespace
{
    class Program
    {
        static void WriteToFile()
        {
            using(Stream s = File.Create("buf.txt"))
            using (TextWriter w = new StreamWriter(s, Encoding.UTF8))
            {
                w.WriteLine("Привет, Мир!");
            }
            // Or
            using (FileStream fs = new FileStream("test.txt", FileMode.Create)) 
            {
                StreamWriter writer = new StreamWriter(fs);
                writer.WriteLine("Helo");
                writer.Flush();
                fs.Position = 0;
                Console.WriteLine(fs.ReadByte());
            }
        }
        
        static void Main(string[] args)
        {
            Encoding cp1251 = Encoding.GetEncoding(1251);

            // Define a string.
            string str = "\u24c8 \u2075 \u221e";
            // Encode a Unicode string.
            Byte[] bytes = cp1251.GetBytes(str);
            // Decode the string.
            string str2 = cp1251.GetString(bytes);
            
            Encoding utf8 = new UTF8Encoding(true, true);
            
            Encoding cp1252r = Encoding.GetEncoding(1251, 
                                  new EncoderReplacementFallback("*"),
                                  new DecoderReplacementFallback("*"));

        }
    }
}