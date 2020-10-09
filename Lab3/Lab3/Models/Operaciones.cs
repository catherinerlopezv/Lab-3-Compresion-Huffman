using System.IO;
using System;


namespace Lab3
{
    public class Operaciones
    {
        
        public string path="";
        public string raiz = "";
      
        public Operaciones(string path, string root)
        {
            this.path = path;
            this.raiz = root;

        }

        public void Comprimir()
        {
            string root2 = raiz;
            raiz = "Arbol" + @"\\Escritoriovpn.hdef";
            HuffmanEncodeFile hef = new HuffmanEncodeFile(@path, @raiz);
            byte[] b = hef.Encode();
            raiz = root2 + @"\\Upload\\Comprimido.huff";
            File.WriteAllBytes(@raiz, b);
        }

        public void Descomporimir()
        {
            string root2 = raiz;
            string root3 = "Arbol";
            raiz = raiz + @"\\Upload\\Comprimido.huff";
            byte[] fileBytes = Manejar.GetArchivoBytes(raiz);
            root3 = root3 + @"\\Escritoriovpn.hdef";
            string bb = HuffmanEncodeFile.Decode(fileBytes, @root3);
            raiz = root2 + @"\\Upload\\descomprimidoHuff.txt";
            File.WriteAllText(@raiz, bb);
        }
    }
}
