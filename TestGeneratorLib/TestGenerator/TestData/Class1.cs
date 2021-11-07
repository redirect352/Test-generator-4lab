using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGeneratorApp.TestData
{
    class Class1
    {
        public Class1(int a, int b)
        {


        }
        public Class1(int a, int b,int c, ICollection<int> vs)
        {


        }

        public int MyFirstMethod( int b, ICollection<int> vs)
        {

            return b * b;
        }

        public void MySecondMethod(int a, int b, int c)
        {
            a = a + b;
        }

        public double MythirdMethod(int a, int b)
        {
            a++;
            return (double)a / b-b;
        }


    }
}
