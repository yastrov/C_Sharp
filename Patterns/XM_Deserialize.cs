using System;
using System.IO;
using System.ServiceModel;
using System.Xml;
using System.Xml.Serialization;
using System.IO.Compression;
//using System.IO.Compression.FileSystem; //You must add it in References.

namespace FB2Deser
{
    [XmlSerializerFormat]
    [XmlRoot(ElementName = "FictionBook", Namespace = "http://www.gribuser.ru/xml/fictionbook/2.0")]
    public class FictionBook
    {
        [XmlElement(ElementName = "description", Namespace = "http://www.gribuser.ru/xml/fictionbook/2.0")]
        public Description description { get; set; }

        public static FictionBook Deserialize(string filename)
        {
            if (filename.EndsWith(".zip"))
                return FictionBookZipReader.ReadZipAsFB2(filename);
            FictionBook fb2book = null;
            XmlSerializer serializer = new XmlSerializer(typeof(FictionBook));
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                using (XmlReader reader = XmlReader.Create(fs))
                {
                    //StreamReader reader = new StreamReader(fs);
                    fb2book = (FictionBook)serializer.Deserialize(reader);
                }
            }
            return fb2book;
        }

        public static FictionBook Deserialize(Stream stream)
        {
            FictionBook fb2book = null;
            XmlSerializer serializer = new XmlSerializer(typeof(FictionBook));
            using (XmlReader reader = XmlReader.Create(stream))
            {
                //StreamReader reader = new StreamReader(fs);
                fb2book = (FictionBook)serializer.Deserialize(reader);
            }
            return fb2book;
        }
    }

    public class Description
    {
        [System.Xml.Serialization.XmlElement(ElementName = "title-info", Namespace = "http://www.gribuser.ru/xml/fictionbook/2.0")]
        public TitleInfo titleInfo { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "document-info", Namespace = "http://www.gribuser.ru/xml/fictionbook/2.0")]
        public DocumentInfo documentInfo { get; set; }
    }

    public class TitleInfo
    {
        [System.Xml.Serialization.XmlElement(ElementName = "genre", Namespace = "http://www.gribuser.ru/xml/fictionbook/2.0")]
        public string Genre { get; set; }
    }
    public class DocumentInfo {
        [System.Xml.Serialization.XmlElement(ElementName = "version", Namespace = "http://www.gribuser.ru/xml/fictionbook/2.0")]
        public string Version { get; set; }
    }
    
    public class FictionBookZipReader
    {
        public static Stream ReadZipAsStream(string zipPath)
        {
            Stream st = null;
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        st = entry.Open();
                        break;
                        //entry.ExtractToFile(Path.Combine(extractPath, entry.FullName));
                    }
                }
            }
            return st;
        }

        public static FictionBook ReadZipAsFB2(string zipPath)
        {
            FictionBook fb2book = null;
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.EndsWith(".fb2", StringComparison.OrdinalIgnoreCase))
                    {
                        fb2book = FictionBook.Deserialize( entry.Open());
                        break;
                        //entry.ExtractToFile(Path.Combine(extractPath, entry.FullName));
                    }
                }
            }
            return fb2book;
        }
    }
}