import { AzureFunction, Context } from "@azure/functions"
import { Registry } from "azure-iothub";
import axios, { AxiosResponse } from "axios"

const timerTrigger: AzureFunction = async function (context: Context, importDeviceTimer: any): Promise<void> {
    const AZURE_IOT_HUB_OWNER_CONNECTION_STRING = 'HostName=xxx;SharedAccessKeyName=iothubowner;SharedAccessKey=xxx';
    const LORIOT_SERVER = 'xx.loriot.io';
    const LORIOT_APPS = ['BE010714'];
    const LORIOT_API_KEY = 'AAAANgBB-cWE5DZIREQfVfrK4rUX--xx7MSc2t5VJeAmgjfPA';

    // Connect to Azure IoT Hub
    let registry: Registry;
    try {
        registry = Registry.fromConnectionString(AZURE_IOT_HUB_OWNER_CONNECTION_STRING)
    } catch(err) {
        return context.done(err = `Cannot connect to Azure IoT Hub: ${err}`);
    }

    console.log(`Importing devices from ${LORIOT_SERVER} ${JSON.stringify(LORIOT_APPS)}`);

    let totMissing = 0, totCreated = 0;
    for(const appId of LORIOT_APPS) {

        // Retrieve LORIOT devices
        let deveuis: string[];
        try {
            deveuis = await getAppDeveuis(LORIOT_SERVER, LORIOT_API_KEY, appId);
            console.log(`[${appId}] Found ${deveuis.length} devices on ${LORIOT_SERVER}`);
        } catch(err) {
            return context.done(err = `[${appId}] Cannot retrieve LORIOT devices: ${err}`);
        }

        // Find missing devices
        let missingDeveuis: string[] = [];
        for(const deveui of deveuis) {
            try {
                await registry.getTwin(deveui);
            }catch(err) {
                // Device not found
                missingDeveuis.push(deveui);
            }
        }
        totMissing += missingDeveuis.length;
        console.log(`[${appId}] Missing ${missingDeveuis.length} devices on Azure IoT Hub`);


        if(missingDeveuis.length > 0) {
            // Create missing devices
            let created = 0;
            for(const missingDeveui of missingDeveuis) {
                try {
                    await registry.addDevices([{deviceId: missingDeveui}]);
                    created++;
                } catch (err) {
                    console.log(`[${appId}] Cannot create ${missingDeveui} on Azure IoT Hub: ${err.message}`);
                }
            }
            totCreated += created;
            console.log(`[${appId}] Created ${created}/${missingDeveuis.length} devices on Azure IoT Hub`);
        }
    }

    if(totMissing !== totCreated) {
        context.done(`Unable to create ${totMissing - totCreated} devices on Azure IoT Hub`);
    }

};

async function getAppDeveuis(target: string, apiKey: string, appId: string): Promise<string[]> {
    return axios.get(`https://${target}/1/nwk/app/${appId}/devices`, {headers: {'Authorization': `Bearer ${apiKey}`}})
    .then((res: AxiosResponse) => {
        return res.data.devices?.map((device) => device.deveui);
    })
}

export default timerTrigger;
