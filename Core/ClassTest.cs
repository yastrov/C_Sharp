﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTest
{
    public interface IMyInterface {
        void Foo();
        int X
        {
            get;
            set;
        }
    }

    public interface IFoo : IMyInterface {
        int Bar();
    }

    // sealed class You can't inherit. 
    public class A : IMyInterface, IComparable<A>
    {
        public readonly int ReadonlyValue = 1; /*Вычисляется на стадии выполнения, можно присваивать в конструкторе*/
        public const int ConstValue = 1; /*Вычисляетяс на стадии компиляциия*/

        private int _x = 0;
        public int X {
            get { return _x; }
            set { if (value > 0) _x = value;}
        }
        public int Y { get; set; }
        // Constructor
        public A() { ; }
        public A(int x) { this._x = x; }
        ~A() { ; } //Not null finalizer is bad for perfomanse.

        // Преобразование пользовательского типа
        /*Пример: int x = 1; A a = (A)x;*/
        public static explicit operator A(int Value)
        {
            return new A(Value);
        }

        public virtual Boolean Equals(Object obj)
        {
            if (obj == null) return false;
            if (this.GetType() != obj.GetType())
                return false;
            // If this class is not base, use: base.Equals(obj) && Z == obj.Z
            return this._x == ((A)obj).X;
        }

        /* Richter CLR via C#.
         You shouldn’t use the C#  == operator;
         AT! This overload may be critical error!!!
        */
        public static Boolean ReferenceEquals(Object objA, Object objB) {
            return (objA == objB);
        }

        public void Foo()
        {
            // In real application, pass Console.WriteLine as Action<string>;
            //Action<string> Foo = Console.WriteLine;
            Console.WriteLine("Foo from A.");
        }
        public virtual void Hello()
        {
            Console.WriteLine("Hello from A.");
        }
        // protected - you can use in inheritance class
        // private - for this class only (not for inheritance)

        public int this[string index]
        {
            get
            {
                return Int32.Parse(index);
            }
            set
            {
                // vars: index and value in this scope
                this._x = value;
            }
        }

        // IComparable<T>
        public int CompareTo(A obj)
        {
            if (this._x > obj.X)
                return 1;
            if (this._x < obj.X)
                return -1;
            else
                return 0;
        }
    }

    // may have parents: one class and many interfaces
    public class B : A
    {
        public B() : base() { ; }
        public B(int x) : base(x) { ; }

        public void Foo()
        {
            Console.WriteLine("Foo from B.");
            base.Foo();
        }
        public override void Hello()
        {
            Console.WriteLine("Hello from B.");
            base.Hello();
        }
    }

    // There are one class with name "C":
    public partial class C
    {
        public void Fizz()
        {
        }
    }

    public partial class C
    {
        public void Buzz()
        {
        }
    }

    class Program
    {
        static void CallFoo(IMyInterface obj) {
            obj.Foo();
        }

        static void MethodA(ref int i)
        {
            // i must be initialized
            i = i + 6;
        }
        static void MethodB(out int i)
        {
            i = 7;
        }
        static void Main(string[] args)
        {
            A a = new A();
            a.Y = 8;
            int i = a["6"];
            Console.WriteLine(a.Y);
            IMyInterface aa = a;
            B b = new B();
            b.Hello();
            b.Foo();
            IMyInterface bb = b as IMyInterface;
            if (bb != null) {
                Console.WriteLine("bb is IMyInterface!");
            }
            bool flag = b is B;
            int j;
            MethodB(out j);
            MethodA(ref j);
        }
    }
}
