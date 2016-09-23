using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RuleParser;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;
using System.Diagnostics;

namespace TrueFalseTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing rules with simple Booleans and advanced subrules");
            Rule r = new Rule(0, "NOTHING WHEN BOOL TRUE AND (BOOL FALSE OR BOOL TRUE)");
            Rule r2 = new Rule(1, "NOTHING WHEN BOOL FALSE OR ((BOOL TRUE AND BOOL TRUE) OR BOOL FALSE)");
            Console.WriteLine(r.Execute(new TrueFalseTestProvider()));
            Console.WriteLine(r2.Execute(new TrueFalseTestProvider()));

            Console.WriteLine("Testing rules with no subrules and advanced content");
            Server server = new Server() { Servername = "TestServer" };
            server.SoftwareVersions = new Dictionary<string, Version> { { "test", new Version(1, 0)} };
            server.PinkSoftwareVersions = new Dictionary<string, Version> { { "test2", new Version(1, 0)} };
            var provider = new ServerTester(server);
            Rule r3 = new Rule(3, @"NOTHING WHEN INSTALLED Server.getSoftVersion(""test"").Major == 1");
            Console.WriteLine(r3.Execute(provider));
            Console.ReadKey();
        }
    }

    public class Server
    {
        public string Servername { get; set; }
        public Dictionary<string,Version> SoftwareVersions { get; set; }
        public Dictionary<string,Version> PinkSoftwareVersions { get; set; }

        public Version getSoftVersion(string name)
        {
            Version v = null;
            SoftwareVersions.TryGetValue(name, out v);
            return v;
        }
        public Version getPinkSoftVersion(string name)
        {
            Version v = null;
            PinkSoftwareVersions.TryGetValue(name, out v);
            return v;
        }
    }

    class ServerTester : ITestProvider
    {
        public Server server { get; private set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        private V8ScriptEngine engine;

        public ServerTester(Server server)
        {
            this.server = server;
            engine = new V8ScriptEngine();
            engine.AddHostObject("Server", server);
            

        }

        ~ServerTester()
        {
            engine.Dispose();
            engine = null;
        }
        
        public bool Evaluate(string command, string expression)
        {
            if(command.Trim() == "INSTALLED")
            {
                var result = engine.Evaluate(expression);
                return result.GetType() == typeof(bool) ? (bool)result : false;
            }
            return false;
        }

        public string[] GetCommands() => new[] { "INSTALLED" };
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
