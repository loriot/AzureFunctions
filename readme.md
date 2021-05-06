# LORIOT Azure Functions Library

This repository contains useful Azure functions for LORIOT users:
- [Device Provisioning](#device-provisioning)
- [Routing](#routing)

## Device Provisioning
![ImportDevices](https://user-images.githubusercontent.com/6308233/117287563-c92f0380-ae6a-11eb-9367-74d192853816.jpg)
![ScheduledImportDevices](https://user-images.githubusercontent.com/6308233/117287574-caf8c700-ae6a-11eb-81cc-c17b7bb0233f.jpeg)

Import devices from LORIOT to Azure IoT Hub:
* **ImportDevices**: Http triggered function
* **ScheduledImportDevices**: Time triggered function to ensure regular syncing of new devices

### Environment parameters:
* **AZURE_IOT_HUB_OWNER_CONNECTION_STRING**: IoT Hub connection string—primary key for iothubowner
  > HostName=myiothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=1nMDaC6ArtFRJVG/DYlbZnxJ+gMHRv9nr8KUSoem0t0=
* **LORIOT_SERVER**: LORIOT network server url
  > eu1.loriot.io
* **LORIOT_API_KEY**: LORIOT user API key
  > AAAANgXyM8bYn71AbFBjAgd08-pkBiE50rcdb_fF8kADixHi0
* **IMPORT_DEVICES_SCHEDULE_EXP**: Schedule expression for ScheduledImportDevices time trigger
  > 0 0 0 * * *
* **LORIOT_APPS (optional)**: List of LORIOT application IDs (comma separated). If not defined, all devices from all applications will be imported.
  > BE7A2148,BE7A25C0

## Routing
![Routing](https://user-images.githubusercontent.com/6308233/117287621-d5b35c00-ae6a-11eb-82e5-bd1a969e0878.jpeg)

Route incoming LORIOT messages
* **Routing**: IoT Hub triggered function that parses every device uplink

### Environment parameters:
* **IOT_HUB_ENDPOINT**: IoT Hub Event Hub-compatible endpoint
  > Endpoint=sb://ihsuprodamres999dednamespace.servicebus.windows.net/;SharedAccessKeyName=iothubowner;SharedAccessKey=1nSEaB6AftSRJVG/DYlbZnxJ+gMHRa9nr8KUSoem0t0=;EntityPath=iothub-ehub-loriot-iot-9127777-ff7bd7b7d8
