using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGeneratorApp.TestData
{
    class Class1
    {

        public int MyFirstMethod(int a, int b)
        {

            return a * b;
        }

        public void MySecondMethod(int a, int b)
        {
            a = a + b;
        }

        public double MythirdMethod(int a, int b)
        {
            a++;
            return (double)a / b;
        }


    }
}
