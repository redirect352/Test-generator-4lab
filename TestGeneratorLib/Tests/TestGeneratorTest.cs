using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TestGeneratorLib;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;


namespace Tests
{
    [TestClass]
    public class TestGeneratorTest
    {
        private static  readonly string  pathToFolder = @"..\..\TestData";
        private static readonly string[] filesName = new string[] { "Class1.cs", "Class2.cs" };
        private static readonly string destPath = @"D:\\1111";
        private string text = "";
        private CompilationUnitSyntax root;

        [TestInitialize]
        public void Initialize()
        {
           
            var t = Task.Run(async () => await new GenerationPipeline().GenerateTests(pathToFolder, filesName, destPath, 2));
            text = File.ReadAllText(destPath + "\\Class1Test.cs");
            root = CSharpSyntaxTree.ParseText(text).GetCompilationUnitRoot();
        }


        [TestMethod]
        public void TestTestsCount()
        {       
            int count = Directory.GetFiles(destPath).Length;
            Assert.AreEqual(count,2);
        }

        [TestMethod]
        public void TestTestNames()
        {
            var files = Directory.GetFiles(destPath);
            Assert.AreEqual(files[0], destPath+ "\\Class1Test.cs");
            Assert.AreEqual(files[1], destPath + "\\Class2Test.cs");
        }


        [TestMethod]
        public void TestNamespaceStructure()
        {
            int count = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().Count();
            Assert.AreEqual(count,1);
        }

        [TestMethod]
        public void TestClassStructure()
        {

            int count = root.DescendantNodes().OfType<ClassDeclarationSyntax>().Count();
            Assert.AreEqual(count, 1);
        }

        [TestMethod]
        public void TestMethodsStructure()
        {

            int count = root.DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Where((methodDeclaration) => methodDeclaration.Modifiers.Any((modifier) => modifier.IsKind(SyntaxKind.PublicKeyword))).Count();
            Assert.AreEqual(count, 4);
        }

        [TestMethod]
        public void TestFieldsStructure()
        {

            int count = root.DescendantNodes().OfType<FieldDeclarationSyntax>()
                .Where((methodDeclaration) => methodDeclaration.Modifiers.Any((modifier) => modifier.IsKind(SyntaxKind.PrivateKeyword))).Count();
            Assert.AreEqual(count, 2);
        }


        [TestMethod]
        public void TestConstructorStructure()
        {

            int count = root.DescendantNodes().OfType<ConstructorDeclarationSyntax>().Count();
            Assert.AreEqual(count, 0);
        }
    }
}
