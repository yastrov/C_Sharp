using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqAndTipsExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // Zip two array and make Dictionary
            var charsSet = new[] { 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p', '[', ']', 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', ';', '\'', 'z', 'x', 'c', 'v', 'b', 'n', 'm', ',', '.', '/' };
            Dictionary<char, char> charDict = charsSet
                .Zip(charsSet.Reverse(), (a, b) => new { Key = a, Val = b })
                .AsEnumerable().ToDictionary(kvp => kvp.Key, kvp => kvp.Val);
            foreach(var pair in charDict)
            {
                Console.WriteLine(pair);
            }
            Dictionary<char, int> charDict2 = charsSet
                .Zip(Enumerable.Range(0, charsSet.Count()), (a, b) => new { Key = a, Val = b })
                .AsEnumerable().ToDictionary(kvp => kvp.Key, kvp => kvp.Val);
            foreach (var pair in charDict2)
            {
                Console.WriteLine(pair);
            }
        }
    }
}
