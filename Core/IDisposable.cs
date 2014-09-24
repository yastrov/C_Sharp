public class MyClass: IDisposable
{
    private bool disposed = false;
    
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed) {
            if (disposing) {
                if (myTimer != null) {
                    myTimer.Stop();
                    myTimer.Dispose();
                }
            }
        // Free native resourced here
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~MyClass()
    {
        Dispose(false);
    }
}