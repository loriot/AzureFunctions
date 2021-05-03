using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Loriot.AzureFunctions.DeviceProvisioning
{
    public static class ImportDevices
    {
        [FunctionName("ImportDevices")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("ImportDevices started from http trigger");

            try
            {
                long importedItemCount = await DeviceProvisioningController.FromLoriotToAzure(log);

                return new OkObjectResult($"Imported {importedItemCount} new devices to Azure Iot Hub");
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
