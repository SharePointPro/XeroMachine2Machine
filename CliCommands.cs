using System;
using System.Collections.Generic;
using System.Text;

namespace XeroMachine2Machine
{
    class CliCommands
    { 
        public const string GET_TENANTS = "GetTenants";

        public const string INIT = "init";

        public static List<string> CLI_COMMANDS = new List<string>() { GET_TENANTS, INIT };
    }
}
