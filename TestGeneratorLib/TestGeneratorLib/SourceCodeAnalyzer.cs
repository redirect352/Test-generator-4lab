using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TestGeneratorLib.FileInformation;



namespace TestGeneratorLib
{
    class SourceCodeAnalyzer
    {
        public FileDescription GetDescription(string fileContent)
        {
            CompilationUnitSyntax root = CSharpSyntaxTree.ParseText(fileContent).GetCompilationUnitRoot();
            var classes = new List<ClassDescription>();
            foreach (ClassDeclarationSyntax classDeclaration in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                classes.Add(GetClassDescription(classDeclaration));
            }

            return new FileDescription(classes);
        }

        private ClassDescription GetClassDescription(ClassDeclarationSyntax classDeclaration)
        {
            var methods = new List<MethodDescription>();
            foreach (var method in classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>())
            {

                if (method.Modifiers.Any((modifier) => modifier.IsKind(SyntaxKind.PublicKeyword)))
                {
                    methods.Add(GetMethodDescription(method));
                }
               
            }
            return new ClassDescription(methods, classDeclaration.Identifier.ValueText + "Test");

        }

        private static MethodDescription GetMethodDescription(MethodDeclarationSyntax method)
        {
            var parameters = new Dictionary<string, string>();
            foreach (var parameter in method.ParameterList.Parameters)
            {
                parameters.Add(parameter.Identifier.Text, parameter.Type.ToString());
            }

            return new MethodDescription(parameters, method.Identifier.ValueText, method.ReturnType.ToString());
        }



    }
}
