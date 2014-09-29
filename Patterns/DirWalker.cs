using System;
using System.IO;
using System.ServiceModel;
using System.Xml;
using System.Xml.Serialization;

namespace DirWalkNamespace {
    public class TakeFileEventArgs : System.EventArgs
    {
        public readonly string FileName;
        public TakeFileEventArgs(string FileName)
        {
            this.FileName = FileName;
        }
    }

    public delegate void OnTakeFileEventHandler<TEventArgs>(object source, TEventArgs e);

    public class DirectoryWalker {
        private readonly object someEventLock = new object();

        public event OnTakeFileEventHandler<TakeFileEventArgs> TakeFileEvent
        {
            add
            {
                lock (someEventLock)
                {
                    TakeFileEvent += value;
                }
            }
            remove
            {
                lock (someEventLock)
                {
                    TakeFileEvent -= value;
                }
            }
        }

        protected virtual OnTakeFileEvent(TakeFileEventArgs e)
        {
            OnTakeFileEventHandler handler;
            // Thread safe
            lock(someEventLock)
            {
                handler = OnTakeFileEvent;
            }
            if (handler != null)
            {
                handler(this, e);
            }
            else throw new NotImplementedException("DirectoryWalker: TakeFileEvent not implemented!");
        }

        /*
         public void ProcessFile (string filename){}
         DirectoryWalker dW = new DirectoryWalker();
         dw.OnTakeFile += this.ProcessFile;
         */

        public DirectoryWalker()
        {
            ;
        }

        public void Walk(string sourceDirectory, string searchPattern, SearchOption searchOptions = SearchOption.AllDirectories)
        {
            try
            {
                if(searchPattern == null) searchPattern = "*.*";
                var txtFiles = Directory.EnumerateFiles(sourceDirectory, searchPattern, searchOptions);
                string fileName = null;
                foreach (string currentFile in txtFiles)
                {
                    fileName = currentFile.Substring(sourceDirectory.Length + 1);
                    OnTakeFileEvent(new TakeFileEventArgs(fileName));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}