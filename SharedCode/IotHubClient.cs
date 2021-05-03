using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.Azure.Devices;


namespace Loriot.AzureFunctions
{
    public class IotHubClient
    {
        public static async Task<long> AddDevices(ILogger log, List<string> deveuis)
        {
            log.LogInformation($"Adding {deveuis.Count} devices to Azure IoT Hub...");

            // Connect to IoT HUB
            string connectionString = System.Environment.GetEnvironmentVariable("IOT_HUB_OWNER_CONNECTION_STRING");
            if (String.IsNullOrEmpty(connectionString))
            {
                throw new Exception("IOT_HUB_OWNER_CONNECTION_STRING not defined");
            }
            var registryManager = RegistryManager.CreateFromConnectionString(connectionString);

            // For each deveuis
            long importedItemCount = 0, newItemCount = 0;
            foreach (string deveui in deveuis)
            {
                // Get azure device
                Device azureDevice = await registryManager.GetDeviceAsync(deveui);
                if (azureDevice == null)
                {
                    // Not found: let's add it
                    newItemCount++;
                    log.LogInformation($"Adding device {deveui}...");
                    try{
                        Device createdDevice = await registryManager.AddDeviceAsync(new Device(deveui));
                        importedItemCount++;
                    }catch(Exception e){
                        log.LogError($"Cannot add device {deveui}: {e.Message}");
                    }
                }
                else
                {
                    // Found: let's skip it
                    log.LogInformation($"Device found in Azure IoT Hub: {deveui}");
                }
            }

            log.LogInformation($"Added {importedItemCount}/{newItemCount} new devices to Azure Iot Hub");

            return importedItemCount;
        }
    }
}
