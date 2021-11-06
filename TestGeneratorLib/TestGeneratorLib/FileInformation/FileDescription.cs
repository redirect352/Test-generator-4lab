using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGeneratorLib.FileInformation
{
    class FileDescription
    {

        public List<ClassDescription> Classes { get; private set; }

        public FileDescription(List<ClassDescription> classes)
        {
            Classes = classes;
        }
    }
}
