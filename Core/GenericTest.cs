using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenricTest
{
    public interface IA<T>
    {
        T Value { get; set; }
    }

    public interface IB<T> : IA<T>
    {
        T Foo();
    }

    public class  MyClass<T>: IB<T>
    {
        private T _value;
        public T Value {
            get { return this._value; }
            set { this._value = value; }
        }
        public MyClass(T value)
        {
            this.Value = value;
        }

        public T Foo()
        {
            return this._value;
        }
    }

    class Program
    {
        // Type with parametherless constructor
        // static T Max <T>(T a, T b) where T : new()
        // AT! This is more faster than next: 
        // static IComparable Max (IComparable a, IComparable b)
        // And more than Struct or interface, in place of 'T'
        static T Max <T>(T a, T b) where T : IComparable<T>
        {
            return a.CompareTo(b) > 0 ? a : b;
        }

        static void Main(string[] args)
        {
            Console.WriteLine(Max<int>(1,4));
            Console.WriteLine(Max(1, 4));
            Console.WriteLine(new MyClass<int>(5).Foo());
        }
    }
}
