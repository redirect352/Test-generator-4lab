using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGeneratorLib.FileInformation
{
    class ClassDescription
    {
        public List<MethodDescription> Methods { get; private set; }
        public string ClassName { get; private set; }
       

        public ClassDescription (List<MethodDescription> methods, string className)
        {
            this.Methods = methods;
            this.ClassName = className;
        }

    }
}
