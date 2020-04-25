using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        public static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
            }
        }

        [TestCase("text", new[] { "text" })]
        [TestCase("hello world", new[] { "hello", "world" })]
        [TestCase("'a'", new[] { "a" })]
        [TestCase("hello  world", new[] { "hello", "world" })]
        [TestCase(@"'' a", new[] { "", "a" })]
        [TestCase(@"'a""cd""'", new[] { @"a""cd""" })]
        [TestCase(@"""a'cd'""", new[] { @"a'cd'" })]
        [TestCase(@"'asd'r", new[] { @"asd", @"r" })]
        [TestCase(@"'def g h'", new[] { @"def g h" })]
        [TestCase(@"a'bc", new[] { @"a", @"bc" })]
        [TestCase(@"'\\'", new[] { @"\" })]
        [TestCase(@"  a  ", new[] { "a" })]
        [TestCase(@"", new string[0])]
        [TestCase(@"""a ", new[] { @"a " })]
        [TestCase(@"'\''", new[] { @"'" })]
        [TestCase(@"""\""""", new[] { @"""" })]
        public static void RunTests(string input, string[] expectedOutput)
        {
            Test(input, expectedOutput);
        }
    }

    public class FieldsParserTask
    {        
        public static List<Token> ParseLine(string line)
        {
            List<Token> fieldsList = new List<Token>();
            int currentIndex = 0;
            while (currentIndex < line.Length)
            {
                if (line[currentIndex]==' ')
                {
                    currentIndex = SkipSpacesBetweenField(line, currentIndex);
                }
                else
                {
                    Token tmp;
                    if (line[currentIndex]=='\'' || line[currentIndex] == '\"')
                    {
                        tmp = ReadQuotedField(line, currentIndex);
                    }
                    else
                    {
                        tmp = ReadField(line, currentIndex);
                    }
                    currentIndex = tmp.GetIndexNextToToken();
                    fieldsList.Add(tmp);
                }
            }
            return fieldsList;
        }
        
        private static Token ReadField(string line, int startIndex)
        {
            int currentIndex = startIndex;
            StringBuilder str = new StringBuilder();
            while(currentIndex<line.Length && line[currentIndex]!=' ' && 
                line[currentIndex]!='\'' && line[currentIndex] != '\"')
            {
                str.Append(line[currentIndex]);
                currentIndex++;
            }
            return new Token(str.ToString(), startIndex, str.Length);
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            return QuotedFieldTask.ReadQuotedField(line, startIndex);
        }

        public static int SkipSpacesBetweenField(string line, int startIndex)
        {
            int currentIndex = startIndex;
            while(currentIndex<line.Length && line[currentIndex]==' ')
            {
                currentIndex++;
            }
            return currentIndex;
        }
    }
}