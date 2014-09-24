using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XMLTest
{
    public class MyXML
    {
        private void DiveIntoElement(XElement element)
        {
            Console.WriteLine(String.Format("    DiveTo: {0}\n", element.Name));
            foreach (XElement el in element.Elements())
                if (!el.HasElements)
                    Console.WriteLine(String.Format("        {0}: {1}\n", el.Name, el.Value));
                else
                    DiveIntoElement(el);
        }

        public void DiveXML(string FileName)
        {
            XDocument doc = XDocument.Load(FileName);
            XElement description = doc.Root.Element("{http://www.gribuser.ru/xml/fictionbook/2.0}description");
            foreach (XElement el in description.Elements())
            {
                Console.WriteLine(String.Format("{0}\n", el.Name));
                Console.WriteLine("  Attributes:\n");
                foreach (XAttribute attr in el.Attributes())
                    Console.WriteLine(String.Format("    {0}\n", attr));
                Console.WriteLine("  Elements:\n");
                DiveIntoElement(el);
            }
        }
    }
}
