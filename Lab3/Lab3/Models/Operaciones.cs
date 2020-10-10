using System.IO;
using System;


namespace Lab3
{
    public class Operaciones
    {
        
        public string path="";
        public string raiz = "";
        private static string NombreTemporal;
        public double PorcentajeCompresion;
        public double FactorCompresion;
        public double RazonCompresion;

        public Operaciones(string path, string root,string temporal)
        {
            this.path = path;
            this.raiz = root;
            NombreTemporal = temporal;
        }

        public void Comprimir()
        {
            string root2 = raiz;
            raiz = "Arbol" + @"\\Escritoriovpn.hdef";
            HuffmanEncodeFile hef = new HuffmanEncodeFile(@path, @raiz);
            byte[] b = hef.Encode();
            raiz = root2 + @"\\Upload\\Comprimido.huff";
            File.WriteAllBytes(@raiz, b);
            NombreTemporal = NombreTemporal.Replace(".txt", "");
           
        }

        public void Descomporimir()
        {
            string root2 = raiz;
            string root3 = "Arbol";
            string descomprimido = "";
            raiz = raiz + @"\\Upload\\Comprimido.huff";
            byte[] fileBytes = Manejar.GetArchivoBytes(raiz);
            root3 = root3 + @"\\Escritoriovpn.hdef";
            string bb = HuffmanEncodeFile.Decode(fileBytes, @root3);
            raiz = root2 + @"\\Upload\\descomprimidoHuff.txt";
            File.WriteAllText(@raiz, bb);
            raiz = root2 + @"\\Upload\\";
            string rootRazonFactor = raiz;
            NombreTemporal = NombreTemporal.Replace(".huff", "");
            string comprimido = System.IO.File.ReadAllText(@path);
            descomprimido = bb;

            FactorCompresion = Convert.ToDouble(comprimido.Length) / Convert.ToDouble(descomprimido.Length);
            RazonCompresion = Convert.ToDouble(descomprimido.Length) / Convert.ToDouble(comprimido.Length);
            PorcentajeCompresion = ((FactorCompresion / RazonCompresion) * 100);
            double raz = Math.Round(RazonCompresion, 2);
            double fac = Math.Round(FactorCompresion, 2);
            double porcent = Math.Round(PorcentajeCompresion, 2);
            rootRazonFactor = rootRazonFactor + @"FactorRazon.txt";
            File.WriteAllText(@rootRazonFactor,  raz.ToString() + Environment.NewLine +  fac.ToString() + Environment.NewLine + porcent.ToString());



        }
    }
}
