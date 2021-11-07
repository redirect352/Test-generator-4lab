using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading.Tasks.Dataflow;

namespace TestGeneratorLib
{
   public class GenerationPipeline
    {
        public Task GenerateTests(string sourcePath, string[] fileNames, string destinationPath, int maxTasksCount)
        {
            ExecutionDataflowBlockOptions execOptions = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = maxTasksCount };
            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

            var loadSourceTextBlock = new TransformBlock<string, string>
           (
               async path =>
               {
                   using (var reader = new StreamReader(path))
                   {
                       return await reader.ReadToEndAsync();
                   }
               },
               execOptions
           );

            var testGenerationBlock = new TransformManyBlock<string, KeyValuePair<string, string>>
           (
               async Code =>
               {
                   
                   return await Task.Run(() => new TestGenerator().GenerateTests(Code));
               },
               execOptions
           );
            var resultWriteBlock = new ActionBlock<KeyValuePair<string, string>>
            (
                async fileName =>
                {
                    using (var writer = new StreamWriter(destinationPath + '\\' + fileName.Key + ".cs"))
                    {
                        await writer.WriteAsync(fileName.Value);
                    }
                },
                execOptions
            );

            loadSourceTextBlock.LinkTo(testGenerationBlock, linkOptions);
            testGenerationBlock.LinkTo(resultWriteBlock, linkOptions);
            foreach (var file in fileNames)
            {
                loadSourceTextBlock.Post(sourcePath + "\\" + file);
            }

            loadSourceTextBlock.Complete();
            return resultWriteBlock.Completion;
        }


    }
}
