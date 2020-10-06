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
        public enum NodeDirection
        {
            Left,
            Right
        }
        public Arbol() { }


        public Arbol Padre = null;
        public Arbol IzqHijo = null;
        public Arbol DerHijo = null;

        public bool BitValue;
        public int? Key { get; set; }
        public ulong Value { get; set; }

        public void AgregandoHijos(Arbol hijoIzqNodo, Arbol hijoDerNodo)
        {
            AgragandoHijo(hijoIzqNodo, NodeDirection.Left);
            AgragandoHijo(hijoDerNodo, NodeDirection.Right);
        }

        public void AgragandoHijo(Arbol btn, NodeDirection nd)
        {
            btn.Padre = this;

            if (nd == NodeDirection.Left)
                this.IzqHijo = btn;
            else
                this.DerHijo = btn;
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

            Serializer.GuardarArchivoBinario<Arbol>(path, this);
        }
    }
}
