﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Huffman
{
    internal class Serializar
    {
        internal static void GuardarArchivoBinario<T>(string filePath, T pObject)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    var formatBin = new BinaryFormatter();

                    formatBin.Serialize(memoryStream, pObject);

                    string path = Path.GetDirectoryName(filePath);

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    File.WriteAllBytes(filePath, memoryStream.ToArray());

                    memoryStream.Close();
                }
            }
            catch
            {
                throw;
            }
        }
        internal static T DeserializeBinaryFile<T>(string filePath)
        {
            BinaryFormatter formatBin = new BinaryFormatter();
            using (var memoryStream = new MemoryStream(File.ReadAllBytes(filePath)))
            {
                memoryStream.Position = 0;
                return (T)formatBin.Deserialize(memoryStream);
            }
        }
    }
}
