using System;
using System.IO;
using System.Text;


namespace Huffman
{
    public class Manejar
    {

        static Encoding enc = Encoding.GetEncoding("us-ascii",new EncoderExceptionFallback(),   new DecoderExceptionFallback());

        public static void EscribirArchivoBinario(byte[] bytes, string fileName)
        {
            using (var crearArch = File.Create(@fileName))
            {
                using (var escribirbin = new BinaryWriter(crearArch, enc))
                {
                    escribirbin.Write(bytes);

                    escribirbin.Close();
                }

                crearArch.Close();
            }
        }

        public static byte[] GetArchivoBytes(string fileName)
        {
            byte[] bytes;

            using (var crearArch = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var leerbin = new BinaryReader(crearArch, enc))
                {
                    bytes = leerbin.ReadBytes(Convert.ToInt32(crearArch.Length));

                    leerbin.Close();
                }

                crearArch.Close();
            }

            return bytes;
        }

    }
}
