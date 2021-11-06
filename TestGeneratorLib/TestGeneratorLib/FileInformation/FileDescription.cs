using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGeneratorLib.FileInformation
{
    class FileDescription
    {

        public List<TestClassDescription> Classes { get; private set; }

        public FileDescription(List<TestClassDescription> classes)
        {
            Classes = classes;
        }
    }
}
