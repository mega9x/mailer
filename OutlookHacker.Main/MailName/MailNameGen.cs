using System.Text;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChanceNET;
using Syllabore;

namespace OutlookHacker.Main.MailName
{
    public class MailNameGen
    {
        private StringBuilder _stringBuilder = new();
        private NameGenerator GenOne = new NameGenerator()
            .UsingProvider(p => p
                .WithVowels("aeiouy")
                .WithLeadingConsonants("vstlr")
                .WithTrailingConsonants("zrt")
                .WithVowelSequences("ey", "ay", "oy"))
            .UsingTransformer(m => m
                .Select(1).Chance(0.99)
                .WithTransform(x => x.ReplaceSyllable(0, "Gran"))
                .WithTransform(x => x.ReplaceSyllable(0, "Bri"))
                .WithTransform(x => x.InsertSyllable(0, "Deu").AppendSyllable("gard")).Weight(2)
                .WithTransform(x => x.When(-2, "[aeoyAEOY]$").ReplaceSyllable(-1, "opolis"))
                .WithTransform(x => x.When(-2, "[^aeoyAEOY]$").ReplaceSyllable(-1, "polis")))
            .UsingFilter(v => v
                .DoNotAllow("yv", "yt", "zs")
                .DoNotAllowPattern(
                    @".{12,}",
                    @"(\w)\1\1",              // Prevents any letter from occuring three times in a row
                    @".*([y|Y]).*([y|Y]).*",  // Prevents double y
                    @".*([z|Z]).*([z|Z]).*")) // Prevents double z
            .UsingSyllableCount(2, 4);
        private List<Action> actions = new();
        public MailNameGen()
        {
            actions = new()
            {
                () => {
                    Append("_");
                },
                () => {
                    var chance = new Chance();
                    var p = chance.Person();
                    Append(p.FirstName);
                },
                () => {
                    var chance = new Chance();
                    var p = chance.Person();
                    Append(p.LastName);
                },
                () => {
                    var chance = new Chance();
                    Append(chance.Age(AgeRanges.Adult));
                },
                () => {
                    var chance = new Chance();
                    Append(chance.Age(AgeRanges.Adult));
                },
                () => {
                    var chance = new Chance();
                    Append(chance.Birthday(AgeRanges.Adult).Year);
                },
                () => {
                    Append(GenOne.Next());
                },
                () => {
                    var chance = new Chance();
                    Append(chance.Animal());
                },
            };
        }
        public string GetRandomName()
        {
            _stringBuilder.Clear();
            var index = RandomNumberGenerator.GetInt32(3, 6);
            for (var i = 0; i <= index; i++)
            {
                actions[RandomNumberGenerator.GetInt32(0, actions.Count)]();
            }
            return _stringBuilder.ToString();
        }
        public string GetPassword()
        {
            var chance = new Chance();
            return chance.String(10);
        }
        public string GetFirstName()
        {
            var chance = new Chance();
            return chance.FirstName();
        }
        public string GetLastName()
        {
            var chance = new Chance();
            return chance.LastName();
        }
        private MailNameGen Append<T>(T str)
        {
            _stringBuilder.Append(str);
            return this;
        }
    }
}
