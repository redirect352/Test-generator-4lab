using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TestGeneratorLib.FileInformation;

namespace TestGeneratorLib
{
    public class TestGenerator
    {
        private static readonly  SourceCodeAnalyzer codeAnalyzer = new SourceCodeAnalyzer();

        private static readonly AttributeSyntax TestClassAtribute = SyntaxFactory.Attribute(SyntaxFactory.ParseName("TestFixture"));
        private static readonly AttributeSyntax TestMethodAttribute = SyntaxFactory.Attribute(SyntaxFactory.ParseName("Test"));


        public Dictionary<string, string> GenerateTests(string fileContent)
        {
            var result = new Dictionary<string, string>();
            FileDescription fileDescription = codeAnalyzer.GetDescription(fileContent);
            foreach (var classDescription in fileDescription.Classes)
            {
                var namespaceDeclaration = GenerateNamespaceDeclaration(classDescription,classDescription.TestedNamespace+".Test");

                var compilationUnit = SyntaxFactory.CompilationUnit()
                   .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")))
                   .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("NUnit.Framework")))
                   .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Collections.Generic")))
                     .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("Moq")))
                   .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(classDescription.TestedNamespace)))
                   .AddMembers(namespaceDeclaration);
                result.Add(classDescription.ClassName, compilationUnit.NormalizeWhitespace().ToFullString());

            }
            return result;

        }
        private NamespaceDeclarationSyntax GenerateNamespaceDeclaration(TestClassDescription classDescription, string @namespace)
        {
            return SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(@namespace))
                .AddMembers(GenerateClassDeclaration(classDescription));

        }


        private ClassDeclarationSyntax GenerateClassDeclaration(TestClassDescription classDescription)
        {

            var testMethodGenerator = new TestMethodsGenerator();
            var methods = new List<MethodDeclarationSyntax>();
           
            string actualName = classDescription.ClassName.Remove(classDescription.ClassName.Length - 4);

            MethodDescription mainConstructor = TestMethodsGenerator.GetMainConstructor(classDescription.Methods);
            methods.Add(testMethodGenerator.GenerateSetupMethod(actualName, mainConstructor));
            foreach (var methodInfo in classDescription.Methods)
            {
                if (!methodInfo.IsConstructor)
                {
                    methods.Add(testMethodGenerator.GenerateMethodDeclaration(methodInfo, TestMethodAttribute,
                   GetClassVarName(actualName)));
                }
               
            }

            List<FieldDeclarationSyntax> fields = new List<FieldDeclarationSyntax>();
            fields.Add(GeneratePrivateField(GetClassVarName(actualName), actualName));

            if (mainConstructor!=null)
            {
                foreach (string param in mainConstructor.Parameters.Keys)
                {
                    if (mainConstructor.Parameters[param].First() == 'I')
                    {
                        fields.Add(GeneratePrivateField(GetDependencyName(mainConstructor.Parameters[param]), $"Mock<{mainConstructor.Parameters[param]}>"));

                    }

                }
            }


            var attributes = SyntaxFactory.AttributeList().Attributes.Add(TestClassAtribute);
            var list = SyntaxFactory.AttributeList(attributes);
            var classDeclaration = SyntaxFactory.ClassDeclaration(classDescription.ClassName)
                                                .AddAttributeLists(list)
                                                .AddMembers(fields.ToArray())
                                                .AddMembers(methods.ToArray());
                                                
            return classDeclaration;
        }


        internal static string GetClassVarName(string className)
        {
            return "_" + className[0].ToString().ToLower() + className.Remove(0, 1);
        }


        internal static string GetDependencyName(string interfaceName)
        {
            string name = ("_" + interfaceName.First().ToString().ToLower() + interfaceName.Remove(0, 1));
            name = name = name.Replace('<', '_');
            name = name.Replace('>', '_');
            return name;
        }


        private static FieldDeclarationSyntax GeneratePrivateField(string varName, string typeName)
        {
            VariableDeclarationSyntax var = SyntaxFactory.VariableDeclaration(SyntaxFactory.ParseTypeName(typeName))
                .AddVariables(SyntaxFactory.VariableDeclarator(varName));

            return SyntaxFactory.FieldDeclaration(var)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
        }


    }
}
