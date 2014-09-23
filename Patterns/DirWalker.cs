using System;
using System.IO;
using System.ServiceModel;
using System.Xml;
using System.Xml.Serialization;

namespace DirWalkNamespace {
    public class DirectoryWalker {

        public delegate void ProcessFile(string fileName);
        public event ProcessFile OnTakeFile;
        /*
         public void ProcessFile (string filename){}
         DirectoryWalker dW = new DirectoryWalker();
         dw.OnTakeFile += this.ProcessFile;
         */

        public DirectoryWalker()
        {
            ;
        }

        public void Walk(string sourceDirectory, string searchPattern)
        {
            Walk(sourceDirectory, searchPattern, SearchOption.AllDirectories);
        }

        public void Walk(string sourceDirectory, string searchPattern, SearchOption searchOptions)
        {
            try
            {
                if(searchPattern == null) searchPattern = "*.*";
                var txtFiles = Directory.EnumerateFiles(sourceDirectory, searchPattern, searchOptions);
                string fileName = null;
                foreach (string currentFile in txtFiles)
                {
                    fileName = currentFile.Substring(sourceDirectory.Length + 1);
                    if(OnTakeFile != null)
                        OnTakeFile(fileName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}