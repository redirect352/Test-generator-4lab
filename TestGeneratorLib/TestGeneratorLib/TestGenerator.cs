using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;


namespace TestGeneratorLib
{
    public class TestGenerator
    {
        private string outputPath = "";
        private List<string> classFiles = new List<string>();

        SyntaxTree Tree = CSharpSyntaxTree.ParseText("wdwdwd");
    }
}
