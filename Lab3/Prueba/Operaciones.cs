using System.IO;
using System;


namespace Huffman
{
    public class Operaciones
    {
        
        public string path="";
        public string root = "";
      
        public Operaciones(string path, string root)
        {
            this.path = path;
            this.root = root;

        }

        public void Comprimir()
        {
            string root2 = root;
            root = root + @"\\Escritoriovpn.hdef";
            HuffmanEncodeFile hef = new HuffmanEncodeFile(@path, @root);
            byte[] b = hef.Encode();
            root = @"..\\..\\..\\Comprimido.huff";
            File.WriteAllBytes(@root, b);
        }

        public void Descomporimir()
        {
            string root2 = root;
            string root3 = root;
            root = @"..\\..\\..\\Comprimido.huff";
            byte[] fileBytes = Manejar.GetArchivoBytes(@"..\\..\\..\\Comprimido.huff");
            root3 =   @"Arbol\\Escritoriovpn.hdef";
            string bb = HuffmanEncodeFile.Decode(fileBytes, @root3);
            root = root2 + @"..\\..\\DescomprimidoHuff.txt";
            File.WriteAllText(@root, bb);
        }
    }
}
