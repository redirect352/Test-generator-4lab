using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGeneratorLib.FileInformation
{
    class MethodDescription
    {
        public string Name { get; private set; }
        public string ReturnType { get; private set; }
        public Dictionary<string, string> Parameters { get; private set; }
        public bool IsConstructor { get; set; } = false;

        public MethodDescription(Dictionary<string, string> parameters, string name, string returnType)
        {
            ReturnType = returnType;
            Name = name;
            Parameters = parameters;
        }
    }
}
