using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Loriot.AzureFunctions.DeviceProvisioning
{
    public static class ScheduledImportDevices
    {
        [FunctionName("ScheduledImportDevices")]
        public static async Task Run([TimerTrigger("%IMPORT_DEVICES_SCHEDULE_EXP%")]TimerInfo myTimer, ILogger log)
        {

            log.LogInformation("ImportDevices started from time trigger");

            try
            {
                long importedItemCount = await DeviceProvisioningController.FromLoriotToAzure(log);

                log.LogInformation($"Imported {importedItemCount} new devices to Azure Iot Hub at {DateTime.Now}");
            }
            catch (Exception e)
            {
                log.LogInformation(e.Message);
            }
        }
    }
}
