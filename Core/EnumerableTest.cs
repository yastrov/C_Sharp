/*
See: for more information
(Second method of declarating):
http://sergeyteplyakov.blogspot.ru/2012/08/duck-typing-foreach.html

yield break;
yield return 5;
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumerableTest
{
    public static class Extender
    {
        public static IEnumerable<int> Range(this int value, int max)
        {
            for (int i = value; i <= max; i++)
                yield return i;
        }

        public static IEnumerable<int> Add(this IEnumerable<int> collection, int addV)
        {
            foreach (int v in collection)
                yield return v + addV;
        }
    }

    //public class MyEnumerable<T> : IEnumerable<T>
    public class MyEnumerable : IEnumerable<int>
    {
        private int max = 0;

        public MyEnumerable(int max)
        {
            this.max = max;
        }

        //public IEnumerator<T> GetEnumerator()
        public IEnumerator<int> GetEnumerator()
        {
            // Or return yield here!
            return new MyEnumerator(max);
        }

        // System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        IEnumerator IEnumerable.GetEnumerator()
        {
            // return this.GetEnumerator(max);
            return new MyEnumerator(max);
        }
    }
    //public class MyEnumerator<T> : IEnumerator<T>
    public class MyEnumerator : IEnumerator<int>
    {
        private int x = 0;
        private int max = 0;

        public MyEnumerator(int max) 
        {
            this.max = max;
        }

        public int Current
        {
            get { return x; }
        }

        public void Dispose()
        {
            // do nothing
        }

        object IEnumerator.Current
        {
            get { return x; }
        }

        public bool MoveNext()
        {
            return x++ < max;
        }

        public void Reset()
        {
            // Must Call!
            Console.WriteLine("Reset Called");
            x = 0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int x = 1;
            foreach (int i in x.Range(6))
                Console.WriteLine(i);
            Console.WriteLine("----------------");
            foreach (int i in new MyEnumerable(6))
                Console.WriteLine(i);
        }
    }
}
