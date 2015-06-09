/*Example for overloadind base operators*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OperatorOverloadTest
{
    class MyPoint : IComparable<MyPoint>
    {
        private int x, y;
        public int X
        {
            get { return x; }
            private set { ; }
        }
        public int Y
        {
            get { return y; }
            private set { ; }
        }
        #region constructors
        public MyPoint(int x=0, int y=0)
        {
            this.x = x;
            this.y = y;
        }
        public MyPoint(MyPoint obj)
        {
            this.x = obj.X;
            this.y = obj.Y;
        }
        #endregion

        #region indexator overload
        /// <summary>
        /// Overload indexator for change attributes via Reflection and string name
        /// </summary>
        /// <param name="propName">Name of attribute</param>
        /// <returns></returns>
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
        #endregion

        // Overload all or none in each region !
        #region ==, !=
        /// https://msdn.microsoft.com/ru-ru/library/ms173147(v=vs.90).aspx
        public static bool operator ==(MyPoint obj1, MyPoint obj2)
        {
            if (System.Object.ReferenceEquals(obj1, obj2))
            {
                return true;
            }
            if (((object)obj1 == null) || ((object)obj2 == null))
            {
                return false;
            }
            if ((obj1.X == obj2.X) && (obj1.Y == obj2.Y))
                return true;
            return false;
        }

        public static bool operator !=(MyPoint obj1, MyPoint obj2)
        {
            if (System.Object.ReferenceEquals(obj1, obj2))
            {
                return false;
            }
            if (((object)obj1 == null) || ((object)obj2 == null))
            {
                return false;
            }
            if ((obj1.X != obj2.X) && (obj1.Y != obj2.Y))
                return false;
            return true;
        }
        #endregion

        #region > <
        public static bool operator >(MyPoint obj1, MyPoint obj2)
        {
            if ((obj1.X > obj2.X) && (obj1.Y > obj2.Y))
                return true;
            return false;
        }

        public static bool operator <(MyPoint obj1, MyPoint obj2)
        {
            if ((obj1.X < obj2.X) || (obj1.Y < obj2.Y))
                return false;
            return true;
        }
#endregion

        #region > <
        public static bool operator >=(MyPoint obj1, MyPoint obj2)
        {
            if ((obj1.X >= obj2.X) && (obj1.Y >= obj2.Y))
                return true;
            return false;
        }

        public static bool operator <=(MyPoint obj1, MyPoint obj2)
        {
            if ((obj1.X <= obj2.X) || (obj1.Y <= obj2.Y))
                return false;
            return true;
        }
        #endregion

        #region true, false
        public static bool operator false(MyPoint obj)
        {
            if ((obj.X <= 0) || (obj.Y <= 0))
                return true;
            return false;
        }

        public static bool operator true(MyPoint obj)
        {
            if ((obj.X > 0) && (obj.Y > 0))
                return true;
            return false;
        }
        #endregion

        #region & !
        public static bool operator &(MyPoint obj1, MyPoint obj2)
        {
            if (((obj1.X > 0) && (obj1.Y > 0))
                & ((obj2.X > 0) && (obj2.Y > 0)))
                return true;
            return false;
        }

        public static bool operator !(MyPoint obj1)
        {
            if ((obj1.X > 0) && (obj1.Y > 0))
                return false;
            return true;
        }
        #endregion

        #region converters operator
        /// <summary>
        /// Явное приведение типа
        /// string s = (string)myObj;
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static explicit operator string(MyPoint obj)
        {
            return String.Format("Coord x: {0} y: {1}", obj.X, obj.Y);
        }
        /// <summary>
        /// Неявное приведение типа
        /// double length = myObj;
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static implicit operator double(MyPoint obj)
        {
            return Math.Sqrt(obj.X*obj.X + obj.Y*obj.Y);
        }
        #endregion
        public override string ToString()
        {
            return String.Format("Coord x: {0} y: {1}", this.x, this.y);
        }
        public override Boolean Equals(Object obj)
        {
            if (obj == null) return false;
            if (this.GetType() != obj.GetType())
                return false;
            // If this class is not base, use: base.Equals(obj) && Z == obj.Z
            if ((this.X == ((MyPoint)obj).X )&& (this.Y == ((MyPoint)obj).Y))
                return true;
            return false;
        }
        public Boolean Equals(MyPoint obj)
        {
            if ((object)obj == null) return false;
            if ((this.X == obj.X) && (this.Y == obj.Y))
                return true;
            // If this class is not base, use: base.Equals(obj) && Z == obj.Z
            return false;
        }
        public int CompareTo(MyPoint other)
        {
            if(this < other)
                return -1;
            if (this > other)
                return 1;
            return 0;
        }
        public override int GetHashCode()
        {
            // Or 31 and 29
            int hash = 13;
hash = (hash * 7) + x.GetHashCode();
hash = (hash * 7) + y.GetHashCode();
            return hash;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            MyPoint p1 = new MyPoint(x: 1, y: 2);
            Console.WriteLine((string)p1);
            Console.WriteLine(p1["X"].ToString());
            MyPoint p2 = new MyPoint(x: -1, y: -2);
            Console.WriteLine("p1 == p2 : {0}", p1 == p2);
            Console.WriteLine("p1.CompareTo(p2) : {0}", p1.CompareTo(p2));
            double d = p1;
        }
    }
}
