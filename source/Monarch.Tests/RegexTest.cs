using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace Monarch.Tests
{
    [TestClass]
    public class RegexTest
    {
        [TestMethod]
        public void Regex_Concatenation_Test()
        {
            // matches one character at a time from left to right
            Match("abc", "abc");
            Match("abc", "123abc123");
            Match("abc", "123abc");
            Match("abc", "123");

            //left most sequence
            Match("abc", "abc123abc");
        }

        [TestMethod]
        public void Regex_Alternation_Test()
        {
            //alternatives are check from left to right; the 1st alternatives wich succeeds stops further checking
            Match("abc|123", "abc");
            Match("abc|123", "123");

            Match("abc|123", "abc123");
            Match("123|abc", "abc123"); //abc is the first match even 123 is the first alternative

            Match("abc|abc123", "abc123"); //abc is the only match
            Match("abc123|abc", "abc123"); //abc123 is the only match
        }

        [TestMethod]
        public void Regex_Grouping_Test()
        {
            //these 2 are equivalent
            Match("gray|grey", "gray and grey");
            Match("gr(a|e)y", "gray and grey"); 
        }

        [TestMethod]
        public void Regex_Quantification_Test()
        {
            //alternates
            Match("abcabcabc|abcabc|abc", "abc, abcabc, abcabcabc"); // match from 1 to 3 repetitions (must keep reverse order)
            Match("(abc){1,3}", "abc, abcabc, abcabcabc"); // match from 1 to 3 repetitions

            //one up to 4 times repetition of abc
            Match("(abc){1,4}", "abc");
            Match("(abc){1,4}", "abc, abcabc");
            Match("(abc){1,4}", "abc, abcabc, abcabcabc");
            Match("(abc){1,4}", "abc, abcabc, abcabcabc, abcabcabcabc");
            Match("(abc){1,4}", "abc, abcabc, abcabcabc, abcabcabcabc, abcabcabcabcabc");

            //upper limit default is the same as the lower limit
            Match("(abc){2,2}", "abc, abcabc, abcabcabc");
            Match("(abc){2}", "abc, abcabc, abcabcabc");
        }

        [TestMethod]
        public void Regex_Quantification_QuestionMark_Test()
        {
            //? is equivalent to {0,1}
            Match("(abc)?", "abc123"); //1st match succesful of 5 matches
            Match("(abc)?", "123abc");  //4th match succesful of 5 matches
        }

        [TestMethod]
        public void Regex_Quantification_KleenePlus_Test()
        {
            //avoids empty matches produced by ?
            Match("(abc)+", "123abc"); //short cut sintax of (abc)(abc)?
            Match("(abc)+", "abc123"); //short cut sintax of (abc)(abc)?
        }


        [TestMethod]
        public void Regex_Quantification_KleeneStar_Test()
        {
            Match("a*", "a");
            Match("a*", "aaaaa"); //match length of 5 (greedy, match the max number of chars)
            Match("1*", "aaaaa"); //0 repetitions of a pattern is consider succesful (empty match)

            Match("abc*", "abc"); // 1 repeat, length of 3
            Match("abc*", "abcabc"); //1 repeat, length of 3
            Match("abc*", "abc123"); //1 repeat, length of 3

            Match("(abc)*", "abc"); // 1 repeat, length of 3
            Match("(abc)*", "abcabc"); //2 repeats, length of 6
            Match("(abc)*", "abc123"); //1 repeat, length of 3
        }

       



        private const string matchInputFormat = "Match('{0}', '{1}')";
        private const string matchResultFormat = "\t{0} @{1}:{2}";

        private void Match(string pattern, string subject)
        {
            Console.WriteLine(matchInputFormat, pattern, subject);

            var regex = new Regex(pattern);
            var match = regex.Match(subject);

            while (match.Success)
            {
                Console.WriteLine(matchResultFormat, match.Success, match.Index, match.Length);
                match = match.NextMatch();
            }
            Console.WriteLine("\t" + match.Success);

            Console.WriteLine("========================================");


        }


    }
}
