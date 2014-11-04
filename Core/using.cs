using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingTest
{
    class MyException : Exception
    {
        private string value = null;
        public MyException(string message)
        {
            this.value = message;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/fs2xkftw.aspx
    /// http://msdn.microsoft.com/ru-ru/library/fs2xkftw%28v=vs.110%29.aspx
    /// Use Dispose pattern, if you have unmanaged resource in your class
    /// or want to have Close() method.
    /// </summary>
    class A : IDisposable {
        bool disposed = false;

        public A()
        {
            Console.WriteLine("Instance of A creating.");
        }

        public void DoSmth()
        {
            Console.WriteLine("A do something.");
        }
        public void TryExcept()
        {
            throw new MyException("Hello world!");
        }

        public void Close()
        {
            Console.WriteLine("A Close()");
        }

        public void Dispose()
        {
            // Perform cleanup / tear-down.
            Console.WriteLine("Instance of A class disposing");
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
                Console.WriteLine("Instance of A class Dispose(bool).");
            }

            // Free any unmanaged objects here. 
            disposed = true;
        }
        ~A()
        {
            Console.WriteLine("Instance of A class detructing.");
            Dispose(false);
        }
    }

    class B : A
    {
        // Flag: Has Dispose already been called?
        bool disposed = false;

        public B() : base()
        {
            Console.WriteLine("Instance of B creating.");
        }

        // Protected implementation of Dispose pattern.
        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                Console.WriteLine("Instance of B class Dispose(bool).");
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        ~B()
        {
            Console.WriteLine("Instance of B class detructing.");
            Dispose(false);
        }
    }

    class Program
    {
        private static A getA()
        {
            A a = new A();
            return a;
        }
        private static B getB()
        {
            B a = new B();
            return a;
        }
        static void Main(string[] args)
        {
            using (A aa = getA())
            {
                aa.DoSmth();
                try
                {
                    aa.TryExcept();
                }
                catch (MyException e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            Console.WriteLine("End of using A class obj.");
            using (B aa = getB())
            {
                aa.DoSmth();
            }
            Console.WriteLine("End of using B class obj.");
            A aaa = new A();
            aaa.DoSmth();
            Console.WriteLine("End of program.");
        }
    }
}