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

        public int this[string propName]
        {
            get
            {
                return (int)this.GetType().GetProperty(propName).GetValue(this, null);
            }
            set
            {
                PropertyInfo propertyInfo = this.GetType().GetProperty(propName);
                propertyInfo.SetValue(this, Convert.ChangeType(value, propertyInfo.PropertyType), null);
            }
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