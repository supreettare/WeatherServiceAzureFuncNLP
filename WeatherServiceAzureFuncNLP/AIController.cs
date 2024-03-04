using Microsoft.AspNetCore.Http;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Planning.Handlebars;
using Newtonsoft.Json;
using WeatherServiceAzureFuncNLP.Services.Interface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Mvc;

namespace WeatherServiceAzureFuncNLP
{
    public class AIController
    {
        private readonly IKernelBase _kernelBase;

        public AIController(IKernelBase kernelBase)
        {
            _kernelBase = kernelBase;
        }

        [Function("NLPChat")]
        public async Task<IActionResult> NLPChat([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
        {
            string result = "";

            try
            {
                var kernel = _kernelBase.CreateKernel();

                //7. Enable auto function calling
                OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
                {
                    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
                };

                var arguments = new KernelArguments(openAIPromptExecutionSettings);



#pragma warning disable SKEXP0003, SKEXP0011, SKEXP0052, SKEXP0060

                //string input = req.Query["input"];
                string input = await new StreamReader(req.Body).ReadToEndAsync();

                var planner = new HandlebarsPlanner();

                arguments["input"] = input;

                var originalPlan = await planner.CreatePlanAsync(kernel, input);

                Console.WriteLine(originalPlan);

                result = await originalPlan.InvokeAsync(kernel, arguments);

                Console.WriteLine(originalPlan);
            }
            catch (JsonReaderException ex)
            {

            }
            catch (Exception ex)
            {
            }

            return new OkObjectResult(result);

        }
    }
}
