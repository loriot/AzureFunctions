using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Loriot.AzureFunctions.DeviceProvisioning
{
    public static class DeviceProvisioningController
    {
        public static async Task<long> FromLoriotToAzure(ILogger log)
        {
            long importedItemCount = 0;

            // Get LORIOT applications
            string apps = System.Environment.GetEnvironmentVariable("LORIOT_APPS");
            if (String.IsNullOrEmpty(apps))
            {
                // LORIOT_APPS not defined, import devices from all LORIOT applications
                log.LogInformation("LORIOT_APPS not defined: all devices will be imported");
                List<string> appids = await LoriotClient.GetApplications(log);
                if(appids.Count > 0) {
                    apps = string.Join(",", appids);
                }else{
                    return 0;
                }
            }

            // For each LORIOT application
            foreach (string app in apps.Split(","))
            {
                // Get LORIOT devices
                List<string> deveuis = await LoriotClient.GetDeveuis(log, app);
                // Add to IoT HUB
                importedItemCount += await IotHubClient.AddDevices(log, deveuis);
            }

            return importedItemCount;
        }
    }
}
