using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Huffman
{ 
    [Serializable]
    public class Arbol
    {
        //nodo 
        public enum NodeDirection
        {
            Izquierdo,
            Derecho
        }
        public Arbol() { }


        public Arbol Padre = null;
        public Arbol HijoIzqu = null;
        public Arbol HijoDer = null;

        public bool BitVal;
        public int? Key { get; set; }
        public ulong Valor { get; set; }

        public void AgregandoHijos(Arbol hijoIzqNodo, Arbol hijoDerNodo)
        {
            AgragandoHijo(hijoIzqNodo, NodeDirection.Izquierdo);
            AgragandoHijo(hijoDerNodo, NodeDirection.Derecho);
        }

        public void AgragandoHijo(Arbol btn, NodeDirection nd)
        {
            btn.Padre = this;

            if (nd == NodeDirection.Izquierdo)
                this.HijoIzqu = btn;
            else
                this.HijoDer = btn;
        }

        public char KeyAsChar
        {
            get
            {
                return Convert.ToChar(this.Key);
            }
        }

        public void GuardarDisco(string path)
        {
            var fit = new FileInfo(path);

            path = path.Replace(fit.Extension, ".hdef");

            Serializar.GuardarArchivoBinario<Arbol>(path, this);
        }
    }
}
