using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;


namespace SerializerUtility
{
    public class DataManagement
    {

        public static void Save(Stream File, LevelUtility.LevelBluePrint data)
        {
            if (File != null)
            {
                //var fullPath = Path.GetFullPath(FileName);
                //var savePath = fullPath + @"\" + FileName;
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(File, data);
                File.Close();
            }


        }

        public static List<LevelUtility.LevelBluePrint> Load(string FileName)
        {


            if (File.Exists(FileName))
            {
                //var newFilePath = _filePath + @"\" + FileName;
                Stream ExistingFile = File.OpenRead(FileName);
                BinaryFormatter deserializer = new BinaryFormatter();
                var data = (List<LevelUtility.LevelBluePrint>)deserializer.Deserialize(ExistingFile);
                ExistingFile.Close();

                Console.WriteLine("File Loaded");

                return data;
            }

            else
            {
                Console.WriteLine("No File Found");
                return new List<LevelUtility.LevelBluePrint>();
            }

        }

        public static List<LevelUtility.LevelBluePrint> Load(Stream File)
        {
            var data = new List<LevelUtility.LevelBluePrint>();

            if (File != null)
            {
                //var newFilePath = _filePath + @"\" + FileName;
                BinaryFormatter deserializer = new BinaryFormatter();
                data = (List<LevelUtility.LevelBluePrint>)deserializer.Deserialize(File);
                File.Close();
            }

            return data;

        }
    }

    public class DataManagementXML
    {
        public static void Save(Stream File, LevelUtility.LevelBluePrint data)
        {
            if (File != null)
            {
                DataContractSerializer Serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(LevelUtility.LevelBluePrint));

                Serializer.WriteObject(File, data);
                File.Close();
            }
        }

        public static List<LevelUtility.LevelBluePrint> Load(string FileName)
        {
            var list = new List<LevelUtility.LevelBluePrint>();
            if (File.Exists(FileName))
            {
                //var newFilePath = _filePath + @"\" + FileName;
                Stream ExistingFile = File.OpenRead(FileName);

                //XmlDictionaryReader XmlReader = XmlDictionaryReader.CreateTextReader(ExistingFile, new System.Xml.XmlDictionaryReaderQuotas());
                DataContractSerializer deserializer = new DataContractSerializer(typeof(LevelUtility.LevelBluePrint));
                var data = (LevelUtility.LevelBluePrint)deserializer.ReadObject(ExistingFile);
                ExistingFile.Close();
                list.Add(data);
                Console.WriteLine("File Loaded");

                return list;
            }

            else
            {
                Console.WriteLine("No File Found");
                return new List<LevelUtility.LevelBluePrint>();
            }
        }
        public static List<LevelUtility.LevelBluePrint> Load(Stream File)
        {
            var data = new List<LevelUtility.LevelBluePrint>();

            if (File != null)
            {
                //var newFilePath = _filePath + @"\" + FileName;
                //XmlDictionaryReader XmlReader = XmlDictionaryReader.CreateTextReader(File, new System.Xml.XmlDictionaryReaderQuotas());
                DataContractSerializer deserializer = new DataContractSerializer(typeof(LevelUtility.LevelBluePrint));
                var Obj = (LevelUtility.LevelBluePrint)deserializer.ReadObject(File);
                File.Close();
                data.Add(Obj);
            }

            return data;

        }
    }

    //public static class LevelFiles
    //{
    //    public static int getFileCount()
    //    {
            
    //    }
    //}
    
}
