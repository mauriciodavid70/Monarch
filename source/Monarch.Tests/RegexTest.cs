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

            Match("abc", "abc", "matches one character at a time from left to right");
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
        public void Regex_Groups_Intro_Test()
        {
            Match("gr(a|e)y", "gray grey");
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
            Match("(abc)?", "abc123", "1st match succesful of 5 total matches");
            Match("(abc)?", "123abc", "4th match succesful of 5 total matches");
        }

        [TestMethod]
        public void Regex_Quantification_KleenePlus_Test()
        {
            //avoids empty matches produced by ?
            Match("(abc)+", "123abc", "short cut sintax of (abc)(abc)?");
            Match("(abc)+", "abc123", "short cut sintax of (abc)(abc)?");
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

        [TestMethod]
        public void Regex_Anchor_BeginningOfString_Test()
        {
            //\A matches only at the beginning of the string.
            Match(@"\Aabc", "abc");
            //\A does not match the pattern at the beginning of a line
            Match(@"\Aabc", "\nabc");

            //^ same as /A
            Match("^abc", "abc");
            Match("^abc", "\nabc");
        }

        [TestMethod]
        public void Regex_Anchor_EndOfString_Test()
        {
            //\Z matches the subject only at the end of the string
            Match(@"abc\Z", "abc");
            //\Z matches the subject even with \n at the end of the string
            Match(@"abc\Z", "abc\n");
            // \Z does not match \n not at the end of the string
            Match(@"abc\Z", "abc\n123");

            //$ same as /Z
            Match("abc$", "abc");
            Match("abc$", "abc\n");
            Match("abc$", "abc\n123");

            //\z matches at only at the end of the string (strict)
            Match(@"abc\z", "abc\n");
        }

        [TestMethod]
        public void Regex_MultilineModifier_Test()
        {
            //(?m) multiline modifier, only applies to ^ and $, does NOT apply to \A \Z \z'

            //(?m)^ matches the pattern at the beginning of string or line'
            Match("(?m)^abc", "abc first line,\nabc second line");

            //(?m) applies to all ^ alternatives in the pattern'
            Match("(?m)^abc|^123", "abc first line,\nabc second line\n123 third line");

            //(?m:^) restricts the scope of ^'
            Match("(?m:^abc)|^123", "abc first line,\nabc second line\n123 third line");

            //(?m)$ matches the pattern at the end of the string or line
            Match("(?m)abc$", "first line abc\nsecond line abc");

            //(?m) applies to all $ alternatives in the pattern'
            Match("(?m)abc$|123$", "first line abc\nsecond line 123\nthird line abc");

            //(?m:$) restricts the scope of $'
            Match("(?m:abc$)|123$", "first line abc\nsecond line 123\nthird line abc");
        }

        [TestMethod]
        public void Regex_CharacterClasses_Test()
        {
            //the pattern with alternatives (single chars)
            Match("a|b|c", "123c"); //9 backtracks, the number of alternatives times the number of failed chars
            //is equivalent to the set of chars 
            Match("[abc]", "123c"); //3 backtracks, one per failed char

            //range of chars
            Match("[a-z]", "abc123abc");
            //multiple ranges
            Match("[a-z0-9]", "abc123abc");
            //range negation
            Match("[^a-z]", "abc123abc");

            //numeric ranges 
            Match(@"[\d]", "abc123abc"); //[0-9]
            Match(@"[\D]", "abc123abc"); //[^0-9]

            //spaces
            Match(@"\s", "abc 123 abc"); //space
            Match(@"\S", "abc 123 abc"); //^space

            //word chars
            Match(@"\w", "abc 123 áéíóúñ !@#$?", "unicode letters, not simbols");
            Match(@"\W", "abc 123 áéíóúñ !@#$?", "not unicode letters");
        }


        [TestMethod]
        public void Regex_CharacterScaping_Test()
        {
            Match("?", "abc?");
            Match(@"\?", "abc?");
            Match(Regex.Escape("?"), "abc?");
        }

        [TestMethod]
        public void Regex_WordBoundaries_Test()
        {
            //boundaries between word \w chars and non-word \W chars

            //\b produces empty matches between words, includes position 0 and any special char
            Match(@"\b", "The quick brown fox jumps over the lazy dog.");

            //\B is the complement of \b
            Match(@"\B", "The quick brown fox jumps over the lazy dog.");
        }

        [TestMethod]
        public void Regex_WildCharacter_Test()
        {
            // . matches any char but \n unless (?s) 
            Match(".", "abc", "matches each char in the subject");
            Match("c.t", "cat cut abc", "matches cat and cut");

            Match("...", "abcdefghij", "matches every 3 char in the subject, omits the last two");
            Match(".{3}", "abcdefghij", "equivalent to ...");
            Match(@"\b.{3}\b", "abc def ghij", "combined with \\b anchor does not match ghij because it has too many chars");
            Match(@"\b.{3}\b", "abc def gh", "combined with \\b anchor does match [space]gh  (three chars including the trailing space from the boundary");
            Match(@"\b\w{3}\b", "abc def gh", " switching the . with \\w combined with the \\b anchor does only match word chars with length of three");

            Match("c.t|gr.y", "cat cut c\nt gray grey gr\ny", "matches c.t and gr.y (omits \\n matches)");
            Match("(?s)c.t|gr.y", "cat cut c\nt gray grey gr\ny", "matches c.t and gr.y (with \\n matches)");
            Match("(?s:c.t)|gr.y", "cat cut c\nt gray grey gr\ny", "scopes only to c.t alternative");

            //performance..
            Match(".*z", "zaaaaa", "matches at 0 length 1 many backtracking");
            Match("[^z\\n]*z", "zaaaaa", "negates z and \\n at the beggining, matches at 0 length 1, with one backtrack");
        }

        [TestMethod]
        public void Regex_Groups_Test()
        {
            Match("(\\w+) is friends with (\\w+)", "Sam is friends with home", "Three groups defined");

            Match(@"(Jan|Feb|Mar|Apr|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\s(\d{1,2})[\s-](\d{2,4})\s+((?#user)\w+@(?#domain)\w+.\w+)",
                    "Mar 5 2010    csanchez@hotmail.com\nApr 04 2012 ctoro@gmail.com",
                    "Five Groups defined");

            Match(@"(?x)
                (Jan|Feb|Mar|Apr|Jun|Jul|Aug|Sep|Oct|Nov|Dec)(?-x: )
                (\d{1,2})[\s-]
                (\d{2,4})\s+
                (\w+@\w+.\w+)",
               "Mar 5 2010    csanchez@hotmail.com\nApr 04 2012 ctoro@gmail.com",
               "(?x)Ignore Whitespace in the pattern (?-x) to revert");

            Match(@"(?x)
                #Month
                (Jan|Feb|Mar|Apr|Jun|Jul|Aug|Sep|Oct|Nov|Dec)(?-x: )
                #Day
                (\d{1,2})[\s-]
                #Year
                (\d{2,4})\s+
                #Email
                ((?#user)\w+@(?#domain)\w+.\w+)",
                "Mar 5 2010    csanchez@hotmail.com\nApr 04 2012 ctoro@gmail.com",
                "# for comments and(?#) for Inline comments");

            Match(@"(?x)
                (?<month>Jan|Feb|Mar|Apr|Jun|Jul|Aug|Sep|Oct|Nov|Dec)(?-x: )
                (?<day>\d{1,2})[\s-]
                (?<year>\d{2,4})\s+
                (?<email>\w+@\w+.\w+)",
                "Mar 5 2010    csanchez@hotmail.com\nApr 04 2012 ctoro@gmail.com",
                "?<> for group naming");

            Match(@"(?x)
                (?<month>Jan|Feb|Mar|Apr|Jun|Jul|Aug|Sep|Oct|Nov|Dec)(?-x: )
                (?<day>\d{1,2})[\s-]
                (?<year>\d{2,4})\s+
                (\w+@\w+.\w+)",
              "Mar 5 2010    csanchez@hotmail.com\nApr 04 2012 ctoro@gmail.com",
              "email group not named, becomes the first group");
        }

        [TestMethod]
        public void Regex_Groups_Capture_Test()
        {
            //one group might capture more than one part of the subject
            Match("(...)(...)(...)", "abcdefghi", "Three subgroups with one capture each");

            Match("(...){3}", "abcdefghi", "One subgroup with three captures, the group value is the last of the capture array");

            Match("(dog)(cat)(...){3}", "dogcatabcdefghi", "G0 is root, G1 is (dog), G2 is (cat), G3 is(...){3}");

            Match("dog((cat)(...){3})", "dogcatabcdefghi", "G0 is root, G1 is ((cat)(...){3}), G2 is (cat), G3 is (...){3}");
        }

        [TestMethod]
        public void Regex_Groups_Backreference_Test()
        {
            //use the identity of the group preceeded by backslash 
            //to backreference the group value 

            Match(@"(\d{1,2})[-/\s](\d{1,2})[-/\s](\d{2,4})", "5-8-70\n5/8/70\n5-8/70", "the last format is mixing minus and slash, KO");

            Match(@"(\d{1,2})([-/\s])(\d{1,2})\2(\d{2,4})", "5-8-70\n5/8/70\n5-8/70", "backreference to G2 is \\2, omits the last format, OK");

            Match(@"(\d{1,2})(?<datesep>[-/\s])(\d{1,2})\k<datesep>(\d{2,4})", "5-8-70\n5/8/70", "backreference by name with \\k");

            Match(@"([:-])+(\w+)\1+$", ":-:-:123:", "G0 is root, G1 value is : (last of [:-:-:]) so \\1 equals to :+$");

            Match(@"([:-])+(\w+)(\1\1)", ":123::", @"(\1\1) tells to match G1 an even number of times. (G3)");

            Match(@"([:-])+(\w+)(?:\1\1)", ":123::", @"(?:\1\1) non capturing group ");

            Match(@"^(?<word>\w+)$|^(?<word>\w+)([:;,]\s+)((?<word>\w+)\1)*(?<word>\w+)$", "cat, dog, fish, bird");

        }


        private const string matchInputFormat = "Match('{0}', '{1}'); // {2}";
        private const string matchResultFormat = "\t{0} @{1}:{2}";
        private const string groupFormat = "\t  Group[{0}]: {1}";
        private const string captureFormat = "\t    Capture: {0}";

        private void Match(string pattern, string subject, string message = null)
        {
            Console.WriteLine(matchInputFormat, pattern, subject, message);
            try
            {
                var regex = new Regex(pattern);
                var match = regex.Match(subject);

                while (match.Success)
                {
                    Console.WriteLine(matchResultFormat, match.Success, match.Index, match.Length);

                    for (var index = 0; index < match.Groups.Count; index++ )
                    {
                        var group = match.Groups[index];
                        Console.WriteLine(groupFormat, index, group.Value);

                        foreach (Capture capture in group.Captures)
                        {
                            Console.WriteLine(captureFormat, capture.Value);
                        }
                    }
                        
                    match = match.NextMatch();
                }
                Console.WriteLine("\t" + match.Success);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\tException: " + ex.Message);
            }
            Console.WriteLine("========================================");
        }


    }
}
