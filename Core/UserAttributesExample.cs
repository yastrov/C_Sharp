using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;

namespace TestAttributes
{

    [AttributeUsage(AttributeTargets.Method 
                    | AttributeTargets.Field
                    | AttributeTargets.Property)]
    public sealed class MyFirstAttribute : System.Attribute
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        public MyFirstAttribute(string message)
        {
            Message = message;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class MySecondAttribute : System.Attribute
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        public MySecondAttribute(string message)
        {
            Message = message;
        }
    }

    [MySecond("It's for class")]
    class MyClass
    {
        private string _desc;
        [MyFirst("It's field")]
        public string Description
        {
            get { return _desc; }
            set { _desc = value; }
        }
        public MyClass(string s)
        {
            Description = s;
        }
    }

    class Program
    {
        static void PrintAttributesForClass(Type t)
        {
            Console.WriteLine("Attributes for Class:");
            IEnumerable<Attribute> attrs = t.GetCustomAttributes(typeof(MySecondAttribute));
            foreach (var a in attrs)
            {
                string s = (a as MySecondAttribute).Message;
                Console.WriteLine("\t{0}", s);
            }
        }

        static void PrintAttributesForEachPropertyes(Type t, MyClass myObj)
        {
            PropertyInfo[] propertyInfos = t.GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                Console.WriteLine("Property Name: {0}", propertyInfo.Name);
                Console.WriteLine("\tValue: {0}", propertyInfo.GetValue(myObj));
                Console.WriteLine("\tMessages from Attribute:");
                Attribute[] attrs = Attribute.GetCustomAttributes(propertyInfo, typeof(MyFirstAttribute));
                foreach (var attr in attrs)
                {
                    var msg = (attr as MyFirstAttribute).Message;
                    Console.WriteLine("\t\t{0}", msg);
                }
            }
        }

        static void PrintAttrForProperty(Type t, MyClass myObj, string propName)
        {
            Console.WriteLine("Single Value for property: {0} from: {1} via Attribute", propName, t.Name);
            PropertyInfo propertyInfo = t.GetProperty(propName);
            Attribute attribut = (MySecondAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(MySecondAttribute));
            Console.WriteLine("\t{0}", propertyInfo.GetValue(myObj)); 
        }

        static void Main(string[] args)
        {
            Type t = typeof(MyClass);
            MyClass myObj = new MyClass("Hello world");
            PrintAttributesForClass(t);
            PrintAttrForProperty(myObj.GetType(), myObj, "Description");
            PrintAttributesForEachPropertyes(t, myObj);

            Console.WriteLine("All types in current Assembly:");
            Type[] types = Assembly.GetEntryAssembly().GetTypes();
            foreach (Type DefinedType in types)
            {
                Console.WriteLine("\t{0}",DefinedType);
            }
        }
    }
}
