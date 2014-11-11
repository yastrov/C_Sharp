/*Solution with lock may be bad! 
Стоит почитать: http://habrahabr.ru/post/240385/
See also: http://msdn.microsoft.com/en-us/magazine/cc163533.aspx

Next Quote from links:
В приведенном примере получаем и потокобезопасность, и «защиту»
от NullReferenceException, и reliability (вспомним про события вроде
DomainUnload, ProcessExit, and UnhandledException).

For CER (Constrained Execution Regions).
Replace:
RuntimeHelpers.PrepareContractedDelegate(value);
To:
RunteHimelpers.PrepareDelegate(value);
*/
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

    class DirWalker
    {
        public delegate void OnTakeFileEventHandler<TEventArgs>(object source, TEventArgs e);
        private static OnTakeFileEventHandler<TakeFileEventArgs> _takeFileEvent = delegate { };
        private readonly object _exceptionLock = new object();
        public event OnTakeFileEventHandler<TakeFileEventArgs> TakeFileEvent
        {
            add
            {
                if (value == null)
                    return;
                RuntimeHelpers.PrepareContractedDelegate(value);
                lock (_exceptionLock)
                {
                    _takeFileEvent += value;
                }
            }
            remove
            {
                lock (_exceptionLock)
                {
                    _takeFileEvent -= value;
                }
            }
        }

        public void Walk(string sourceDirectory, string searchPattern, SearchOption searchOptions = SearchOption.AllDirectories)
        {
            try
            {
                if (searchPattern == null) searchPattern = "*.*";
                var txtFiles = Directory.EnumerateFiles(sourceDirectory, searchPattern, searchOptions);
                foreach (string currentFile in txtFiles)
                {
                    _takeFileEvent(this, new TakeFileEventArgs(currentFile));
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }
    }
}