using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.EventHubs;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Loriot.Routing
{
    public static class Routing
    {
        [FunctionName("Routing")]
        public static void Run([IoTHubTrigger("messages/events", Connection = "IOT_HUB_ENDPOINT")]EventData message, ILogger log)
        {
            // Parse message from LORIOT
            dynamic loriotMessage = JObject.Parse(Encoding.UTF8.GetString(message.Body.Array));

            // Go to https://docs.loriot.io/display/LNS/API+Data+Format for more info about LORIOT message format
            if(loriotMessage.cmd == "rx")
            {
                // Uplink Data Message
                // https://docs.loriot.io/display/LNS/Uplink+Data+Message
                string deveui = loriotMessage.EUI;
                string port = loriotMessage.port;
                string data = loriotMessage.data;

                log.LogInformation($"Received {data} on port {port} from {deveui}");
            }else{
                log.LogInformation($"{loriotMessage}");
            }
        }
    }
}