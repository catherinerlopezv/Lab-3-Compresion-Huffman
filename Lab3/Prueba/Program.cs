using System;
using Huffman;

namespace Prueba
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
      
            
            Operaciones imp = new Operaciones("Prueba", "Algo");

            imp.Comprimir();
            
            

        }
    }
}
