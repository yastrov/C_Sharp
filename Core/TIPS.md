# Tips, Notes and other

## Compare strings and links:
    var s1 = "hello";
    var s2 = "hello";
    var s3 = "hello world!".Substring(0, 5);
    object s4 = s3;
    Console.WriteLine("{0} {1} {2}", object.ReferenceEquals(s1, s2), s1 == s2, s1.Equals(s2));
    Console.WriteLine("{0} {1} {2}", object.ReferenceEquals(s1, s3), s1 == s3, s1.Equals(s3));
    Console.WriteLine("{0} {1} {2}", object.ReferenceEquals(s1, s4), s1 == s4, s1.Equals(s4));

## Code Contracts
[MSDN](https://msdn.microsoft.com/ru-ru/library/system.diagnostics.contracts.contract(v=vs.110).aspx) namespace System.Diagnostics.Contracts.Contract