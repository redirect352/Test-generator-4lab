using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGeneratorLib;

namespace TestGeneratorApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TestGenerator generator = new TestGenerator();
            var res = generator.GenerateTests(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGeneratorLib;

namespace TestGeneratorApp
{
    class Program
    {
        
static void Main(string[] args)
        {}
        public void PrintStr(string str)
        {



        }
    }
}");

            foreach (string key in res.Keys)
            {
                Console.WriteLine(key);
                Console.WriteLine(res[key]);

            }

            Console.ReadLine();
        }



        public void PrintStr(string str)
        {



        }
    }
}
