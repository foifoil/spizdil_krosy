namespace EveAIO.Cloudflare
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;

    public static class ChallengeSolver
    {
        private static double ApplyDecodingStep(double number, Tuple<string, double> step)
        {
            string str = step.Item1;
            double num = step.Item2;
            switch (str)
            {
                case "+":
                    return (number + num);

                case "-":
                    return (number - num);

                case "*":
                    return (number * num);
            }
            if (str != "/")
            {
                throw new ArgumentOutOfRangeException($"Unknown operator: {str}");
            }
            return (number / num);
        }

        private static double DecodeSecretNumber(string challengePageContent, string targetHost)
        {
            string str = (from m in Regex.Matches(challengePageContent, @"<script\b[^>]*>(?<Content>.*?)<\/script>", RegexOptions.Singleline).Cast<Match>() select m.Groups["Content"].Value).First<string>(c => c.Contains("jschl-answer"));
            char[] separator = new char[] { ';' };
            List<Tuple<string, double>> source = (from g in str.Split(separator).Select<string, IEnumerable<Tuple<string, double>>>(new Func<string, IEnumerable<Tuple<string, double>>>(ChallengeSolver.GetSteps))
                where g.Any<Tuple<string, double>>()
                select g).ToList<IEnumerable<Tuple<string, double>>>().Select<IEnumerable<Tuple<string, double>>, Tuple<string, double>>(new Func<IEnumerable<Tuple<string, double>>, Tuple<string, double>>(ChallengeSolver.ResolveStepGroup)).ToList<Tuple<string, double>>();
            double seed = source.First<Tuple<string, double>>().Item2;
            double num3 = Math.Round(source.Skip<Tuple<string, double>>(1).Aggregate<Tuple<string, double>, double>(seed, new Func<double, Tuple<string, double>, double>(ChallengeSolver.ApplyDecodingStep)), 10) + targetHost.Length;
            if (str.Contains("parseInt("))
            {
                return (double) ((int) num3);
            }
            return num3;
        }

        private static double DeobfuscateNumber(string obfuscatedNumber)
        {
            IEnumerable<int> values = from c in Regex.Match(obfuscatedNumber, @"\+?\(?(?<Digits>\+?\(?(\+?(\!\+\[\]|\!\!\[\]|\[\]))+\)?)+\)?").Groups["Digits"].Captures.Cast<Capture>() select Regex.Matches(c.Value, @"\!\+\[\]|\!\!\[\]").Count;
            return double.Parse(string.Join<int>(string.Empty, values));
        }

        private static IEnumerable<Tuple<string, double>> GetSteps(string text) => 
            (from s in Regex.Matches(text, @"((?<Operator>[\+|\-|\*|\/])\=?)??(?<Number>\+?\(?(?<Digits>\+?\(?(\+?(\!\+\[\]|\!\!\[\]|\[\]))+\)?)+\)?)").Cast<Match>() select Tuple.Create<string, double>(s.Groups["Operator"].Value, DeobfuscateNumber(s.Groups["Number"].Value))).ToList<Tuple<string, double>>();

        private static Tuple<string, double> ResolveStepGroup(IEnumerable<Tuple<string, double>> group)
        {
            List<Tuple<string, double>> source = group.ToList<Tuple<string, double>>();
            string str = source.First<Tuple<string, double>>().Item1;
            double seed = source.First<Tuple<string, double>>().Item2;
            double num2 = source.Skip<Tuple<string, double>>(1).Aggregate<Tuple<string, double>, double>(seed, new Func<double, Tuple<string, double>, double>(ChallengeSolver.ApplyDecodingStep));
            return Tuple.Create<string, double>(str, num2);
        }

        public static ChallengeSolution Solve(string challengePageContent, string targetHost)
        {
            double answer = DecodeSecretNumber(challengePageContent, targetHost);
            string verificationCode = Regex.Match(challengePageContent, "name=\"jschl_vc\" value=\"(?<jschl_vc>[^\"]+)").Groups["jschl_vc"].Value;
            return new ChallengeSolution(Regex.Match(challengePageContent, "id=\"challenge-form\" action=\"(?<action>[^\"]+)").Groups["action"].Value, verificationCode, Regex.Match(challengePageContent, "name=\"pass\" value=\"(?<pass>[^\"]+)").Groups["pass"].Value, answer);
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly ChallengeSolver.<>c <>9;
            public static Func<Match, string> <>9__9_0;
            public static Func<string, bool> <>9__9_1;
            public static Func<IEnumerable<Tuple<string, double>>, bool> <>9__9_2;
            public static Func<Match, Tuple<string, double>> <>9__11_0;
            public static Func<Capture, int> <>9__12_0;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new ChallengeSolver.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal string <DecodeSecretNumber>b__9_0(Match m) => 
                m.Groups["Content"].Value;

            internal bool <DecodeSecretNumber>b__9_1(string c) => 
                c.Contains("jschl-answer");

            internal bool <DecodeSecretNumber>b__9_2(IEnumerable<Tuple<string, double>> g) => 
                g.Any<Tuple<string, double>>();

            internal int <DeobfuscateNumber>b__12_0(Capture c) => 
                Regex.Matches(c.Value, @"\!\+\[\]|\!\!\[\]").Count;

            internal Tuple<string, double> <GetSteps>b__11_0(Match s) => 
                Tuple.Create<string, double>(s.Groups["Operator"].Value, ChallengeSolver.DeobfuscateNumber(s.Groups["Number"].Value));
        }
    }
}

