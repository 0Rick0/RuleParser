using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RuleParser
{
    class RuleSection
    {
        private static Regex splitRuleRegex;
        private static Regex subRuleRegex;

        private List<RuleSection> subRules = new List<RuleSection>();
        private List<string> preSplitRules;

        private string totalRule;

        static RuleSection()
        {
            splitRuleRegex = new Regex(@"(?<=AND|OR)");
            subRuleRegex = new Regex(@"#SUBRULE(\d+)#");
        }

        public RuleSection(string rulePart)
        {
            Console.WriteLine($"Parsing rule '{rulePart}'");
            totalRule = rulePart;
            //step one create sub sections
            while (totalRule.IndexOf('(') >= 0)
            {
                var startIndex = totalRule.IndexOf('(');
                var dept = 1;
                var endIndex = startIndex;
                while (dept > 0)
                {
                    var possibleEnd = totalRule.IndexOf(')',endIndex+1);//search for next closing tag
                    var nextIn = totalRule.IndexOf('(', endIndex + 1);//search for next opening tag
                    if(nextIn > 0 && nextIn < possibleEnd)//if there is an opening tag and it is before the end then increase dept and set endIndex
                    {
                        endIndex = nextIn;
                        dept++;
                    }else{//else send endIndex and decrease dept
                        endIndex = possibleEnd;
                        dept--;
                    }
                }
                subRules.Add(new RuleSection(totalRule.Substring(startIndex + 1, endIndex - startIndex - 1)));
                totalRule = totalRule.Remove(startIndex, endIndex - startIndex + 1).Insert(startIndex, $"#SUBRULE{subRules.Count-1}#");//subsitute the subrule with #SUBRULEid#
                
            }

            preSplitRules = new List<string>(splitRuleRegex.Split(totalRule));
            
        }

        public bool Evaluate(ITestProvider testProvider)
        {
            bool resUntilNow = false;
            var nextCommand = "OR";
            foreach(var ppart in preSplitRules)
            {
                var part = ppart.Trim();
                bool nextResult = false;
                if (subRuleRegex.IsMatch(part))
                {
                    int subId;
                    if(!int.TryParse(subRuleRegex.Match(part).Groups[1].Value, out subId))
                    {
                        throw new ArgumentException("Rule group does not exist!");
                    }
                    if (subId >= subRules.Count) {
                        throw new ArgumentException("Rule id does not exist!");
                    }
                    nextResult = subRules[subId].Evaluate(testProvider);
                    
                }else if (testProvider.GetCommands().Any(command=>part.StartsWith(command))){
                    var command =  testProvider.GetCommands().Where(com => part.StartsWith(com)).First();
                    nextResult = testProvider.Evaluate(command, part.Substring(command.Length, part.Length - command.Length).Split(new[] { "AND","OR" },StringSplitOptions.None)[0].Trim());//get the expresson without the command and the following operator
                }else
                {
                    throw new ArgumentException("Command is not know for rule part " + part);
                }

                if(nextCommand == "OR")
                {
                    resUntilNow = resUntilNow || nextResult;
                }else if(nextCommand == "AND")
                {
                    resUntilNow = resUntilNow && nextResult;
                }
                else
                {
                    throw new ArgumentException("Last rule command was not vallid! " + nextCommand);
                }

                nextCommand = part.Substring(part.Length - 3).Trim(); //get AND or OR
                 
            }
            return resUntilNow;
        }
    }
}
