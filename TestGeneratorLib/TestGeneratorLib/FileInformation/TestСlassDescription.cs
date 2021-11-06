using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGeneratorLib.FileInformation
{
    class TestClassDescription
    {
        public List<MethodDescription> Methods { get; private set; }
        public string ClassName { get; private set; }
        public string TestedNamespace { get; private set; }

        public TestClassDescription (List<MethodDescription> methods, string className,string @namespace)
        {
            Methods = methods;
            ClassName = className;
            TestedNamespace = @namespace;
        }

    }
}
