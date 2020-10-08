using System.IO;


namespace Huffman
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
            string raiz2 = raiz;
            raiz = raiz + @"\\Escritoriovpn.hdef";
            HuffmanEncodeFile hef = new HuffmanEncodeFile(@path, @raiz);
            byte[] b = hef.Encode();
            raiz= raiz2 + @"\\Upload\\Comprimido.huff";
            File.WriteAllBytes(@raiz, b);
        }

        public void Descomporimir()
        {
            string raiz2 = raiz;
            string raiz3 = raiz;
            raiz = raiz + @"\\Upload\\Comprimido.huff";
            byte[] fileBytes = FileHelper.GetArchivoBytes(@raiz);
            raiz3 = raiz3 + @"\\Escritoriovpn.hdef";
            string bb = HuffmanEncodeFile.Decode(fileBytes, @raiz3);
            raiz = raiz2 + @"\\Upload\\DescomprimidoHuff.txt";
            File.WriteAllText(@raiz, bb);
        }
    }
}
