using System;
using System.Threading.Tasks;
using TestGeneratorLib;
using System.IO;
namespace TestGeneratorApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TestGenerator generator = new TestGenerator();

            var pathToFolder = @"..\..\TestData";
            var filesName = new string[] { "Class1.cs", "Class2.cs" };
            var destPath = @"D:\\1111";
            var t = Task.Run(async () => await new GenerationPipeline().GenerateTests(pathToFolder, filesName, destPath, 2));
            Console.WriteLine("Tests Generated");

            var files = Directory.GetFiles(destPath);

            Console.ReadLine();

        }

    }
}
