using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleParser
{
    public interface ITestProvider
    {
        string[] GetCommands();
        bool Evaluate(string command, string expression);
    }
}
