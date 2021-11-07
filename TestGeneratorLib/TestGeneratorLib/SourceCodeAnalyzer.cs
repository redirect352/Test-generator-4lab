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

            var classes = new List<TestClassDescription>();
            foreach (NamespaceDeclarationSyntax namespaceDeclaration in root.DescendantNodes().OfType<NamespaceDeclarationSyntax>())
            {
                foreach (ClassDeclarationSyntax classDeclaration in namespaceDeclaration.DescendantNodes().OfType<ClassDeclarationSyntax>())
                {
                    classes.Add(GetClassDescription(classDeclaration,namespaceDeclaration));
                }
            }
            return new FileDescription(classes);
        }

        private TestClassDescription GetClassDescription(ClassDeclarationSyntax classDeclaration, NamespaceDeclarationSyntax namespaceDeclaration)
        {
            string name = namespaceDeclaration.Name.ToString();
            var methods = new List<MethodDescription>();
            foreach (var method in classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>())
            {

                if (method.Modifiers.Any((modifier) => modifier.IsKind(SyntaxKind.PublicKeyword)))
                {
                    methods.Add(GetMethodDescription(method));
                }
               
            }
            foreach (var constructor in classDeclaration.DescendantNodes().OfType<ConstructorDeclarationSyntax>())
            {
                if (constructor.Modifiers.Any((modifier) => modifier.IsKind(SyntaxKind.PublicKeyword)))
                {
                    methods.Add(GetMethodDescription(constructor, classDeclaration.Identifier.ValueText));
                }


            }


           
            return new TestClassDescription(methods, classDeclaration.Identifier.ValueText + "Test", name);

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

        private static MethodDescription GetMethodDescription(ConstructorDeclarationSyntax constructor, string returnType)
        {

            var parameters = new Dictionary<string, string>();
            foreach (var parameter in constructor.ParameterList.Parameters)
            {
                parameters.Add(parameter.Identifier.Text, parameter.Type.ToString());
            }

            var res = new MethodDescription(parameters, constructor.Identifier.ValueText, returnType);
            res.IsConstructor = true;
            return res;
        }


    }
}
