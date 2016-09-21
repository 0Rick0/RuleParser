using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleParser
{
    public class Rule
    {
        public int id { get; private set; }
        private string stringRule;
        private RuleSection rootSection;

        public string StringRule
        {
            get { return stringRule; }
            set { throw new NotImplementedException(); stringRule = value; }
        }

        public Rule(int id, string rule)
        {
            this.id = id;
            this.stringRule = rule;
            Parse();
        }

        public bool Execute(ITestProvider testProvider)
        {
            bool result = rootSection.Evaluate(testProvider);
            return result;
        }

        private void Parse()
        {
            var parts = stringRule.Split(new[]{ " WHEN "}, 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                throw new ArgumentException("Rule does not have WHEN keyword or one side is empty!");
            }

            ParseAction(parts[0]);
            rootSection = new RuleSection(parts[1]);
        }

        private void ParseAction(string action)
        {

        }

    }
}
