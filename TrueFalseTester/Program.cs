using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RuleParser;

namespace TrueFalseTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Rule r = new Rule(0, "NOTHING WHEN BOOL TRUE AND (BOOL FALSE OR BOOL TRUE)");
            Rule r2 = new Rule(1, "NOTHING WHEN BOOL FALSE OR ((BOOL TRUE AND BOOL TRUE) OR BOOL FALSE)");
            Console.WriteLine(r.Execute(new TrueFalseTestProvider()));
            Console.WriteLine(r2.Execute(new TrueFalseTestProvider()));
            Console.ReadKey();
        }
    }

    class TrueFalseTestProvider : ITestProvider
    {
        bool ITestProvider.Evaluate(string command, string expression)
        {
            if (expression.Contains("TRUE"))
            {
                return true;
            }else if (expression.Contains("FALSE"))
            {
                return false;
            }
            return false;
        }

        string[] ITestProvider.GetCommands() => new[] { "BOOL" };
    }
}
