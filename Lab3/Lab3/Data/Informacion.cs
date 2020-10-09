using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3
{
    public sealed class Informacion
    {
        private readonly static Informacion _instance = new Informacion();
        public List<Json> informacion { get; set; }
        public int LastId = 5;
        private Informacion()
        {
                informacion = new List<Json>();
                Json newExchangeRate = new Json
                {
                    nombreArchivo = "",
                    ubicacionComprimido="",
                    factorCompresion = 1,
                    razonCompresion = 7.9,
                    porcentajeCompresion=3




                };
                informacion.Add(newExchangeRate);

               
        }

        public static Informacion Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
