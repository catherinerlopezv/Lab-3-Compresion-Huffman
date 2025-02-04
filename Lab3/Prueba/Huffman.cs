﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Huffman
{
    public class HuffmanEncodeFile : Huffman
    {
        private string _compressedFileName = String.Empty;

        public HuffmanEncodeFile(string filePath, string huffmanTreeFilePath)
            : base(filePath, huffmanTreeFilePath)
        {
            if (filePath != String.Empty)
                LoadText(new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read)));
        }
    }

    public class HuffmanEncodeText : Huffman
    {
        public HuffmanEncodeText(string text)
        {
            byte[] myByteArray = Encoding.ASCII.GetBytes(text);
            var ms = new MemoryStream(myByteArray);

            LoadText(new StreamReader(ms));
        }
    }

    public abstract class Huffman
    {

        private List<Arbol> _priorityQueue = new List<Arbol>();
        private LeafList _leafList = new LeafList();
        private List<bool> _bitString = new List<bool>();
        private List<byte> _fullText = new List<byte>();

        private string _huffmanPath = String.Empty;
        private string _filePath = String.Empty;

        private Dictionary<int?, List<bool>> encodeTable = new Dictionary<int?, List<bool>>();

        public Arbol HuffmanTree { get; set; }

        public string FilePath
        {
            get { return _filePath; }
        }

        public string HuffmanPath
        {
            get { return _huffmanPath; }
        }

        internal Huffman() { }

        internal Huffman(string filePath, string huffmanPath)
        {
            _filePath = filePath;
            _huffmanPath = huffmanPath;
        }

        public byte[] Encode()
        {
            HuffmanTree = BuildHuffmanTree(_priorityQueue);

            BuildLookupTable();

            //Loop through the text one character at a time, replacing a character token with its bit string equivalent
            var bitBuffer = new List<bool>();
            var outputBytes = new List<byte>();

            //Start-up local delegate to process bytes
            Action processBytes = () =>
            {
                outputBytes.Add(ConvertToByte(new BitArray(bitBuffer.GetRange(0, 8).ToArray())));
                bitBuffer.RemoveRange(0, 8);
            };

            foreach (var b in Manejar.GetArchivoBytes(_filePath))
            {
                bitBuffer.AddRange(encodeTable[b]);

                if (bitBuffer.Count > 8)
                    processBytes();
            }

            //Add the EOF bits
            bitBuffer.AddRange(encodeTable[-1]);

            while (bitBuffer.Count > 8)
                processBytes();

            //Handle the trailing bits
            bool[] finalByte = new bool[8];

            if (bitBuffer.Count > 0)
                Array.Copy(bitBuffer.ToArray(), finalByte, bitBuffer.Count);

            outputBytes.Add(ConvertToByte(new BitArray(finalByte)));

            if (_huffmanPath != String.Empty)
                HuffmanTree.GuardarDisco(_huffmanPath);

            return outputBytes.ToArray();
        }

        public static string Decode(byte[] bytes, string direccion)
        {
            return Decode(bytes, Serializar.DeserializeBinaryFile<Arbol>(direccion));
        }

        public static string Decode(byte[] bytes, Arbol huffmanTree)
        {
            if (bytes == null)
                throw new ArgumentNullException("Byte array for Huffman Tree decoding cannot be null.");

            if (huffmanTree == null)
                throw new ArgumentNullException("Huffman tree for decoding cannot be null.");

            var sb = new StringBuilder();
            var localNode = huffmanTree;
            var bits = new BitArray(bytes);

            //Loop through the bits
            foreach (bool bit in bits)
            {
                if (!bit)
                    localNode = localNode.HijoIzqu;
                else
                    localNode = localNode.HijoDer;

                if (localNode.Key == -1)
                    break;

                if (localNode.Key == null)
                    continue;

                sb.Append(localNode.KeyAsChar);

                localNode = huffmanTree;
            }

            return sb.ToString();
        }

        private void BuildLookupTable()
        {
            //Loop through the leaves and build the bit string lookup table
            foreach (var btn in _leafList)
            {
                CrawlTree(btn);

                encodeTable.Add(btn.Key, new List<bool>(_bitString));

                _bitString.Clear();
            }
        }

        private Arbol BuildHuffmanTree(List<Arbol> priorityQueue)
        {
            if (priorityQueue.Count == 0)
                throw new ArgumentException("Priority Queue is empty.", "priorityQueue");

            //When the queue reaches one item we have a fully formed Huffman Tree
            while (priorityQueue.Count > 1)
            {
                //Order ascending on the order of values so low values are first.
                //Sort descending on the key so that newly formed trees are pushed behind "un-treed" values
                priorityQueue = priorityQueue.OrderBy(x => x.Valor).ThenByDescending(x => x.Key).ToList();

                var btnLeft = priorityQueue[0];
                btnLeft.BitVal = false;

                var btnRight = priorityQueue[1];
                btnRight.BitVal = true;

                //New parent node gets the value of the two Nodes combined value
                var btnParent = new Arbol() { Key = null, Valor = btnLeft.Valor + btnRight.Valor };

                btnParent.AgregandoHijos(btnLeft, btnRight);

                priorityQueue.RemoveRange(0, 2);

                priorityQueue.Add(btnParent);

                //Collect the leaves into a table so we don't have to traverse the binary
                _leafList.AddRange(btnLeft, btnRight);
            }

            return priorityQueue[0];
        }

        //Recurse through the leaves up to the parent and count the number of bits until the root is hit
        private void CrawlTree(Arbol btn)
        {
            if (btn.Padre == null)
                return;

            CrawlTree(btn.Padre);

            //Add the keys recursively so it reads correctly when travelling down the tree from root.
            _bitString.Add(btn.BitVal);
        }

        private byte ConvertToByte(BitArray bits)
        {
            if (bits.Count > 8)
                throw new ArgumentException("ConvertToByte can only work with a BitArray containing a maximum of 8 values");

            byte result = 0;

            for (byte i = 0; i < bits.Count; i++)
                if (bits[i])
                    result |= (byte)(1 << i);

            return result;
        }

        internal void LoadText(StreamReader sr)
        {
            int code = 0;
            var lookup = new Dictionary<int?, ulong>();

            for (int i = 0; i < 128; i++)
                lookup.Add(i, 0);

            using (sr)
            {
                while (sr.Peek() != -1)
                {
                    code = sr.Read();

                    //Check if the character is ASCII, if not, throw an error
                    if (code > 127)
                        throw new Exception("Text must include only items from the ASCII character set: Code: " + code + " Char: " + Convert.ToChar(code));

                    lookup[code]++;
                }

                sr.Close();
            }

            //Ignore the chars with a count of 0
            foreach (var kvp in lookup)
                if (kvp.Value > 0)
                    _priorityQueue.Add(new Arbol() { Key = kvp.Key, Valor = kvp.Value });

            //Add EOF character
            _priorityQueue.Add(new Arbol() { Key = -1, Valor = 1 });
        }
    }

    internal class LeafList : List<Arbol>
    {
        private new void Add(Arbol leaf)
        {
            if (leaf.Key != null && !Exists(x => x.Key == leaf.Key))
                base.Add(leaf);
        }

        public void AddRange(params Arbol[] collection)
        {
            foreach (var btn in collection)
                Add(btn);
        }
    }
}
