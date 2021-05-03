using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace Loriot.AzureFunctions
{
    public class LoriotClient
    {
        public static async Task<List<string>> GetApplications(ILogger log)
        {
            log.LogInformation("Getting LORIOT applications...");
            using (var client = new HttpClient())
            {
                List<string> applications = new List<string>();
                JArray pagedApplications = null;
                int page = 1;
                do
                {
                    // Get paged applications
                    string url = $"{SetupApiCall(client, log)}/1/nwk/apps?page={page}&perPage=10";
                    var result = await client.GetAsync(url);
                    string resultContent = await result.Content.ReadAsStringAsync();
                    if (!result.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException(result.ReasonPhrase);
                    }
                    dynamic response = JObject.Parse(resultContent);
                    pagedApplications = response.apps;

                    // Get applications ID
                    foreach (dynamic app in pagedApplications)
                    {
                        applications.Add(app._id.ToString("X"));
                    }

                    // Repeat next page
                    page++;
                } while (pagedApplications != null && pagedApplications.Count > 0);

                log.LogInformation($"Found {applications.Count} LORIOT applications");

                return applications;
            }
        }
        public static async Task<List<string>> GetDeveuis(ILogger log, string app)
        {
            log.LogInformation($"Getting devices from LORIOT application {app}...");
            using (var client = new HttpClient())
            {
                List<string> deveuis = new List<string>();
                JArray pagedDevices = null;
                int page = 1;
                do
                {
                    // Get paged devices
                    string url = $"{SetupApiCall(client, log)}/1/nwk/app/{app}/devices?page={page}&perPage=10";
                    var result = await client.GetAsync(url);
                    string resultContent = await result.Content.ReadAsStringAsync();
                    if (!result.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException(result.ReasonPhrase);
                    }
                    dynamic response = JObject.Parse(resultContent);
                    pagedDevices = response.devices;

                    // Get deveuis
                    foreach (dynamic device in pagedDevices)
                    {
                        deveuis.Add(device._id.ToString());
                    }

                    // Repeat next page
                    page++;
                } while (pagedDevices != null && pagedDevices.Count > 0);

                log.LogInformation($"Found {deveuis.Count} LORIOT devices");

                return deveuis;
            }
        }

        private static string SetupApiCall(HttpClient client, ILogger log)
        {
            string serverUrl = System.Environment.GetEnvironmentVariable("LORIOT_SERVER");
            string apiKey = System.Environment.GetEnvironmentVariable("LORIOT_API_KEY");
            if (String.IsNullOrEmpty(serverUrl))
            {
                throw new Exception("LORIOT_SERVER not defined");
            }
            if (String.IsNullOrEmpty(apiKey))
            {
                throw new Exception("LORIOT_API_KEY not defined");
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            return "https://" + serverUrl;
        }
    }
}
