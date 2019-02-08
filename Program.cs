using System;
using System.Collections.Generic;
using System.Linq;

namespace PatternMatchingCSharp
{
    public static class Program
    {
        static void Main(string[] args)
        {
            (var x, var y) = "deconstructed".Deconstruct();

            char[] heads =
            {
                "".Head(),
                "head".Head(),
                x
            };

            string[] tails =
            {
                "".Tail(),
                "tail".Tail(),
                "thisIsSomeCamelCasedText".InsertSpacesRecursive(),
                "UsingDeconstructToRecursivelyInsertSpaces".InsertSpacesRecursive2(),
                y
            };

            tails.Append(y);

            Console.WriteLine("writing heads:");
            heads.ToList()
                .ForEach(ch => Console.WriteLine(ch));

            Console.WriteLine("writing tails:");
            tails.ToList()
                .ForEach(ch => Console.WriteLine(ch));
        }

        private static string InsertSpacesRecursive(this string str) => str switch
        {
            "" => "",
            var x when char.IsUpper(x.Tail().Head()) => x.Head() + " " + str.Tail().InsertSpacesRecursive(),
            var x => x.Head() + str.Tail().InsertSpacesRecursive()
        };

        private static string InsertSpacesRecursive2(this string str) => str.Deconstruct() switch
        {
            (char.MinValue, _) => "",
            (char h, string t) when char.IsUpper(t.Head()) => h + " " + t.InsertSpacesRecursive2(),
            (char h, string t) => h + t.InsertSpacesRecursive2()
        };

        private static char Head(this string str) => str switch
        {
            ""      => char.MinValue,
            var x   => x.First()
        };

        private static string Tail(this string str) => str switch
        {
            ""      => "",
            var x   => x.Remove(0, 1)
        };

        public static (char, string) Deconstruct(this string str) => (str.Head(), str.Tail());

        private static string InsertSpacesAgg(this string str) => str
            .Aggregate(string.Empty, (a, b) =>
                char.IsUpper(b)
                    ? $"{a} {b}"
                    : $"{a}{b}"
            ).TrimStart();

        private enum AlertFields
        {
            None = 0,
            Alert,
            Resource,
            ConditionDetails,
            Link
        }

        private static string ExtractMessagePart(string key, dynamic classicInsight) => key switch
        {
            nameof(AlertFields.Alert) => $"{classicInsight.Name} - {classicInsight.Description}",
            //nameof(AlertFields.Resource)            => $"{classicInsight.ResourceGroupName}, {classicInsight.ResourceName }",
            nameof(AlertFields.ConditionDetails) => "",//ConditionText(classicInsight.Condition),
            nameof(AlertFields.Link) => "",//GetAzureResourceLink(data.ResourceId),
            _ => $"You need to add this key: to the pattern matching switch."
        };
    }
}
