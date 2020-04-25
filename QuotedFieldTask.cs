using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
        }
    }

    class QuotedFieldTask
    {
        public static Token ReadQuotedField(string line, int startIndex)
        {
            StringBuilder tokenValue = new StringBuilder();
            char stopCh = line[startIndex];
            int i = startIndex + 1;
            int countQuotes = 1;
            while (i < line.Length)
            {
                if (line[i] == stopCh)
                {
                    countQuotes++;
                    break;
                }
                if (line[i] == '\\')
                {
                    if (i+1 == line.Length)
                    {
                        tokenValue.Append(line[i]);
                        break;
                    }
                    else if (line[i + 1] == '\'' || line[i+1]=='\"' || line[i + 1] == '\\')
                    {
                        tokenValue.Append(line[i + 1]);
                        countQuotes++;
                        i += 2;
                    }
                    else
                    {
                        tokenValue.Append(line[i]);
                        i++;
                    }
                }
                else
                {
                    tokenValue.Append(line[i]);
                    i++;
                }
            }
            return new Token(tokenValue.ToString(), startIndex, tokenValue.Length + countQuotes);
        }
    }
}
