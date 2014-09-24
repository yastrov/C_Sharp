using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConsoleApplication2
{
    class Test {
        public int X { get { return 0; } set { } }
        public void Hello()
        {
            ;
        }
    }

    static void Main(string[] args)
    {
        Console.WriteLine( Assembly.GetExecutingAssembly().Location);
        foreach (MethodInfo mi in typeof(Test).GetMethods())
        {
            Console.Write(mi.Name + " ");
        }
    }
}